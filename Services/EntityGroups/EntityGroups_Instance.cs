using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace HamstarHelpers.Services.EntityGroups {
	/// <summary>
	/// Supplies collections of named entity groups based on traits shared between entities. Groups are either items, NPCs,
	/// or projectiles. Must be enabled on mod load to be used (note: collections may require memory).
	/// </summary>
	public partial class EntityGroups {
		private readonly static object MyLock = new object();

		private readonly static object MyValidatorKey;
		/// <summary>
		/// Used as the identifier object for binding events (cusom load hooks) to entity group loading completion.
		/// </summary>
		public readonly static CustomLoadHookValidator<object> LoadedAllValidator;



		////////////////

		static EntityGroups() {
			EntityGroups.MyValidatorKey = new object();
			EntityGroups.LoadedAllValidator = new CustomLoadHookValidator<object>( EntityGroups.MyValidatorKey );
		}



		////////////////

		private bool IsEnabled = false;

		private IDictionary<string, IReadOnlySet<int>> ItemGroups = new Dictionary<string, IReadOnlySet<int>>();
		private IDictionary<string, IReadOnlySet<int>> NPCGroups = new Dictionary<string, IReadOnlySet<int>>();
		private IDictionary<string, IReadOnlySet<int>> ProjGroups = new Dictionary<string, IReadOnlySet<int>>();

		private IDictionary<int, IReadOnlySet<string>> GroupsPerItem = new Dictionary<int, IReadOnlySet<string>>();
		private IDictionary<int, IReadOnlySet<string>> GroupsPerNPC = new Dictionary<int, IReadOnlySet<string>>();
		private IDictionary<int, IReadOnlySet<string>> GroupsPerProj = new Dictionary<int, IReadOnlySet<string>>();

		private IList<EntityGroupMatcherDefinition<Item>> CustomItemMatchers = new List<EntityGroupMatcherDefinition<Item>>();
		private IList<EntityGroupMatcherDefinition<NPC>> CustomNPCMatchers = new List<EntityGroupMatcherDefinition<NPC>>();
		private IList<EntityGroupMatcherDefinition<Projectile>> CustomProjMatchers = new List<EntityGroupMatcherDefinition<Projectile>>();

		private IList<Item> ItemPool = null;
		private IList<NPC> NPCPool = null;
		private IList<Projectile> ProjPool = null;



		////////////////

		internal EntityGroups() {
			LoadHooks.AddPostModLoadHook( () => {
				if( !this.IsEnabled ) { return; }

				this.InitializePools();
				this.InitializeDefinitions();
			} );

			//LoadHooks.AddModUnloadHook( () => {
			//	lock( EntityGroups.MyLock ) { }
			//} );
		}

		private void InitializePools() {
			this.GetItemPool();
			this.GetNPCPool();
			this.GetProjPool();
		}

		private int _InitCheck = 0;

		private void InitializeDefinitions() {
			this._InitCheck = 0;

			IList<EntityGroupMatcherDefinition<Item>> itemMatchers = null;
			IList<EntityGroupMatcherDefinition<NPC>> npcMatchers = null;
			IList<EntityGroupMatcherDefinition<Projectile>> projMatchers = null;

			try {
				lock( EntityGroups.MyLock ) {
					itemMatchers = EntityGroups.DefineItemGroupDefinitions();
					this._InitCheck++;
					npcMatchers = EntityGroups.DefineNPCGroupDefinitions();
					this._InitCheck++;
					projMatchers = EntityGroups.DefineProjectileGroupDefinitions();
					this._InitCheck++;
				}
			} catch( Exception e ) {
				LogHelpers.Warn( "Initialization failed 1 (at #" + this._InitCheck + "): " + e.ToString() );
				return;
			}

			Task.Run( () => {
				Task[] tasks = new Task[3];

				tasks[0] = Task.Run( () => {
					this.ComputeGroups<Item>( itemMatchers, this.ItemGroups, this.GroupsPerItem );
					this._InitCheck++;
				} );
				tasks[1] = Task.Run( () => {
					this.ComputeGroups<NPC>( npcMatchers, this.NPCGroups, this.GroupsPerNPC );
					this._InitCheck++;
				} );
				tasks[2] = Task.Run( () => {
					this.ComputeGroups<Projectile>( projMatchers, this.ProjGroups, this.GroupsPerProj );
					this._InitCheck++;
				} );

				try {
					Task.WaitAll( tasks );
				} catch( Exception e ) {
					LogHelpers.Warn( "Entity group compute threads failed 1: " + e.ToString() );
					return;
				}

				tasks[0] = Task.Run( () => {
					this.ComputeGroups<Item>( this.CustomItemMatchers, this.ItemGroups, this.GroupsPerItem );
					this._InitCheck++;
				} );
				tasks[1] = Task.Run( () => {
					this.ComputeGroups<NPC>( this.CustomNPCMatchers, this.NPCGroups, this.GroupsPerNPC );
					this._InitCheck++;
				} );
				tasks[2] = Task.Run( () => {
					this.ComputeGroups<Projectile>( this.CustomProjMatchers, this.ProjGroups, this.GroupsPerProj );
					this._InitCheck++;
				} );

				try {
					Task.WaitAll( tasks );
				} catch( Exception e ) {
					LogHelpers.Warn( "Entity group compute threads failed 2: " + e.ToString() );
					return;
				}

				lock( EntityGroups.MyLock ) {
					this.CustomItemMatchers = null;
					this.CustomNPCMatchers = null;
					this.CustomProjMatchers = null;
					this.ItemPool = null;
					this.NPCPool = null;
					this.ProjPool = null;
				}

				CustomLoadHooks.TriggerHook( EntityGroups.LoadedAllValidator, EntityGroups.MyValidatorKey );
				this._InitCheck++;
			} );
		}


		////////////////

		private IList<T> GetPool<T>() where T : Entity {
			IList<T> list = null;
			
			switch( typeof( T ).Name ) {
			case "Item":
				list = (IList<T>)this.GetItemPool();
				break;
			case "NPC":
				list = (IList<T>)this.GetNPCPool();
				break;
			case "Projectile":
				list = (IList<T>)this.GetProjPool();
				break;
			default:
				throw new NotImplementedException( "Invalid Entity type " + typeof( T ).Name );
			}
			
			return list;
		}

		internal IList<Item> GetItemPool() {
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

		internal IList<NPC> GetNPCPool() {
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

		internal IList<Projectile> GetProjPool() {
			if( this.ProjPool != null ) { return this.ProjPool; }
			
			var list = new Projectile[ ProjectileLoader.ProjectileCount ];
			list[0] = null;

			UnifiedRandom oldRand = Main.rand;
			Main.rand = new UnifiedRandom();
			
			for( int i = 1; i < ProjectileLoader.ProjectileCount; i++ ) {
				list[i] = new Projectile();

				try {
					list[i].SetDefaults( i );
				} catch( Exception e ) {
					LogHelpers.Log( "GetProjPool " + i + " - " + e.ToString() );
				}
			} 

			Main.rand = oldRand;

			this.ProjPool = new List<Projectile>( list );
			return this.ProjPool;
		}
	}
}
