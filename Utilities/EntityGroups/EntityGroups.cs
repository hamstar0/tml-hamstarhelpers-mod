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

		
		////////////////

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


		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerItem {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._GroupsPerItem;
				}
			}
		}
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerNPC {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._GroupsPerNPC;
				}
			}
		}
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerProj {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._GroupsPerProj;
				}
			}
		}



		////////////////

		private IReadOnlyDictionary<string, ReadOnlySet<int>> _ItemGroups = null;
		private IReadOnlyDictionary<string, ReadOnlySet<int>> _NPCGroups = null;
		private IReadOnlyDictionary<string, ReadOnlySet<int>> _ProjGroups = null;

		private IReadOnlyDictionary<int, ReadOnlySet<string>> _GroupsPerItem = null;
		private IReadOnlyDictionary<int, ReadOnlySet<string>> _GroupsPerNPC = null;
		private IReadOnlyDictionary<int, ReadOnlySet<string>> _GroupsPerProj = null;

		private IList<Item> ItemPool = null;
		private IList<NPC> NPCPool = null;
		private IList<Projectile> ProjPool = null;


		////////////////

		internal EntityGroups() {
			TmlLoadHelpers.AddPostModLoadPromise( () => {
				lock( EntityGroups.MyLock ) {
					IList<KeyValuePair<string, Func<Item, bool>>> item_matchers = this.DefineGroups();
					var npc_matchers = new List<KeyValuePair<string, Func<NPC, bool>>>();
					var proj_matchers = new List<KeyValuePair<string, Func<Projectile, bool>>>();

					IDictionary<string, ReadOnlySet<int>> raw_item_grps;
					IDictionary<string, ReadOnlySet<int>> raw_npc_grps;
					IDictionary<string, ReadOnlySet<int>> raw_proj_grps;
					IDictionary<int, ReadOnlySet<string>> raw_grps_per_item;
					IDictionary<int, ReadOnlySet<string>> raw_grps_per_npc;
					IDictionary<int, ReadOnlySet<string>> raw_grps_per_proj;

					this.ComputeGroups( item_matchers, out raw_item_grps, out raw_grps_per_item );
					this.ComputeGroups( npc_matchers, out raw_npc_grps, out raw_grps_per_npc );
					this.ComputeGroups( proj_matchers, out raw_proj_grps, out raw_grps_per_proj );

					this._ItemGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( raw_item_grps );
					this._NPCGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( raw_npc_grps );
					this._ProjGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( raw_proj_grps );

					this._GroupsPerItem = new ReadOnlyDictionary<int, ReadOnlySet<string>>( raw_grps_per_item );
					this._GroupsPerNPC = new ReadOnlyDictionary<int, ReadOnlySet<string>>( raw_grps_per_npc );
					this._GroupsPerProj = new ReadOnlyDictionary<int, ReadOnlySet<string>>( raw_grps_per_proj );
				}

				this.ItemPool = null;
				this.NPCPool = null;
				this.ProjPool = null;
			} );
		}


		////////////////

		private IList<T> GetPool<T>() where T : Entity {
			switch( typeof( T ).Name ) {
			case "Item":
				return (IList<T>)this.GetItemPool();
			case "NPC":
				return (IList<T>)this.GetNPCPool();
			case "Projectile":
				return (IList<T>)this.GetProjPool();
			default:
				throw new NotImplementedException();
			}
		}

		private IList<Item> GetItemPool() {
			if( this.ItemPool != null ) { return this.ItemPool; }

			this.ItemPool = new List<Item>( ItemLoader.ItemCount );

			for( int i = 0; i < ItemLoader.ItemCount; i++ ) {
				this.ItemPool[i] = new Item();
				this.ItemPool[i].SetDefaults( i, true );
			}
			return this.ItemPool;
		}

		private IList<NPC> GetNPCPool() {
			if( this.NPCPool != null ) { return this.NPCPool; }

			this.NPCPool = new List<NPC>( NPCLoader.NPCCount );

			for( int i = 0; i < NPCLoader.NPCCount; i++ ) {
				this.NPCPool[i] = new NPC();
				this.NPCPool[i].SetDefaults( i );
			}
			return this.NPCPool;
		}

		private IList<Projectile> GetProjPool() {
			if( this.ProjPool != null ) { return this.ProjPool; }
			
			this.ProjPool = new List<Projectile>( ProjectileLoader.ProjectileCount );

			for( int i = 0; i < ProjectileLoader.ProjectileCount; i++ ) {
				this.ProjPool[i] = new Projectile();
				this.ProjPool[i].SetDefaults( i );
			}
			return this.ProjPool;
		}
	}
}
