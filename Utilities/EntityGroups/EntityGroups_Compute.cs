using HamstarHelpers.DotNetHelpers.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private IDictionary<string, ReadOnlySet<int>> ComputeGroups( IList<KeyValuePair<string, Func<Item, bool>>> matchers ) {
			var pool = this.GetItemPool();
			var groups = new Dictionary<string, ReadOnlySet<int>>();

			foreach( var kv in matchers ) {
				string grp_name = kv.Key;
				Func<Item, bool> matcher = kv.Value;
				var grp = new HashSet<int>();

				for( int i = 0; i < pool.Length; i++ ) {
					if( matcher( pool[i] ) ) {
						grp.Add( i );
					}
				}

				groups[grp_name] = new ReadOnlySet<int>( grp );
			}

			return groups;
		}

		private IDictionary<string, ReadOnlySet<int>> ComputeGroups( IList<KeyValuePair<string, Func<NPC, bool>>> matchers ) {
			var pool = this.GetNPCPool();
			var groups = new Dictionary<string, ReadOnlySet<int>>();

			foreach( var kv in matchers ) {
				string grp_name = kv.Key;
				Func<NPC, bool> matcher = kv.Value;
				var grp = new HashSet<int>();
				
				for( int i = 0; i < pool.Length; i++ ) {
					if( matcher( pool[i] ) ) {
						grp.Add( i );
					}
				}

				groups[grp_name] = new ReadOnlySet<int>( grp );
			}

			return groups;
		}

		private IDictionary<string, ReadOnlySet<int>> ComputeGroups( IList<KeyValuePair<string, Func<Projectile, bool>>> matchers ) {
			var pool = this.GetProjPool();
			var groups = new Dictionary<string, ReadOnlySet<int>>();

			foreach( var kv in matchers ) {
				string grp_name = kv.Key;
				Func<Projectile, bool> matcher = kv.Value;
				var grp = new HashSet<int>();

				for( int i = 0; i < pool.Length; i++ ) {
					if( matcher( pool[i] ) ) {
						grp.Add( i );
					}
				}

				groups[grp_name] = new ReadOnlySet<int>( grp );
			}

			return groups;
		}
	}
}
