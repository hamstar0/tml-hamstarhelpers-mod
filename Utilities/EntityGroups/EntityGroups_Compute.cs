using HamstarHelpers.DotNetHelpers.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void ComputeGroups<T>( IList<KeyValuePair<string, Func<T, bool>>> matchers,
				out IDictionary<string, ReadOnlySet<int>> groups,
				out IDictionary<int, ReadOnlySet<string>> groups_per_ent ) where T : Entity {
			groups = new Dictionary<string, ReadOnlySet<int>>();
			groups_per_ent = new Dictionary<int, ReadOnlySet<string>>();
			var raw_groups_per_ent = new Dictionary<int, ISet<string>>();

			var pool = this.GetPool<T>();

			foreach( var kv in matchers ) {
				string grp_name = kv.Key;
				Func<T, bool> matcher = kv.Value;
				var grp = new HashSet<int>();

				for( int i = 0; i < pool.Count; i++ ) {
					if( matcher( pool[i] ) ) {
						grp.Add( i );
					}
				}

				groups[grp_name] = new ReadOnlySet<int>( grp );

				foreach( int idx in grp ) {
					if( !raw_groups_per_ent.ContainsKey( idx ) ) {
						raw_groups_per_ent[ idx ] = new HashSet<string>();
					}
					raw_groups_per_ent[ idx ].Add( grp_name );
				}
			}

			foreach( var kv in raw_groups_per_ent ) {
				groups_per_ent[ kv.Key ] = new ReadOnlySet<string>( kv.Value );
			}
		}
	}
}
