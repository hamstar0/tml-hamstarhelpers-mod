using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.UI;


namespace HamstarHelpers.Libraries.TModLoader.Menus {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the mod list menu.
	/// </summary>
	public partial class ModMenuLibraries {
		/// <summary>
		/// Filters the mod browser mod list to a specific set of mods, or none.
		/// </summary>
		/// <param name="filterName">Name of the filter (appears on the Download All button).</param>
		/// <param name="isFiltered">Indicates whether to simply clear the filter list and filtered state.</param>
		/// <param name="modNames">List of (internal) mod names to filter the list to.</param>
		public static void ApplyModBrowserFilter( string filterName, bool isFiltered, List<string> modNames ) {
			Type interfaceType = ReflectionLibraries.GetMainAssembly()
				.GetType( "Terraria.ModLoader.UI.Interface" );

			UIState modBrowserUi;
			if( !ReflectionLibraries.Get(interfaceType, null, "modBrowser", out modBrowserUi) || modBrowserUi == null ) {
				LogLibraries.Warn( "Could not acquire mod browser UI." );
				return;
			}

			Type uiType = modBrowserUi.GetType();
			PropertyInfo specialFilterProp = uiType.GetProperty( "SpecialModPackFilter", BindingFlags.Instance | BindingFlags.Public );
			PropertyInfo filterTitleProp = uiType.GetProperty( "SpecialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic );
			if( specialFilterProp == null ) {
				LogLibraries.Warn( "Could not acquire mod browser UI 'SpecialModPackFilter'" );
				return;
			}
			if( filterTitleProp == null ) {
				LogLibraries.Warn( "Could not acquire mod browser UI 'SpecialModPackFilterTitle'" );
				return;
			}

			object _;
			if( !ReflectionLibraries.RunMethod(modBrowserUi, "Activate", new object[] { }, out _) ) {
				LogLibraries.Warn( "Could not acquire run method 'Activate' for mod browser");
				return;
			}
			if( !ReflectionLibraries.Set(modBrowserUi, "UpdateNeeded", true) ) {
				LogLibraries.Warn( "Could not acquire set 'updateNeeded' for mod browser" );
				return;
			}
			if( !ReflectionLibraries.Set(modBrowserUi, "UpdateFilterMode", (UpdateFilter)0) ) {
				LogLibraries.Warn( "Could not acquire set 'UpdateFilterMode' for mod browser" );
				return;
			}

			UIElement inputTextUi;    //UIInputTextField
			if( ReflectionLibraries.Get(modBrowserUi, "FilterTextBox", out inputTextUi) && inputTextUi != null ) {
				if( !ReflectionLibraries.Set( inputTextUi, "_currentString", (object)"" ) ) {
					LogLibraries.Alert( "Could not acquire set '_currentString' of mod browser's filter box" );
				}
			} else {
				LogLibraries.Alert( "Could not acquire get 'FilterTextBox' from mod browser" );
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

		/// <summary>
		/// Retrieves the file data for a given mod within a given mod-representing menu UI (typically the Mod Info menu page).
		/// </summary>
		/// <param name="ui"></param>
		/// <returns></returns>
		public static TmodFile GetModFile( UIState ui ) {
			FieldInfo uiLocalmodField;// = uiType.GetField( "_localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( !ReflectionLibraries.Get(ui, "_localMod", out uiLocalmodField) || uiLocalmodField == null ) {
				LogLibraries.Warn( "No '_localMod' field in " + ui.GetType() );
				return null;
			}

			object localmod = uiLocalmodField.GetValue( ui );
			if( localmod != null ) {
				return ModMenuLibraries.GetLocalMod( localmod );
			}

			LogLibraries.Alert( "No mod loaded." );
			return null;
		}


		/// <summary>
		/// Gets the mod name of a mod-representing menu UI (sometimes needs the previous UI for context).
		/// </summary>
		/// <param name="prevUi"></param>
		/// <param name="currUi"></param>
		/// <returns></returns>
		public static string GetModName( UIState prevUi, UIState currUi ) {
			// = uiType.GetField( "_localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			object localmod;	// <- is a LocalMod class
			if( !ReflectionLibraries.Get(currUi, "_localMod", out localmod) ) {
				LogLibraries.Warn( "No '_localMod' field in " + currUi.GetType() );
				return null;
			}

			if( localmod != null ) {
				return ModMenuLibraries.GetLocalMod( localmod ).name;
			} else {
				if( prevUi?.GetType().Name == "UIModBrowser" ) {
					return ModMenuLibraries.GetSelectedModBrowserModName( prevUi );
				}
			}

			LogLibraries.Alert( "No mod loaded." );
			return null;
		}


		////

		private static string GetSelectedModBrowserModName( UIState modBrowserUi ) {
			object modDownloadItem;
			if( !ReflectionLibraries.Get( modBrowserUi, "SelectedItem", out modDownloadItem ) || modDownloadItem == null ) {
				LogLibraries.Warn( "No 'selectedItem' list item in "+modBrowserUi.GetType().ToString() );
				return null;
			}

			string modName;
			if( !ReflectionLibraries.Get(modDownloadItem, "ModName", out modName) ) {
				LogLibraries.Warn( "Invalid 'mod' data in mod browser listed entry." );
				return null;
			}

			return modName;
		}

		private static TmodFile GetLocalMod( object localmod ) {
			object rawModFile;
			if( !ReflectionLibraries.Get(localmod, "modFile", out rawModFile) || rawModFile == null ) {
				LogLibraries.Warn( "Empty 'modFile' field" );
				return null;
			}

			return (TmodFile)rawModFile;
		}


		////

		/// <summary>
		/// Gets the shown description text from the given mod-representing UI, if applicable.
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public static bool GetModDescriptionFromCurrentMenuUI( out string output ) {
			UIState modUI = Main.MenuUI.CurrentState;
			if( modUI == null ) {
				output = "No current UI state found.";
				return false;
			}
			if( modUI.GetType().Name != "UIModInfo" ) {
				output = "Not currently viewing mod info (or no such UI found).";
				return false;
			}

			UIPanel msgBox;
			if( !ReflectionLibraries.Get( modUI, "_modInfo", out msgBox ) ) {
				output = "No _modInfo field.";
				return false;
			}

			string modDesc;
			if( !ReflectionLibraries.Get( msgBox, "_text", out modDesc ) ) {
				if( !ReflectionLibraries.Get( msgBox, "text", out modDesc ) ) {
					output = "No modInfo._text or text field.";
					return false;
				}
			}

			output = modDesc;
			return true;
		}
	}
}
