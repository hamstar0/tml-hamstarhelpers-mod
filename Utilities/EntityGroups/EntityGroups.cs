using HamstarHelpers.DotNetHelpers.DataStructures;
using HamstarHelpers.TmlHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private static object MyLock = new object();


		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ItemGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._ItemGroups;
				}
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> NPCGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._NPCGroups;
				}
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ProjectileGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._ProjGroups;
				}
			}
		}



		////////////////

		private IReadOnlyDictionary<string, ReadOnlySet<int>> _ItemGroups = null;
		private IReadOnlyDictionary<string, ReadOnlySet<int>> _NPCGroups = null;
		private IReadOnlyDictionary<string, ReadOnlySet<int>> _ProjGroups = null;

		private Item[] ItemPool = null;
		private NPC[] NPCPool = null;
		private Projectile[] ProjPool = null;


		////////////////

		internal EntityGroups() {
			TmlLoadHelpers.AddPostModLoadPromise( () => {
				lock( EntityGroups.MyLock ) {
					IList<KeyValuePair<string, Func<Item, bool>>> item_matchers = this.DefineGroups();
					var npc_matchers = new List<KeyValuePair<string, Func<NPC, bool>>>();
					var proj_matchers = new List<KeyValuePair<string, Func<Projectile, bool>>>();

					this._ItemGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this.ComputeGroups( item_matchers ) );
					this._NPCGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this.ComputeGroups( npc_matchers ) );
					this._ProjGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this.ComputeGroups( proj_matchers ) );
				}

				this.ItemPool = null;
				this.NPCPool = null;
				this.ProjPool = null;
			} );
		}


		////////////////

		private Item[] GetItemPool() {
			if( this.ItemPool != null ) { return this.ItemPool; }

			this.ItemPool = new Item[ItemLoader.ItemCount];

			for( int i = 0; i < ItemLoader.ItemCount; i++ ) {
				this.ItemPool[i] = new Item();
				this.ItemPool[i].SetDefaults( i, true );
			}
			return this.ItemPool;
		}

		private NPC[] GetNPCPool() {
			if( this.NPCPool != null ) { return this.NPCPool; }

			this.NPCPool = new NPC[NPCLoader.NPCCount];

			for( int i = 0; i < NPCLoader.NPCCount; i++ ) {
				this.NPCPool[i] = new NPC();
				this.NPCPool[i].SetDefaults( i );
			}
			return this.NPCPool;
		}

		private Projectile[] GetProjPool() {
			if( this.ProjPool != null ) { return this.ProjPool; }

			this.ProjPool = new Projectile[ProjectileLoader.ProjectileCount];

			for( int i = 0; i < ProjectileLoader.ProjectileCount; i++ ) {
				this.ProjPool[i] = new Projectile();
				this.ProjPool[i].SetDefaults( i );
			}
			return this.ProjPool;
		}
	}
}
