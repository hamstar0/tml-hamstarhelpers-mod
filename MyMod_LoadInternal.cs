using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.World;
using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Players;
using System.Reflection;
using HamstarHelpers.Services.Debug.DataDumper;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		private void PostInitializeInternal() {
			this.ConfigJson = new JsonConfig<HamstarHelpersConfigData>(
				HamstarHelpersConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath,
				new HamstarHelpersConfigData()
			);
		}


		////////////////
		

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
				return "  " + WorldHelpers.GetUniqueId(true) + " (net mode: " + Main.netMode + ")";
			} );

			DataDumper.SetDumpSource( "PlayerUid", () => {
				if( Main.myPlayer < 0 || Main.myPlayer >= ( Main.player.Length - 1 ) ) {
					return "  Unobtainable";
				}

				return "  " + PlayerIdentityHelpers.GetUniqueId();
			} );
		}


		////////////////
		
		private void AddRecipesInternal() {
			if( this.Config.AddCrimsonLeatherRecipe ) {
				var vertebraeToLeather = new ModRecipe( this );

				vertebraeToLeather.AddIngredient( ItemID.Vertebrae, 5 );
				vertebraeToLeather.SetResult( ItemID.Leather );
				vertebraeToLeather.AddRecipe();
			}
		}
	}
}
