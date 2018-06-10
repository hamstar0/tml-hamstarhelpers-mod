using HamstarHelpers.DebugHelpers;
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

		public static void AddCustomItemGroup( string name, Func<Item, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = HamstarHelpersMod.Instance.EntityGroups;
				if( ent_grps.CustomItemMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<Item, bool>>( name, matcher );
				ent_grps.CustomItemMatchers.Add( entry );
			}
		}

		public static void AddCustomNPCGroup( string name, Func<NPC, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = HamstarHelpersMod.Instance.EntityGroups;
				if( ent_grps.CustomNPCMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<NPC, bool>>( name, matcher );
				ent_grps.CustomNPCMatchers.Add( entry );
			}
		}

		public static void AddCustomProjectileGroup( string name, Func<Projectile, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = HamstarHelpersMod.Instance.EntityGroups;
				if( ent_grps.CustomProjMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<Projectile, bool>>( name, matcher );
				ent_grps.CustomProjMatchers.Add( entry );
			}
		}


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

		private IDictionary<string, ReadOnlySet<int>> _RawItemGroups = new Dictionary<string, ReadOnlySet<int>>();
		private IDictionary<string, ReadOnlySet<int>> _RawNPCGroups = new Dictionary<string, ReadOnlySet<int>>();
		private IDictionary<string, ReadOnlySet<int>> _RawProjGroups = new Dictionary<string, ReadOnlySet<int>>();

		private IDictionary<int, ReadOnlySet<string>> _RawGroupsPerItem = new Dictionary<int, ReadOnlySet<string>>();
		private IDictionary<int, ReadOnlySet<string>> _RawGroupsPerNPC = new Dictionary<int, ReadOnlySet<string>>();
		private IDictionary<int, ReadOnlySet<string>> _RawGroupsPerProj = new Dictionary<int, ReadOnlySet<string>>();

		private IList<KeyValuePair<string, Func<Item, bool>>> CustomItemMatchers = new List<KeyValuePair<string, Func<Item, bool>>>();
		private IList<KeyValuePair<string, Func<NPC, bool>>> CustomNPCMatchers = new List<KeyValuePair<string, Func<NPC, bool>>>();
		private IList<KeyValuePair<string, Func<Projectile, bool>>> CustomProjMatchers = new List<KeyValuePair<string, Func<Projectile, bool>>>();

		private IList<Item> ItemPool = null;
		private IList<NPC> NPCPool = null;
		private IList<Projectile> ProjPool = null;


		////////////////

		internal EntityGroups() {
			this._ItemGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this._RawItemGroups );
			this._NPCGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this._RawNPCGroups );
			this._ProjGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this._RawProjGroups );

			this._GroupsPerItem = new ReadOnlyDictionary<int, ReadOnlySet<string>>( this._RawGroupsPerItem );
			this._GroupsPerNPC = new ReadOnlyDictionary<int, ReadOnlySet<string>>( this._RawGroupsPerNPC );
			this._GroupsPerProj = new ReadOnlyDictionary<int, ReadOnlySet<string>>( this._RawGroupsPerProj );

			TmlLoadHelpers.AddPostModLoadPromise( () => {
				lock( EntityGroups.MyLock ) {
					IList<KeyValuePair<string, Func<Item, bool>>> item_matchers = this.DefineItemGroups();
					IList<KeyValuePair<string, Func<NPC, bool>>> npc_matchers = this.DefineNPCGroups();
					IList<KeyValuePair<string, Func<Projectile, bool>>> proj_matchers = this.DefineProjectileGroups();
					
					this.ComputeGroups<Item>( item_matchers, ref this._RawItemGroups, ref this._RawGroupsPerItem );
					this.ComputeGroups<NPC>( npc_matchers, ref this._RawNPCGroups, ref this._RawGroupsPerNPC );
					this.ComputeGroups<Projectile>( proj_matchers, ref this._RawProjGroups, ref this._RawGroupsPerProj );

					this.ComputeGroups<Item>( this.CustomItemMatchers, ref this._RawItemGroups, ref this._RawGroupsPerItem );
					this.ComputeGroups<NPC>( this.CustomNPCMatchers, ref this._RawNPCGroups, ref this._RawGroupsPerNPC );
					this.ComputeGroups<Projectile>( this.CustomProjMatchers, ref this._RawProjGroups, ref this._RawGroupsPerProj );

					this.CustomItemMatchers = null;
					this.CustomNPCMatchers = null;
					this.CustomProjMatchers = null;
					this.ItemPool = null;
					this.NPCPool = null;
					this.ProjPool = null;
				}
			} );
		}


		////////////////

		private IList<T> GetPool<T>() where T : Entity {
			IList<T> list = null;
			
			switch( typeof( T ).Name ) {
			case "Item":
				list =( IList<T>)this.GetItemPool();
				break;
			case "NPC":
				list = (IList<T>)this.GetNPCPool();
				break;
			case "Projectile":
				list = (IList<T>)this.GetProjPool();
				break;
			default:
				throw new NotImplementedException();
			}
			
			return list;
		}

		private IList<Item> GetItemPool() {
			if( this.ItemPool != null ) { return this.ItemPool; }

			var list = new Item[ ItemLoader.ItemCount ];
			list[0] = null;
			
			for( int i = 1; i < ItemLoader.ItemCount; i++ ) {
				list[i] = new Item();
				list[i].SetDefaults( i, true );
			}

			this.ItemPool = new List<Item>( list );
			return this.ItemPool;
		}

		private IList<NPC> GetNPCPool() {
			if( this.NPCPool != null ) { return this.NPCPool; }
			
			var list = new NPC[ NPCLoader.NPCCount ];
			list[0] = null;

			for( int i = 1; i < NPCLoader.NPCCount; i++ ) {
				list[i] = new NPC();
				list[i].SetDefaults( i );
			}

			this.NPCPool = new List<NPC>( list );
			return this.NPCPool;
		}

		private IList<Projectile> GetProjPool() {
			if( this.ProjPool != null ) { return this.ProjPool; }
			
			var list = new Projectile[ ProjectileLoader.ProjectileCount ];
			list[0] = null;

			for( int i = 1; i < ProjectileLoader.ProjectileCount; i++ ) {
				list[i] = new Projectile();
				list[i].SetDefaults( i );
			}

			this.ProjPool = new List<Projectile>( list );
			return this.ProjPool;
		}
	}
}
