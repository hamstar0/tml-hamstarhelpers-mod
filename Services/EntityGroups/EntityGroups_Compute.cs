using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void ComputeGroups<T>( IList<KeyValuePair<string, Func<T, bool>>> matchers,
				ref IDictionary<string, ReadOnlySet<int>> groups,
				ref IDictionary<int, ReadOnlySet<string>> groups_per_ent ) where T : Entity {
			var raw_groups_per_ent = new Dictionary<int, ISet<string>>();
			
			IList<T> pool = this.GetPool<T>();

			foreach( var kv in matchers ) {
				string grp_name = kv.Key;
				Func<T, bool> matcher = kv.Value;
				var grp = new HashSet<int>();
				
				for( int i = 1; i < pool.Count; i++ ) {
					try {
						if( matcher( pool[i] ) ) {
							grp.Add( i );
						}
					} catch( Exception ) {
						LogHelpers.Log( "EntityGroups.ComputeGroups - Compute fail for '" + grp_name+"' with ent ("+i+") "+(pool[i]==null?"null":pool[i].ToString()) );
					}
				}
				
				groups[ grp_name ] = new ReadOnlySet<int>( grp );

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
