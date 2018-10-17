using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private ISet<string> AlreadyRequeued = new HashSet<string>();


		private void ComputeGroups<T>( IList<Tuple<string, string[], Func<T, IDictionary<string, ISet<int>>, bool>>> matchers,
				ref IDictionary<string, ReadOnlySet<int>> groups,
				ref IDictionary<int, ReadOnlySet<string>> groups_per_ent ) where T : Entity {
			var raw_groups_per_ent = new Dictionary<int, ISet<string>>();
			
			IList<T> pool = this.GetPool<T>();

			for( int i=0; i<matchers.Count; i++ ) {
				string grp_name = matchers[i].Item1;
				ISet<int> grp;

				if( !this.ComputeGroupMatch( pool, grp_name, matchers[i].Item2, matchers[i].Item3, out grp ) ) {
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
				Func<T, IDictionary<string, ISet<int>>, bool> matcher,
				out ISet<int> entity_ids_of_group )
				where T : Entity {
			entity_ids_of_group = new HashSet<int>();
			var dep = new Dictionary<string, ISet<int>>();

			if( dependencies != null ) {
				IReadOnlyDictionary<string, ReadOnlySet<int>> entity_groups;

				switch( typeof(T).Name ) {
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
					if( !entity_groups.ContainsKey(dependency) ) {
						if( this.AlreadyRequeued.Contains(group_name) ) {
							throw new Exception( "Entity group "+group_name+" could not find dependency "+dependency+"." );
						}
						this.AlreadyRequeued.Add( group_name );

						return false;
					}
				}
			}

			for( int i = 1; i < entity_pool.Count; i++ ) {
				try {
					lock( EntityGroups.MyLock ) {
						if( matcher( entity_pool[i], dep ) ) {
							entity_ids_of_group.Add( i );
						}
					}
				} catch( Exception ) {
					LogHelpers.Log( "EntityGroups.ComputeGroups - Compute fail for '" + group_name + "' with ent (" + i + ") " + ( entity_pool[i] == null ? "null" : entity_pool[i].ToString() ) );
				}
			}

			return true;
		}
	}
}
