using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Items.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
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

			int failedAt = this.GetComputedGroupsThreaded(
				matchers: matchers,
				groups: groups,
				groupsPerEnt: out rawGroupsPerEnt
			);

			lock( EntityGroups.MyLock ) {
				lock( EntityGroups.MatchersLock ) {
					foreach( EntityGroupMatcherDefinition<T> def in matchers ) {
						if( !groups.ContainsKey( def.GroupName ) ) {
							LogHelpers.Log( "!Entity group " + def.GroupName + " not loaded." );
						}
					}

					if( ModHelpersConfig.Instance.DebugModeEntityGroupDisplay ) {
						ModHelpersMod.Instance.Logger.Info( typeof(T).Name+" has groups "+string.Join(", ", groups.Keys) );
					}
				}
			}

			if( failedAt != -1 ) {
//LogHelpers.Log( "ent:" + typeof( T ).Name + ", !OK " + failedAt+", processed:"+groups.Count );
				return false;
			}

			lock( EntityGroups.MyLock ) {
				lock( EntityGroups.ComputeLock ) {
					foreach( (int itemType, ISet<string> groupNames) in rawGroupsPerEnt ) {
						groupsPerEnt[ itemType ] = new ReadOnlySet<string>( groupNames );
					}
				}
			}

			//

			if( ModHelpersConfig.Instance.DebugModeEntityGroupDisplay ) {
				foreach( (string groupName, IReadOnlySet<int> entIds) in groups ) {
					switch( groupName ) {
					case "Any Item":
					case "Any NPC":
					case "Any Projectile":
						break;
					default:
						IList<string> entNames = entIds.SafeSelect(
							itemType => ItemAttributeHelpers.GetQualifiedName( itemType )
						).ToList();

						var entNameChunks = new List<string>();
						var chunk = new List<string>();

						for( int i=0; i<entNames.Count; i++ ) {
							chunk.Add( entNames[i] );
							if( chunk.Count >= 10 ) {
								entNameChunks.Add( string.Join(", ", chunk) );
								chunk.Clear();
							}
						}
						if( chunk.Count > 0 ) {
							entNameChunks.Add( string.Join( ", ", chunk ) );
						}

						ModHelpersMod.Instance.Logger.Info( "\"" + groupName + "\" (" + typeof( T ).Name + ") - "
							+ ( entIds.Count > 0 ? "[\n  " : "[" )
							+ string.Join( ",\n  ", entNameChunks )
							+ ( entIds.Count > 0 ? "\n]" : "]" )
						);
						break;
					}
				}
			}

			//

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
						bool success = this.GetComputedGroup(
							matcher,
							matchers,
							entityPool,
							reQueuedCounts,
							groups,
							myGroupsPerEnt
						);

						failedAt = success
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
	}
}
