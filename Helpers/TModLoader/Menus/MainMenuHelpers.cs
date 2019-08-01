using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;


namespace HamstarHelpers.Helpers.TModLoader.Menus {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the main menu.
	/// </summary>
	public static class MainMenuHelpers {
		public static void LoadConfigList() {
			Type interfaceType = Assembly.GetAssembly( typeof( ModLoader ) ).GetType( "Terraria.ModLoader.Interface" );

			if( !Main.gameMenu ) {
				IngameOptions.Close();
				IngameFancyUI.CoverNextFrame();
				Main.playerInventory = false;
				Main.editChest = false;
				Main.npcChatText = "";
				Main.inFancyUI = true;
			} else {
				if( !ReflectionHelpers.Get(interfaceType, null, "modConfigID", out Main.menuMode) ) {
					LogHelpers.Warn( "Could not get Interface.modConfigID" );
					return;
				}
			}

			UIState modConfigListInterfaceObj;
			if( !ReflectionHelpers.Get(interfaceType, null, "modConfigList", out modConfigListInterfaceObj)
					|| modConfigListInterfaceObj == null ) {
				LogHelpers.Warn( "Could not get Interface.modConfigList" );
				return;
			}

			Main.InGameUI.SetState( modConfigListInterfaceObj );

			Main.PlaySound( SoundID.MenuTick );
		}


		/// <summary>
		/// Loads the UI for mod configs.
		/// </summary>
		/// <param name="config"></param>
		public static void LoadConfigUI( ModConfig config ) {
			Type interfaceType = Assembly.GetAssembly( typeof( ModLoader ) ).GetType( "Terraria.ModLoader.Interface" );

			if( !Main.gameMenu ) {
				IngameOptions.Close();
				IngameFancyUI.CoverNextFrame();
				Main.playerInventory = false;
				Main.editChest = false;
				Main.npcChatText = "";
				Main.inFancyUI = true;
			} else {
				if( !ReflectionHelpers.Get( interfaceType, null, "modConfigID", out Main.menuMode ) ) {
					LogHelpers.Warn( "Could not get Interface.modConfigID" );
					return;
				}
			}

			UIState modConfigInterfaceObj;
			if( !ReflectionHelpers.Get(interfaceType, null, "modConfig", out modConfigInterfaceObj) || modConfigInterfaceObj == null ) {
				LogHelpers.Warn( "Could not get Interface.modConfig" );
				return;
			}

			object _;
			if( !ReflectionHelpers.RunMethod(modConfigInterfaceObj, "SetMod", new object[] { config.mod, config }, out _) ) {
				LogHelpers.Warn( "Could not run Interface.modConfig.SetMod" );
				return;
			}

			Main.InGameUI.SetState( modConfigInterfaceObj );

			Main.PlaySound( SoundID.MenuTick );
		}


		/// <summary>
		/// Loads the mod browser menu.
		/// </summary>
		public static void LoadModBrowser() {
			Type interfaceType = Assembly.GetAssembly( typeof( ModLoader ) ).GetType( "Terraria.ModLoader.Interface" );

			int modBrowserMenuMode;
			if( !ReflectionHelpers.Get( interfaceType, null, "modBrowserID", out modBrowserMenuMode ) ) {
				LogHelpers.Warn( "Could not switch to mod browser menu context." );
				return;
			}

			Main.PlaySound( SoundID.MenuTick );
			Main.menuMode = modBrowserMenuMode;

			UIState modBrowserUi;
			if( !ReflectionHelpers.Get( interfaceType, null, "modBrowser", out modBrowserUi ) ) {
				LogHelpers.Warn( "Could not acquire mod browser UI." );
				return;
			}

			Timers.SetTimer( "ModHelpersModDownloadPrompt", 5, () => {
				if( MenuContextService.GetCurrentMenuUI()?.GetType().Name != "UIModBrowser" ) {
					return false;
				}

				bool isLoading;
				if( !ReflectionHelpers.Get( modBrowserUi, "loading", out isLoading ) ) {
					return false;
				}

				if( isLoading ) {
					return true;
				}

				ModMenuHelpers.ApplyModBrowserFilter( "", false, new List<string>() );
				return false;
			} );
		}


		/// <summary>
		/// Loads the mod browser menu with a given set of mods to one-click bulk download
		/// (via. `ModMenuHelpers.ApplyModBrowserFilter(...)`).
		/// </summary>
		/// <param name="packTitle">Name of the set.</param>
		/// <param name="modNames">Mod (internal) names of the set.</param>
		public static void LoadModBrowserModDownloads( string packTitle, List<string> modNames ) {
			Type interfaceType = Assembly.GetAssembly( typeof( ModLoader ) )
				.GetType( "Terraria.ModLoader.Interface" );

			int modBrowserMenuMode;
			if( !ReflectionHelpers.Get( interfaceType, null, "modBrowserID", out modBrowserMenuMode ) ) {
				LogHelpers.Warn( "Could not switch to mod browser menu context." );
				return;
			}

			Main.PlaySound( SoundID.MenuTick );
			Main.menuMode = modBrowserMenuMode;

			UIState modBrowserUi;
			if( !ReflectionHelpers.Get( interfaceType, null, "modBrowser", out modBrowserUi ) ) {
				LogHelpers.Warn( "Could not acquire mod browser UI." );
				return;
			}

			Timers.SetTimer( "ModHelpersModDownloadPrompt", 5, () => {
				if( MenuContextService.GetCurrentMenuUI()?.GetType().Name != "UIModBrowser" ) {
					return false;
				}

				bool isLoading;
				if( !ReflectionHelpers.Get( modBrowserUi, "loading", out isLoading ) ) {
					return false;
				}

				if( isLoading ) {
					return true;
				}

				ModMenuHelpers.ApplyModBrowserFilter( packTitle, true, modNames );
				return false;
			} );

			/*Assembly tmlAsm = typeof( ModLoader ).Assembly;
			Type interfaceType = tmlAsm.GetType( "Interface" );

			Type uiModDlType = tmlAsm.GetType( "UIModDownloadItem" );
			object uiModDl = Activator.CreateInstance( uiModDlType, "ModName", "0.0.0", "hamstar", "", ModSide.Both, "", "http://javid.ddns.net/tModLoader/download.php?Down=mods/HamstarHelpers.tmod", 0, 0, "", false, false, null );
			//UIModDownloadItem modItem = new UIModDownloadItem( displayname, name, version, author, modreferences, modside, modIconURL, download, downloads, hot, timeStamp, update, updateIsDowngrade, installed );
			items.Add( modItem );
			
			Interface.downloadMods.SetDownloading( packTitle );
			Interface.downloadMods.SetModsToDownload( modFilter, items );
			Interface.modBrowser.updateNeeded = true;

			int menuMode;
			if( !ReflectionHelpers.GetField<int>( interfaceType, null, "downloadModsID", out menuMode ) ) {
				LogHelpers.Log( "Could not switch to downloads menu." );
				return;
			}
			Main.PlaySound( SoundID.MenuTick );
			Main.menuMode = menuMode;*/
		}
	}
}
