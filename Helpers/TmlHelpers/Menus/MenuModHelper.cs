using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Terraria.UI;


namespace HamstarHelpers.Helpers.TmlHelpers.Menus {
	public static partial class MenuModHelper {
		public static void ApplyModBrowserFilter( string filterName, bool isFiltered, List<string> modNames ) {
			Type interfaceType = Assembly.GetAssembly( typeof( ModLoader ) )
				.GetType( "Terraria.ModLoader.Interface" );

			UIState modBrowserUi;
			if( !ReflectionHelpers.Get( interfaceType, null, "modBrowser", out modBrowserUi ) ) {
				LogHelpers.Warn( "Could not acquire mod browser UI." );
				return;
			}

			Type uiType = modBrowserUi.GetType();
			PropertyInfo specialFilterProp = uiType.GetProperty( "SpecialModPackFilter", BindingFlags.Instance | BindingFlags.Public );
			PropertyInfo filterTitleProp = uiType.GetProperty( "SpecialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic );

			object _;
			ReflectionHelpers.RunMethod<object>( modBrowserUi, "Activate", new object[] { }, out _ );
			ReflectionHelpers.Set( modBrowserUi, "updateNeeded", true );
			ReflectionHelpers.Set( modBrowserUi, "updateFilterMode", (UpdateFilter)0 );

			UIElement inputTextUi;    //UIInputTextField
			if( ReflectionHelpers.Get( modBrowserUi, "filterTextBox", out inputTextUi ) && inputTextUi != null ) {
				ReflectionHelpers.Set( inputTextUi, "currentString", (object)"" );
			}

			//UIElement filterToggle;
			//ReflectionHelpers.GetProperty<UIElement>( modBrowserUi, "UpdateFilterToggle", out filterToggle );
			//ReflectionHelpers.SetProperty( filterToggle, "CurrentState", 0 );

			if( isFiltered ) {
				specialFilterProp.SetValue( modBrowserUi, modNames );
				filterTitleProp.SetValue( modBrowserUi, filterName );
			} else {
				specialFilterProp.SetValue( modBrowserUi, null );
				filterTitleProp.SetValue( modBrowserUi, "" );
			}
		}


		////////////////

		public static object GetLocalMod( UIState ui ) {
			Type uiType = ui.GetType();
			FieldInfo uiLocalmodField = uiType.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( uiLocalmodField == null ) {
				LogHelpers.Warn( "No 'localMod' field in " + uiType );
				return null;
			}

			object localmod = uiLocalmodField.GetValue( ui );
			if( localmod != null ) {
				return MenuModHelper.GetLocalModName( localmod );
			}

			LogHelpers.Alert( "No mod loaded." );
			return null;
		}


		public static string GetModName( UIState prevUi, UIState currUi ) {
			Type uiType = currUi.GetType();
			FieldInfo uiLocalmodField = uiType.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( uiLocalmodField == null ) {
				LogHelpers.Warn( "No 'localMod' field in " + uiType );
				return null;
			}

			object localmod = uiLocalmodField.GetValue( currUi );

			if( localmod != null ) {
				return MenuModHelper.GetLocalModName( localmod );
			} else {
				if( prevUi?.GetType().Name == "UIModBrowser" ) {
					return MenuModHelper.GetSelectedModBrowserMod( prevUi );
				}
			}

			LogHelpers.Alert( "No mod loaded." );
			return null;
		}


		////

		private static string GetSelectedModBrowserMod( UIState modBrowser ) {
			object modListItem;
			if( !ReflectionHelpers.Get( modBrowser, "selectedItem", out modListItem ) || modListItem == null ) {
				LogHelpers.Warn( "No selected mod list item." );
				return null;
			}

			string modName;
			if( !ReflectionHelpers.Get( modListItem, "mod", out modName ) ) {
				LogHelpers.Warn( "Invalid mod data in mod browser listed entry." );
				return null;
			}

			return modName;
		}

		private static string GetLocalModName( object localmod ) {
			object rawModFile;
			if( !ReflectionHelpers.Get( localmod, "modFile", out rawModFile ) ) {
				LogHelpers.Warn( "Empty 'mod' field" );
				return null;
			}

			return ((TmodFile)rawModFile).name;
		}


		////////////////

		public static void LoadMenuModDownloads( string packTitle, List<string> modNames ) {
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

				MenuModHelper.ApplyModBrowserFilter( packTitle, true, modNames );
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
