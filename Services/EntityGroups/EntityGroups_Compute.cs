using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void ComputeGroups<T>( IList<Tuple<string, string[], Func<T, IDictionary<string, ISet<int>>, bool>>> matchers,
				ref IDictionary<string, ReadOnlySet<int>> groups,
				ref IDictionary<int, ReadOnlySet<string>> groups_per_ent ) where T : Entity {
			var raw_groups_per_ent = new Dictionary<int, ISet<string>>();
			
			IList<T> pool = this.GetPool<T>();

			for( int i=0; i<matchers.Count; i++ ) {
				string grp_name = matchers[i].Item1;
				string[] dependencies = matchers[i].Item2;
				var matcher_func = matchers[i].Item3;
				ISet<int> grp;

				if( !this.ComputeGroupMatch( pool, grp_name, dependencies, matcher_func, out grp ) ) {
					matchers.Add( matchers[i] );
					continue;
				}

				lock( EntityGroups.MyLock ) {
					groups[ grp_name ] = new ReadOnlySet<int>( grp );
				}

				foreach( int idx in grp ) {
					if( !raw_groups_per_ent.ContainsKey( idx ) ) {
						raw_groups_per_ent[ idx ] = new HashSet<string>();
					}
					raw_groups_per_ent[ idx ].Add( grp_name );
				}
			}

			lock( EntityGroups.MyLock ) {
				foreach( var kv in raw_groups_per_ent ) {
					groups_per_ent[ kv.Key ] = new ReadOnlySet<string>( kv.Value );
				}
			}
		}


		private bool ComputeGroupMatch<T>( IList<T> entity_pool,
				string group_name,
				string[] dependencies,
				Func<T, IDictionary<string, ISet<int>>, bool> matcher_func,
				out ISet<int> entity_ids_of_group )
				where T : Entity {
			entity_ids_of_group = new HashSet<int>();
			IDictionary<string, ISet<int>> deps = this.GetGroups<T>( group_name, dependencies );

			for( int i = 1; i < entity_pool.Count; i++ ) {
				try {
					lock( EntityGroups.MyLock ) {
						if( matcher_func( entity_pool[i], deps ) ) {
							entity_ids_of_group.Add( i );
						}
					}
				} catch( Exception ) {
					LogHelpers.Log( "EntityGroups.ComputeGroups - Compute fail for '" + group_name + "' with ent (" + i + ") " + ( entity_pool[i] == null ? "null" : entity_pool[i].ToString() ) );
				}
			}

			return true;
		}


		////////////////

		private ISet<string> _AlreadyRequeued = new HashSet<string>();

		private IDictionary<string, ISet<int>> GetGroups<T>( string group_name, string[] dependencies )
				where T : Entity {
			var deps = new Dictionary<string, ISet<int>>();
			if( dependencies == null ) { return deps; }

			IReadOnlyDictionary<string, ReadOnlySet<int>> entity_groups;

			switch( typeof( T ).Name ) {
			case "Item":
				entity_groups = this._ItemGroups;
				break;
			case "NPC":
				entity_groups = this._NPCGroups;
				break;
			case "Projectile":
				entity_groups = this._ProjGroups;
				break;
			default:
				throw new NotImplementedException();
			}

			foreach( string dependency in dependencies ) {
				if( !entity_groups.ContainsKey( dependency ) ) {
					if( this._AlreadyRequeued.Contains( group_name ) ) {
						throw new Exception( "Entity group " + group_name + " could not find dependency " + dependency + "." );
					}
					this._AlreadyRequeued.Add( group_name );

					return deps;
				}

				deps[dependency] = entity_groups[dependency];
			}

			return deps;
		}
	}
}
