using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
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
		private static object GroupsLock = new object();



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
					EntityGroups.LogGroup( typeof(T), groupName, entIds );
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

			bool isFailed = false;
			int failedAt = -1;
			int i = 0, count;

			lock( EntityGroups.MatchersLock ) {
				count = matchers.Count;
			}

			do {
				//for( i = 0; i < matchers.Count; i++ ) {
				Parallel.For( i, count, (j) => {
					lock( EntityGroups.GroupsLock ) {
						if( failedAt != -1 ) { return; }
					}

					EntityGroupMatcherDefinition<T> matcher;
					lock( EntityGroups.MatchersLock ) {
						matcher = matchers[j];
					}

					lock( EntityGroups.GroupsLock ) {
						bool success = this.ComputeGroup(
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
					}
				} );

//LogHelpers.Log( "ent:"+typeof(T).Name+", i:"+i+", count:"+count+", real count:"+matchers.Count+", failed? at:"+failedAt);
				lock( EntityGroups.MatchersLock ) {
					i = count;
					count = matchers.Count;
				}

				lock( EntityGroups.GroupsLock ) {
					isFailed = failedAt != -1;
				}
			} while( !isFailed && i < matchers.Count );

			groupsPerEnt = myGroupsPerEnt;
			return failedAt;
		}
	}
}
