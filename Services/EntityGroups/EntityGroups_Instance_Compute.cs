using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Items.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	/// <summary>
	/// Supplies collections of named entity groups based on traits shared between entities. Groups are either items, NPCs,
	/// or projectiles. Must be enabled on mod load to be used (note: collections may require memory).
	/// </summary>
	public partial class EntityGroups {
		private static object ComputeLock = new object();
		private static object ReQueueLock = new object();
		private static object MatchersLock = new object();



		////////////////

		private bool ComputeGroups<T>(
					IList<EntityGroupMatcherDefinition<T>> matchers,
					IDictionary<string, IReadOnlySet<int>> groups,
					IDictionary<int, IReadOnlySet<string>> groupsPerEnt )
					where T : Entity {
			IDictionary<int, ISet<string>> rawGroupsPerEnt;
			int failedAt = this.GetComputedGroupsThreaded( matchers, groups, out rawGroupsPerEnt );

			lock( EntityGroups.MyLock ) {
				lock( EntityGroups.MatchersLock ) {
					foreach( EntityGroupMatcherDefinition<T> def in matchers ) {
						if( !groups.ContainsKey( def.GroupName ) ) {
							LogHelpers.Log( "!Entity group " + def.GroupName + " not loaded." );
						}
					}
				}
			}

			if( failedAt != -1 ) {
//LogHelpers.Log( "ent:" + typeof( T ).Name + ", !OK " + failedAt+", processed:"+groups.Count );
				return false;
			}

			lock( EntityGroups.MyLock ) {
				lock( EntityGroups.ComputeLock ) {
					foreach( var kv in rawGroupsPerEnt ) {
						groupsPerEnt[kv.Key] = new ReadOnlySet<string>( kv.Value );
					}
				}
			}

			//LogHelpers.Log( "ent:" + typeof( T ).Name + ", OK " + groups.Count );
			return true;
		}

		private int GetComputedGroupsThreaded<T>(
					IList<EntityGroupMatcherDefinition<T>> matchers,
					IDictionary<string, IReadOnlySet<int>> groups,
					out IDictionary<int, ISet<string>> groupsPerEnt )
					where T : Entity {
			var myGroupsPerEnt = new Dictionary<int, ISet<string>>();
			var reQueuedCounts = new Dictionary<EntityGroupMatcherDefinition<T>, int>();
			IList<T> entityPool = this.GetPool<T>();

			int failedAt = -1;
			int i = 0, count;

			lock( EntityGroups.MatchersLock ) {
				count = matchers.Count;
			}

			do {
				//for( i = 0; i < matchers.Count; i++ ) {
				Parallel.For( i, count, ( j ) => {
					if( failedAt != -1 ) { return; }

					EntityGroupMatcherDefinition<T> matcher;
					lock( EntityGroups.MatchersLock ) {
						matcher = matchers[j];
					}

					try {
						failedAt = this.GetComputedGroup(
							matcher,
							matchers,
							entityPool,
							reQueuedCounts,
							groups,
							myGroupsPerEnt
						)
							? -1
							: (failedAt == -1 ? j : failedAt);
					} catch( Exception e ) {
						LogHelpers.Warn( "Failed (at #" + j + "): " + e.ToString() );
						failedAt = j;
					}
				} );

//LogHelpers.Log( "ent:"+typeof(T).Name+", i:"+i+", count:"+count+", real count:"+matchers.Count+", failed? at:"+failedAt);
				lock( EntityGroups.MatchersLock ) {
					i = count;
					count = matchers.Count;
				}
			} while( failedAt == -1 && i < matchers.Count );

			groupsPerEnt = myGroupsPerEnt;
			return failedAt;
		}

		private bool GetComputedGroup<T>(
					EntityGroupMatcherDefinition<T> matcher,
					IList<EntityGroupMatcherDefinition<T>> matchers,
					IList<T> entityPool,
					IDictionary<EntityGroupMatcherDefinition<T>, int> reQueuedCounts,
					IDictionary<string, IReadOnlySet<int>> groups,
					IDictionary<int, ISet<string>> groupsPerEnt )
					where T : Entity {
			ISet<int> grp;

			if( !this.ComputeGroupMatch( entityPool, matcher, out grp ) ) {
				lock( EntityGroups.MatchersLock ) {
					matchers.Add( matcher );
				}

				lock( EntityGroups.ReQueueLock ) {
					reQueuedCounts.AddOrSet( matcher, 1 );

					if( reQueuedCounts[matcher] > 100 ) {
						LogHelpers.Warn( "Could not find all dependencies for " + matcher.GroupName );
						return false;
					}
				}

				return true;
			}

			lock( EntityGroups.MyLock ) {
				groups[ matcher.GroupName ] = new ReadOnlySet<int>( grp );

				if( ModHelpersConfig.Instance.DebugModeEntityGroupDisplay ) {
					switch( matcher.GroupName ) {
					case "Any Item":
					case "Any NPC":
					case "Any Projectile":
						break;
					default:
						LogHelpers.Log( "\"" + matcher.GroupName + "\" - [\""
							+ string.Join( "\", \"", grp.SafeSelect(
								itemType => ItemAttributeHelpers.GetQualifiedName( itemType ) )
							)
							+ "\"]"
						);
						break;
					}
				}
			}

			lock( EntityGroups.ComputeLock ) {
				foreach( int grpIdx in grp ) {
					groupsPerEnt.Set2D( grpIdx, matcher.GroupName );
				}
			}

			return true;
		}


		private bool ComputeGroupMatch<T>(
					IList<T> entityPool,
					EntityGroupMatcherDefinition<T> matcher,
					out ISet<int> entityIdsOfGroup )
					where T : Entity {
			entityIdsOfGroup = new HashSet<int>();
			EntityGroupDependencies deps;
			
			if( !this.GetGroups<T>(matcher.GroupName, matcher.GroupDependencies, out deps) ) {
				return false;
			}

			for( int i = 1; i < entityPool.Count; i++ ) {
				if( (i == 102 || i == 221) && entityPool[i].GetType() == typeof(Projectile) ) {
					continue;	// Go away, log warning spam
				}

				try {
					//lock( EntityGroups.MyLock ) {
					if( matcher.Matcher.MatcherFunc(entityPool[i], deps) ) {
						entityIdsOfGroup.Add( i );
					}
					//}
				} catch( Exception ) {
					LogHelpers.Alert( "Compute fail for '"+matcher.GroupName+"' with ent ("+i+") "+(entityPool[i] == null ? "null" : entityPool[i].ToString()) );
				}
			}

			return true;
		}


		////////////////

		private bool GetGroups<T>(
					string groupName, string[] dependencies,
					out EntityGroupDependencies groupDependencies )
					where T : Entity {
			groupDependencies = new EntityGroupDependencies();
			if( dependencies == null || dependencies.Length == 0 ) {
				return true;
			}

			IDictionary<string, IReadOnlySet<int>> entityGroups;

			switch( typeof(T).Name ) {
			case "Item":
				entityGroups = this.ItemGroups;
				break;
			case "NPC":
				entityGroups = this.NPCGroups;
				break;
			case "Projectile":
				entityGroups = this.ProjGroups;
				break;
			default:
				throw new NotImplementedException( "Invalid Entity type " + typeof( T ).Name );
			}

			foreach( string dependency in dependencies ) {
				if( !entityGroups.ContainsKey( dependency ) ) {
					return false;
				}

				groupDependencies[dependency] = entityGroups[dependency];
			}

			return true;
		}
	}
}
