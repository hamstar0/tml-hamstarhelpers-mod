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
using System.Reflection;


namespace HamstarHelpers {
	partial class ModHelpersMod : Mod {
		internal JsonConfig<HamstarHelpersConfigData> ConfigJson;
		public HamstarHelpersConfigData Config { get { return ConfigJson.Data; } }

		////

		private int LastSeenCPScreenWidth = -1;
		private int LastSeenCPScreenHeight = -1;


		private bool HasUnhandledExceptionLogger = false;



		////////////////

		private void InitializeInner() {
			this.InitializeOuter();

			this.ConfigJson = new JsonConfig<HamstarHelpersConfigData>(
				HamstarHelpersConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath,
				new HamstarHelpersConfigData()
			);
		}


		private void LoadInner() {
			this.LoadConfigs();
			this.LoadExceptionBehavior();
			this.LoadOuter();
			this.LoadHotkeys();
			this.LoadModData();
			this.LoadDataSources();
		}


		private void LoadExceptionBehavior() {
			if( this.Config.DebugModeDisableSilentLogging ) {
				var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				FieldInfo fce_field = typeof( AppDomain ).GetField( "FirstChanceException", flags );
				if( fce_field == null ) {
					fce_field = typeof( AppDomain ).GetField( "_firstChanceException", flags );
				}
				if( fce_field != null ) {
					//if( field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf( typeof(MulticastDelegate) )) ) {
					fce_field.SetValue( AppDomain.CurrentDomain, null );
				}
			}

			if( !this.HasUnhandledExceptionLogger && this.Config.DebugModeUnhandledExceptionLogging ) {
				this.HasUnhandledExceptionLogger = true;
				AppDomain.CurrentDomain.UnhandledException += ModHelpersMod.UnhandledLogger;
			}
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


		private void LoadHotkeys() {
			if( !this.Config.DisableControlPanelHotkey ) {
				this.ControlPanelHotkey = this.RegisterHotKey( "Toggle Control Panel", "O" );
			}
			this.DataDumpHotkey = this.RegisterHotKey( "Dump Debug Data", "P" );
		}


		private void LoadDataSources() {
			DataDumper.SetDumpSource( "WorldUidWithSeed", () => {
				return "  " + WorldHelpers.GetUniqueIdWithSeed();
			} );

			DataDumper.SetDumpSource( "PlayerUid", () => {
				if( Main.myPlayer < 0 || Main.myPlayer >= Main.player.Length ) {
					return "  Unobtainable";
				}

				return "  " + PlayerIdentityHelpers.GetProperUniqueId( Main.LocalPlayer );
			} );
		}


		////

		private void UnloadInner() {
			this.Promises.FulfillModUnloadPromises();

			this.UnloadModData();
			this.UnloadOuter();

			try {
				if( this.HasUnhandledExceptionLogger ) {
					this.HasUnhandledExceptionLogger = false;
					AppDomain.CurrentDomain.UnhandledException -= ModHelpersMod.UnhandledLogger;
				}
			} catch { }
		}


		////////////////

		private void PostSetupContentInner() {
			this.PostSetupContentOuter();
		}

		////////////////

		private void AddRecipesInner() {
			if( this.Config.AddCrimsonLeatherRecipe ) {
				var vertebrae_to_leather = new ModRecipe( this );

				vertebrae_to_leather.AddIngredient( ItemID.Vertebrae, 5 );
				vertebrae_to_leather.SetResult( ItemID.Leather );
				vertebrae_to_leather.AddRecipe();
			}
		}

		private void AddRecipeGroupsInner() {
			this.AddRecipeGroupsOuter();
		}

		private void PostAddRecipesInner() {
			this.PostAddRecipesOuter();
		}


		////////////////

		private void PostLoadAll() {
			Promises.AddWorldUnloadEachPromise( () => {
				this.OnWorldExit();
			} );

			this.Promises.FulfillPostModLoadPromises();
		}


		////////////////

		private void OnWorldExit() {
			var myworld = this.GetModWorld<ModHelpersWorld>();
			myworld.OnWorldExit();
		}
	}
}
