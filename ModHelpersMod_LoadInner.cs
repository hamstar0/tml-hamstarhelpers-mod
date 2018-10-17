using HamstarHelpers.Components.Config;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Helpers.PlayerHelpers;


namespace HamstarHelpers {
	partial class ModHelpersMod : Mod {
		internal JsonConfig<HamstarHelpersConfigData> ConfigJson;
		public HamstarHelpersConfigData Config { get { return ConfigJson.Data; } }

		////

		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }

		private int LastSeenCPScreenWidth = -1;
		private int LastSeenCPScreenHeight = -1;


		private bool HasUnhandledExceptionLogger = false;



		////////////////

		public ModHelpersMod() {
			ModHelpersMod.Instance = this;

			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;

			this.InitializeOuter();

			this.ConfigJson = new JsonConfig<HamstarHelpersConfigData>(
				HamstarHelpersConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath,
				new HamstarHelpersConfigData()
			);
		}


		public override void Load() {
			this.LoadConfigs();

			if( !this.HasUnhandledExceptionLogger && this.Config.DebugModeUnhandledExceptionLogging ) {
				this.HasUnhandledExceptionLogger = true;
				AppDomain.CurrentDomain.UnhandledException += ModHelpersMod.UnhandledLogger;
			}

			this.LoadOuter();
			
			if( !this.Config.DisableControlPanelHotkey ) {
				this.ControlPanelHotkey = this.RegisterHotKey( "Toggle Control Panel", "O" );
			}
			this.DataDumpHotkey = this.RegisterHotKey( "Dump Debug Data", "P" );

			this.LoadModData();

			DataDumper.SetDumpSource( "WorldUidWithSeed", () => {
				return "  "+WorldHelpers.GetUniqueIdWithSeed();
			} );

			DataDumper.SetDumpSource( "PlayerUid", () => {
				if( Main.myPlayer < 0 || Main.myPlayer >= Main.player.Length ) {
					return "  Unobtainable";
				}
				

				return "  " + PlayerIdentityHelpers.GetProperUniqueId( Main.LocalPlayer );
			} );
		}


		private void LoadConfigs() {
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion( this ) ) {
				ErrorLogger.Log( "Mod Helpers updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}
		
		////

		public override void Unload() {
			this.Promises.FulfillModUnloadPromises();

			this.UnloadModData();
			this.UnloadOuter();
			
			ModHelpersMod.Instance = null;

			try {
				if( this.HasUnhandledExceptionLogger ) {
					this.HasUnhandledExceptionLogger = false;
					AppDomain.CurrentDomain.UnhandledException -= ModHelpersMod.UnhandledLogger;
				}
			} catch { }
		}


		////////////////

		public override void PostSetupContent() {
			this.PostSetupContentOuter();

			this.HasSetupContent = true;
			this.CheckAndProcessLoadFinish();
		}

		////////////////

		public override void AddRecipes() {
			if( this.Config.AddCrimsonLeatherRecipe ) {
				var vertebrae_to_leather = new ModRecipe( this );

				vertebrae_to_leather.AddIngredient( ItemID.Vertebrae, 5 );
				vertebrae_to_leather.SetResult( ItemID.Leather );
				vertebrae_to_leather.AddRecipe();
			}
		}

		public override void AddRecipeGroups() {
			this.AddRecipeGroupsOuter();

			this.HasAddedRecipeGroups = true;
			this.CheckAndProcessLoadFinish();
		}

		public override void PostAddRecipes() {
			this.PostAddRecipesOuter();
			
			this.HasAddedRecipes = true;
			this.CheckAndProcessLoadFinish();
		}


		////////////////

		private void CheckAndProcessLoadFinish() {
			if( !this.HasSetupContent ) { return; }
			if( !this.HasAddedRecipeGroups ) { return; }
			if( !this.HasAddedRecipes ) { return; }

			Promises.AddWorldUnloadEachPromise( () => {
				this.OnWorldExit();
			} );

			this.Promises.FulfillPostModLoadPromises();
		}


		////////////////

		public override void PreSaveAndQuit() {
			this.Promises.PreSaveAndExit();
		}


		////////////////
		
		private void OnWorldExit() {
			var myworld = this.GetModWorld<ModHelpersWorld>();
			myworld.OnWorldExit();
		}
	}
}
