using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.LoadHooks {
	public partial class LoadHooks {
		private IList<Action> PostModLoadHooks = new List<Action>();
		private IList<Action> ModUnloadHooks = new List<Action>();
		private IList<Action> WorldLoadOnceHooks = new List<Action>();
		private IList<Action> WorldLoadEachHooks = new List<Action>();
		private IList<Action> PostWorldLoadOnceHooks = new List<Action>();
		private IList<Action> PostWorldLoadEachHooks = new List<Action>();
		private IList<Action> WorldUnloadOnceHooks = new List<Action>();
		private IList<Action> WorldUnloadEachHooks = new List<Action>();
		private IList<Action> PostWorldUnloadOnceHooks = new List<Action>();
		private IList<Action> PostWorldUnloadEachHooks = new List<Action>();
		private IList<Action> WorldInPlayOnceHooks = new List<Action>();
		private IList<Action> WorldInPlayEachHooks = new List<Action>();
		private IList<Action> SafeWorldLoadOnceHooks = new List<Action>();
		private IList<Action> SafeWorldLoadEachHooks = new List<Action>();
		
		private bool PostModLoadHookConditionsMet = false;
		private bool WorldLoadHookConditionsMet = false;
		private bool WorldUnloadHookConditionsMet = false;
		private bool PostWorldUnloadHookConditionsMet = false;
		private bool WorldInPlayHookConditionsMet = false;
		private bool SafeWorldLoadHookConditionsMet = false;
		
		private IDictionary<CustomLoadHookValidator, List<Func<CustomLoadHookArguments, bool>>> CustomHooks = new Dictionary<CustomLoadHookValidator, List<Func<CustomLoadHookArguments, bool>>>();
		private ISet<CustomLoadHookValidator> CustomHookConditionsMet = new HashSet<CustomLoadHookValidator>();
		private IDictionary<CustomLoadHookValidator, CustomLoadHookArguments> CustomHookArgs = new Dictionary<CustomLoadHookValidator, CustomLoadHookArguments>();

		private Func<bool> OnTickGet;



		////////////////

		internal LoadHooks() {
			this.OnTickGet = Timers.Timers.MainOnTickGet();
			Main.OnTick += LoadHooks._Update;
		}

		~LoadHooks() {
			try {
				Main.OnTick -= LoadHooks._Update;

				if( this.WorldLoadHookConditionsMet && !this.WorldUnloadHookConditionsMet ) {
					this.FulfillWorldUnloadHooks();
					this.FulfillPostWorldUnloadHooks();
				}
			} catch { }
		}


		internal void OnPostSetupContent() {
			LoadHooks.AddWorldLoadEachHook( () => {
				this.WorldUnloadHookConditionsMet = false;
				this.PostWorldUnloadHookConditionsMet = false;
			} );
		}


		////////////////

		internal void PreSaveAndExit() {
			this.FulfillWorldUnloadHooks();
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.LoadHooks == null ) { return; }
			
			if( mymod.LoadHooks.OnTickGet() ) {	// <- Throttles to 60fps
				mymod.LoadHooks.Update();
			}
		}

		private void Update() {
			if( Main.netMode != 2 ) {
				if( this.WorldLoadHookConditionsMet && Main.gameMenu ) {
					this.WorldLoadHookConditionsMet = false;
					this.SafeWorldLoadHookConditionsMet = false;
				}
			}

			if( this.WorldUnloadHookConditionsMet ) {
				if( Main.gameMenu && Main.menuMode == 0 ) {
					this.FulfillPostWorldUnloadHooks();
				}
			}
		}
	}
}
