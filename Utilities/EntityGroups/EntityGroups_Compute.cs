using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		internal void ComputeItemGroups() {
			var item_pool = EntityGroups.GetItemPool();

			foreach( var kv in this.ItemMatchers ) {
				string grp_name = kv.Key;
				Func<Item, bool> matcher = kv.Value;

				var grp = new HashSet<int>();
				this._ItemGroups[grp_name] = grp;

				for( int i = 0; i < item_pool.Length; i++ ) {
					if( matcher( item_pool[i] ) ) {
						grp.Add( i );
					}
				}
			}
		}

		internal void ComputeNPCGroups() {
			var npc_pool = EntityGroups.GetNPCPool();

			foreach( var kv in this.NPCMatchers ) {
				string grp_name = kv.Key;
				Func<NPC, bool> matcher = kv.Value;

				var grp = new HashSet<int>();
				this._NPCGroups[grp_name] = grp;

				for( int i = 0; i < npc_pool.Length; i++ ) {
					if( matcher( npc_pool[i] ) ) {
						grp.Add( i );
					}
				}
			}
		}

		internal void ComputeProjectileGroups() {
			var proj_pool = EntityGroups.GetProjectilePool();

			foreach( var kv in this.ProjMatchers ) {
				string grp_name = kv.Key;
				Func<Projectile, bool> matcher = kv.Value;

				var grp = new HashSet<int>();
				this._ProjGroups[grp_name] = grp;

				for( int i = 0; i < proj_pool.Length; i++ ) {
					if( matcher( proj_pool[i] ) ) {
						grp.Add( i );
					}
				}
			}
		}
		

		////////////////

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

		private static Projectile[] GetProjectilePool() {
			Projectile[] pool = new Projectile[ProjectileLoader.ProjectileCount];

			for( int i = 0; i < ProjectileLoader.ProjectileCount; i++ ) {
				pool[i] = new Projectile();
				pool[i].SetDefaults( i );
			}
			return pool;
		}
	}
}
