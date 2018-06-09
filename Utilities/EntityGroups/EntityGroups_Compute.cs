using HamstarHelpers.DotNetHelpers.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private static Item[] GetItemPool() {
			Item[] pool = new Item[ItemLoader.ItemCount];

			for( int i = 0; i < ItemLoader.ItemCount; i++ ) {
				pool[i] = new Item();
				pool[i].SetDefaults( i, true );
			}
			return pool;
		}

		private static NPC[] GetNPCPool() {
			NPC[] pool = new NPC[NPCLoader.NPCCount];

			for( int i = 0; i < NPCLoader.NPCCount; i++ ) {
				pool[i] = new NPC();
				pool[i].SetDefaults( i );
			}
			return pool;
		}

		private static Projectile[] GetProjPool() {
			Projectile[] pool = new Projectile[ProjectileLoader.ProjectileCount];

			for( int i = 0; i < ProjectileLoader.ProjectileCount; i++ ) {
				pool[i] = new Projectile();
				pool[i].SetDefaults( i );
			}
			return pool;
		}


		////////////////

		private static IDictionary<string, ReadOnlySet<int>> ComputeGroups( IList<KeyValuePair<string, Func<Item, bool>>> matchers ) {
			var pool = EntityGroups.GetItemPool();
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

		private static IDictionary<string, ReadOnlySet<int>> ComputeGroups( IList<KeyValuePair<string, Func<NPC, bool>>> matchers ) {
			var pool = EntityGroups.GetNPCPool();
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

		private static IDictionary<string, ReadOnlySet<int>> ComputeGroups( IList<KeyValuePair<string, Func<Projectile, bool>>> matchers ) {
			var pool = EntityGroups.GetProjPool();
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
