using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	/// <summary>
	/// Supplies collections of named entity groups based on traits shared between entities. Groups are either items, NPCs,
	/// or projectiles. Must be enabled on mod load to be used (note: collections may require memory).
	/// </summary>
	public partial class EntityGroups {
		private bool ComputeGroups<T>(
					IList<EntityGroupMatcherDefinition<T>> matchers,
					IDictionary<string, IReadOnlySet<int>> groups,
					IDictionary<int, IReadOnlySet<string>> groupsPerEnt )
					where T : Entity {
			IDictionary<int, ISet<string>> rawGroupsPerEnt;
			if( !this.GetComputedGroups( matchers, groups, out rawGroupsPerEnt ) ) {
				return false;
			}

			lock( EntityGroups.MyLock ) {
				foreach( var kv in rawGroupsPerEnt ) {
					groupsPerEnt[ kv.Key ] = new ReadOnlySet<string>( kv.Value );
				}
			}

			return true;
		}

		private bool GetComputedGroups<T>(
					IList<EntityGroupMatcherDefinition<T>> matchers,
					IDictionary<string, IReadOnlySet<int>> groups,
					out IDictionary<int, ISet<string>> groupsPerEnt )
					where T : Entity {
			groupsPerEnt = new Dictionary<int, ISet<string>>();
			var reQueuedCounts = new Dictionary<EntityGroupMatcherDefinition<T>, int>();
			IList<T> entityPool = this.GetPool<T>();

			for( int i = 0; i < matchers.Count; i++ ) {
				EntityGroupMatcherDefinition<T> matcher = matchers[i];
				ISet<int> grp;

				try {
					if( !this.ComputeGroupMatch( entityPool, matcher, out grp ) ) {
						reQueuedCounts.AddOrSet( matchers[i], 1 );
						matchers.Add( matchers[i] );

						if( reQueuedCounts[matchers[i]] > 100 ) {
							LogHelpers.Warn( "Could not find all dependencies for " + matcher.GroupName );
							return false;
						}
						continue;
					}

					lock( EntityGroups.MyLock ) {
						groups[ matcher.GroupName ] = new ReadOnlySet<int>( grp );
					}

					foreach( int idx in grp ) {
						groupsPerEnt.Set2D( idx, matcher.GroupName );
					}
				} catch( Exception e ) {
					LogHelpers.Warn( "Failed (at #" + i + "): " + e.ToString() );
				}
			}

			return true;
		}


		private bool ComputeGroupMatch<T>( IList<T> entityPool,
					EntityGroupMatcherDefinition<T> matcher,
					out ISet<int> entityIdsOfGroup )
					where T : Entity {
			entityIdsOfGroup = new HashSet<int>();
			EntityGroupDependencies deps;
			
			if( !this.GetGroups<T>(matcher.GroupName, matcher.GroupDependencies, out deps) ) {
				return false;
			}

			for( int i = 1; i < entityPool.Count; i++ ) {
				try {
					lock( EntityGroups.MyLock ) {
						if( matcher.Matcher.MatcherFunc(entityPool[i], deps) ) {
							entityIdsOfGroup.Add( i );
						}
					}
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
