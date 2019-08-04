using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	/// <summary>
	/// Supplies collections of named entity groups based on traits shared between entities. Groups are either items, NPCs,
	/// or projectiles. Must be enabled on mod load to be used (note: collections may require memory).
	/// </summary>
	public partial class EntityGroups {
		private void ComputeGroups<T>( IList<EntityGroupMatcherDefinition<T>> matchers,
					ref IDictionary<string, IReadOnlySet<int>> groups,
					ref IDictionary<int, IReadOnlySet<string>> groupsPerEnt ) where T : Entity {
			var rawGroupsPerEnt = new Dictionary<int, ISet<string>>();
			
			IList<T> pool = this.GetPool<T>();

			for( int i=0; i<matchers.Count; i++ ) {
				EntityGroupMatcherDefinition<T> matcher = matchers[i];
				ISet<int> grp;

				if( !this.ComputeGroupMatch( pool, matcher, out grp ) ) {
					matchers.Add( matchers[i] );
					continue;
				}

				lock( EntityGroups.MyLock ) {
					groups[ matcher.GroupName ] = new ReadOnlySet<int>( grp );
				}

				foreach( int idx in grp ) {
					if( !rawGroupsPerEnt.ContainsKey( idx ) ) {
						rawGroupsPerEnt[ idx ] = new HashSet<string>();
					}
					rawGroupsPerEnt[ idx ].Add( matcher.GroupName );
				}
			}

			lock( EntityGroups.MyLock ) {
				foreach( var kv in rawGroupsPerEnt ) {
					groupsPerEnt[ kv.Key ] = new ReadOnlySet<string>( kv.Value );
				}
			}
		}


		private bool ComputeGroupMatch<T>( IList<T> entityPool,
					EntityGroupMatcherDefinition<T> matcher,
					out ISet<int> entityIdsOfGroup )
					where T : Entity {
			entityIdsOfGroup = new HashSet<int>();
			EntityGroupDependencies deps = this.GetGroups<T>( matcher.GroupName, matcher.GroupDependencies );

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

		private ISet<string> _AlreadyRequeued = new HashSet<string>();

		private EntityGroupDependencies GetGroups<T>( string groupName, string[] dependencies )
					where T : Entity {
			var deps = new EntityGroupDependencies();
			if( dependencies == null ) { return deps; }

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
					if( this._AlreadyRequeued.Contains( groupName ) ) {
						throw new ModHelpersException( "Entity group " + groupName + " could not find dependency " + dependency + "." );
					}
					this._AlreadyRequeued.Add( groupName );

					return deps;
				}

				deps[dependency] = entityGroups[dependency];
			}

			return deps;
		}
	}
}
