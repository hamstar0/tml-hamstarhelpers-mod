using HamstarHelpers.Helpers.Debug;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/** @private */
	partial class ModHelpersMod : Mod {
		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }



		////////////////

		private void LoadFull() {
			this.LoadConfigs();
			this.LoadExceptionBehavior();

			this.LoadModules();

			this.LoadHotkeys();
			this.LoadModData();
			this.LoadDataSources();
		}


		////////////////

		private void PostSetupContentFull() {
			this.PostSetupContentModules();
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
			Services.PromisedHooks.PromisedHooks.AddWorldUnloadEachPromise( () => {
				var myworld = this.GetModWorld<ModHelpersWorld>();
				myworld.OnWorldExit();
			} );

			this.Promises.FulfillPostModLoadPromises();
		}


		////////////////

		private void UnloadFull() {
			try {
				this.Promises?.FulfillModUnloadPromises();

				this.UnloadModData();
				this.UnloadModules();
			} catch( Exception e ) {
				ErrorLogger.Log( "!ModHelpers.ModHelpersMod.UnloadInner - " + e.ToString() );
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
