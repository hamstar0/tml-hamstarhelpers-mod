using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	/// <summary>
	/// Supplies collections of named entity groups based on traits shared between entities. Groups are either items, NPCs,
	/// or projectiles. Must be enabled on mod load to be used (note: collections may require memory).
	/// </summary>
	public partial class EntityGroups {
		private bool GetComputedGroup<T>(
					EntityGroupMatcherDefinition<T> matcher,
					IList<EntityGroupMatcherDefinition<T>> matchers,
					IList<T> entityPool,
					IDictionary<EntityGroupMatcherDefinition<T>, int> reQueuedCounts,
					IDictionary<string, IReadOnlySet<int>> groups,
					IDictionary<int, ISet<string>> groupsPerEnt,
					out bool fail )
					where T : Entity {
			fail = false;
			ISet<int> grp;

			if( !this.ComputeGroupMatch(entityPool, matcher, matchers, out grp ) ) {
				lock( EntityGroups.MatchersLock ) {
					matchers.Add( matcher );
				}

				lock( EntityGroups.ReQueueLock ) {
					reQueuedCounts.AddOrSet( matcher, 1 );

					if( reQueuedCounts[matcher] > 100 ) {
						LogHelpers.Warn( "Could not find all dependencies for " + matcher.GroupName );
						fail = true;
					}
				}

				return false;
			}

			lock( EntityGroups.MyLock ) {
				groups[ matcher.GroupName ] = new ReadOnlySet<int>( grp );
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
					IList<EntityGroupMatcherDefinition<T>> matchers,
					out ISet<int> entityIdsOfGroup )
					where T : Entity {
			entityIdsOfGroup = new HashSet<int>();
			EntityGroupDependencies deps;

			bool groupsFound = this.GetGroupsAsDependencies<T>(
				matcher.GroupName,
				matcher.GroupDependencies,
				out deps
			);
			if( !groupsFound ) {
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
lock( EntityGroups.MatchersLock ) {
LogHelpers.Log( "ComputeGroupMatch "+typeof(T).Name+" (pool="+entityPool.GetType().GenericTypeArguments?.First().Name+" "+entityPool.Count+")"
	+",\n  matcher: "+matcher?.GroupName+", matchers type "+matchers.First()?.GetType().GenericTypeArguments?.First().Name
	+",\n  entityIdsOfGroup ("+entityIdsOfGroup?.Count+"): "+(entityIdsOfGroup != null ? string.Join(", ", entityIdsOfGroup) : "")
);
}
			
			return true;
		}


		////////////////

		private bool GetGroupsAsDependencies<T>(
					string groupName,
					string[] dependencies,
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
