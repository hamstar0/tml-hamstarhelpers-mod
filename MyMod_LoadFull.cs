using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		private static bool IsOnExitSet = false;



		////////////////

		private static void OnExit<TEventArgs>( object sender, TEventArgs args ) {
			var mymod = ModHelpersMod.Instance;

			try {
				mymod?.UnloadModData();
			} catch( Exception e ) {
				mymod?.Logger.Warn( "!ModHelpers.ModHelpersMod.OnExit - " + e.ToString() );
			}
		}



		////////////////

		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }



		////////////////

		private void LoadFull() {
			this.LoadExceptionBehavior();

			this.LoadModules();

			this.LoadHotkeys();
			this.LoadModData();
			this.LoadDataSources();

			if( !ModHelpersMod.IsOnExitSet ) {
				ModHelpersMod.IsOnExitSet = true;
				Main.instance.Exiting += ModHelpersMod.OnExit;
			}
		}


		////////////////

		private void PostSetupContentFull() {
			this.PostSetupFullModules();
		}

		////

		private void AddRecipesFull() {
			this.AddRecipesInternal();
		}

		private void AddRecipeGroupsFull() {
			this.AddRecipeGroupsModules();
		}

		private void PostAddRecipesFull() {
			this.PostAddRecipesModules();
		}


		////////////////

		private void PostLoadFull() {
			this.LoadHooks.FulfillPostModLoadHooks();

			Services.Hooks.LoadHooks.LoadHooks.AddWorldUnloadEachHook( () => {
				var myworld = ModContent.GetInstance<ModHelpersWorld>();
				myworld.OnWorldExit();
			} );
		}


		////////////////

		private void UnloadFull() {
			try {
				this.LoadHooks?.FulfillModUnloadHooks();

				this.UnloadModData();
				this.UnloadModules();
			} catch( Exception e ) {
				this.Logger.Warn( "!ModHelpers.ModHelpersMod.UnloadFull - " + e.ToString() );	//was Error(...)
			}

			try {
				if( this.HasUnhandledExceptionLogger ) {
					this.HasUnhandledExceptionLogger = false;
					AppDomain.CurrentDomain.UnhandledException -= ModHelpersMod.UnhandledLogger;
				}
			} catch { }
		}
	}
}
