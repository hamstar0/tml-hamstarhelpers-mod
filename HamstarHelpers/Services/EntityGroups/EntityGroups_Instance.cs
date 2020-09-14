using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Threading;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;


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

		private bool IsEnabledSinceLoad = false;

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
				if( !this.IsEnabledSinceLoad ) { return; }

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

			//

			TaskLauncher.Run( (token) => {
				Task[] tasks = new Task[3];

				//

				tasks[0] = Task.Run( () => {
					this.ComputeGroups<Item>( token, itemMatchers, this.ItemGroups, this.GroupsPerItem );
					this._InitCheck++;
				}, token );
				tasks[1] = Task.Run( () => {
					this.ComputeGroups<NPC>( token, npcMatchers, this.NPCGroups, this.GroupsPerNPC );
					this._InitCheck++;
				}, token );
				tasks[2] = Task.Run( () => {
					this.ComputeGroups<Projectile>( token, projMatchers, this.ProjGroups, this.GroupsPerProj );
					this._InitCheck++;
				}, token );

				try {
					Task.WaitAll( tasks );
				} catch( Exception e ) {
					LogHelpers.Warn( "Entity group compute threads failed 1: " + e.ToString() );
					return;
				}

				//

				tasks[0] = Task.Run( () => {
					this.ComputeGroups<Item>( token, this.CustomItemMatchers, this.ItemGroups, this.GroupsPerItem );
					this._InitCheck++;
				}, token );
				tasks[1] = Task.Run( () => {
					this.ComputeGroups<NPC>( token, this.CustomNPCMatchers, this.NPCGroups, this.GroupsPerNPC );
					this._InitCheck++;
				}, token );
				tasks[2] = Task.Run( () => {
					this.ComputeGroups<Projectile>( token, this.CustomProjMatchers, this.ProjGroups, this.GroupsPerProj );
					this._InitCheck++;
				}, token );

				try {
					Task.WaitAll( tasks );
				} catch( Exception e ) {
					LogHelpers.Warn( "Entity group compute threads failed 2: " + e.ToString() );
					return;
				}

				//

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
	}
}
