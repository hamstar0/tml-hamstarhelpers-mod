using HamstarHelpers.Libraries.World;
using HamstarHelpers.Libraries.Debug;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Players;
using System.Reflection;
using HamstarHelpers.Services.Debug.DataDumper;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		private void LoadExceptionBehavior() {
			if( ModHelpersConfig.Instance.DebugModeDisableSilentLogging ) {
				var flags = Libraries.DotNET.Reflection.ReflectionLibraries.MostAccess;
				FieldInfo fceField = typeof( AppDomain ).GetField( "FirstChanceException", flags );
				if( fceField == null ) {
					fceField = typeof( AppDomain ).GetField( "_firstChanceException", flags );
				}
				if( fceField != null ) {
					//if( field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf( typeof(MulticastDelegate) )) ) {
					fceField.SetValue( AppDomain.CurrentDomain, null );
				}
			}

			if( !this.HasUnhandledExceptionLogger && ModHelpersConfig.Instance.DebugModeUnhandledExceptionLogging ) {
				this.HasUnhandledExceptionLogger = true;
				AppDomain.CurrentDomain.UnhandledException += ModHelpersMod.UnhandledLogger;
			}
		}


		private void LoadHotkeys() {
			this.ControlPanelHotkey = this.RegisterHotKey( "Toggle Control Panel", "O" );
			this.DataDumpHotkey = this.RegisterHotKey( "Dump Debug Data", "P" );
		}


		private void LoadDataSources() {
			DataDumper.SetDumpSource( "WorldUidWithSeed", () => {
				return "  " + WorldLibraries.GetUniqueIdForCurrentWorld(true) + " (net mode: " + Main.netMode + ")";
			} );

			DataDumper.SetDumpSource( "PlayerUid", () => {
				if( Main.myPlayer < 0 || Main.myPlayer >= ( Main.player.Length - 1 ) ) {
					return "  Unobtainable";
				}

				return "  " + PlayerIdentityLibraries.GetUniqueId();
			} );
		}


		////////////////
		
		private void AddRecipesInternal() {
			if( ModHelpersConfig.Instance.AddCrimsonLeatherRecipe ) {
				var vertebraeToLeather = new ModRecipe( this );

				vertebraeToLeather.AddIngredient( ItemID.Vertebrae, 5 );
				vertebraeToLeather.SetResult( ItemID.Leather );
				vertebraeToLeather.AddRecipe();
			}
		}
	}
}
