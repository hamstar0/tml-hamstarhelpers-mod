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
		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }



		////////////////

		private void InitializeInner() {
			this.InitializeOuter();

			this.ConfigJson = new JsonConfig<HamstarHelpersConfigData>(
				HamstarHelpersConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath,
				new HamstarHelpersConfigData()
			);
		}

		////////////////

		private void LoadInner() {
			this.LoadConfigs();
			this.LoadExceptionBehavior();
			this.LoadOuter();
			this.LoadHotkeys();
			this.LoadModData();
			this.LoadDataSources();
		}

		////

		private void LoadExceptionBehavior() {
			if( this.Config.DebugModeDisableSilentLogging ) {
				var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				FieldInfo fceField = typeof( AppDomain ).GetField( "FirstChanceException", flags );
				if( fceField == null ) {
					fceField = typeof( AppDomain ).GetField( "_firstChanceException", flags );
				}
				if( fceField != null ) {
					//if( field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf( typeof(MulticastDelegate) )) ) {
					fceField.SetValue( AppDomain.CurrentDomain, null );
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

			if( this.Config.UpdateToLatestVersion() ) {
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
				return "  " + WorldHelpers.GetUniqueIdWithSeed() + " (net mode: "+Main.netMode+")";
			} );

			DataDumper.SetDumpSource( "PlayerUid", () => {
				if( Main.myPlayer < 0 || Main.myPlayer >= (Main.player.Length - 1) ) {
					return "  Unobtainable";
				}

				bool _;
				string oldUid = PlayerIdentityHelpers._GetUniqueId( Main.LocalPlayer, out _ );

				return "  " + PlayerIdentityHelpers.GetMyProperUniqueId() + " (old uid: "+oldUid+")";
			} );
		}


		////////////////

		private void PostSetupContentInner() {
			this.PostSetupContentOuter();
		}

		////////////////

		private void AddRecipesInner() {
			if( this.Config.AddCrimsonLeatherRecipe ) {
				var vertebraeToLeather = new ModRecipe( this );

				vertebraeToLeather.AddIngredient( ItemID.Vertebrae, 5 );
				vertebraeToLeather.SetResult( ItemID.Leather );
				vertebraeToLeather.AddRecipe();
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


		////////////////

		private void UnloadInner() {
			try {
				this.Promises?.FulfillModUnloadPromises();

				this.UnloadModData();
				this.UnloadOuter();
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
