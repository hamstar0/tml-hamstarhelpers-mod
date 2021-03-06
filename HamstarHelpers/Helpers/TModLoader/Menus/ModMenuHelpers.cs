﻿using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.UI;


namespace HamstarHelpers.Helpers.TModLoader.Menus {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the mod list menu.
	/// </summary>
	public partial class ModMenuHelpers {
		/// <summary>
		/// Filters the mod browser mod list to a specific set of mods, or none.
		/// </summary>
		/// <param name="filterName">Name of the filter (appears on the Download All button).</param>
		/// <param name="isFiltered">Indicates whether to simply clear the filter list and filtered state.</param>
		/// <param name="modNames">List of (internal) mod names to filter the list to.</param>
		public static void ApplyModBrowserFilter( string filterName, bool isFiltered, List<string> modNames ) {
			Type interfaceType = ReflectionHelpers.GetMainAssembly()
				.GetType( "Terraria.ModLoader.UI.Interface" );

			UIState modBrowserUi;
			if( !ReflectionHelpers.Get(interfaceType, null, "modBrowser", out modBrowserUi) || modBrowserUi == null ) {
				LogHelpers.Warn( "Could not acquire mod browser UI." );
				return;
			}

			Type uiType = modBrowserUi.GetType();
			PropertyInfo specialFilterProp = uiType.GetProperty( "SpecialModPackFilter", BindingFlags.Instance | BindingFlags.Public );
			PropertyInfo filterTitleProp = uiType.GetProperty( "SpecialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic );
			if( specialFilterProp == null ) {
				LogHelpers.Warn( "Could not acquire mod browser UI 'SpecialModPackFilter'" );
				return;
			}
			if( filterTitleProp == null ) {
				LogHelpers.Warn( "Could not acquire mod browser UI 'SpecialModPackFilterTitle'" );
				return;
			}

			object _;
			if( !ReflectionHelpers.RunMethod(modBrowserUi, "Activate", new object[] { }, out _) ) {
				LogHelpers.Warn( "Could not acquire run method 'Activate' for mod browser");
				return;
			}
			if( !ReflectionHelpers.Set(modBrowserUi, "UpdateNeeded", true) ) {
				LogHelpers.Warn( "Could not acquire set 'updateNeeded' for mod browser" );
				return;
			}
			if( !ReflectionHelpers.Set(modBrowserUi, "UpdateFilterMode", (UpdateFilter)0) ) {
				LogHelpers.Warn( "Could not acquire set 'UpdateFilterMode' for mod browser" );
				return;
			}

			UIElement inputTextUi;    //UIInputTextField
			if( ReflectionHelpers.Get(modBrowserUi, "FilterTextBox", out inputTextUi) && inputTextUi != null ) {
				if( !ReflectionHelpers.Set( inputTextUi, "_currentString", (object)"" ) ) {
					LogHelpers.Alert( "Could not acquire set '_currentString' of mod browser's filter box" );
				}
			} else {
				LogHelpers.Alert( "Could not acquire get 'FilterTextBox' from mod browser" );
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
			if( !ReflectionHelpers.Get(ui, "_localMod", out uiLocalmodField) || uiLocalmodField == null ) {
				LogHelpers.Warn( "No '_localMod' field in " + ui.GetType() );
				return null;
			}

			object localmod = uiLocalmodField.GetValue( ui );
			if( localmod != null ) {
				return ModMenuHelpers.GetLocalMod( localmod );
			}

			LogHelpers.Alert( "No mod loaded." );
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
			if( !ReflectionHelpers.Get(currUi, "_localMod", out localmod) ) {
				LogHelpers.Warn( "No '_localMod' field in " + currUi.GetType() );
				return null;
			}

			if( localmod != null ) {
				return ModMenuHelpers.GetLocalMod( localmod ).name;
			} else {
				if( prevUi?.GetType().Name == "UIModBrowser" ) {
					return ModMenuHelpers.GetSelectedModBrowserModName( prevUi );
				}
			}

			LogHelpers.Alert( "No mod loaded." );
			return null;
		}


		////

		private static string GetSelectedModBrowserModName( UIState modBrowserUi ) {
			object modDownloadItem;
			if( !ReflectionHelpers.Get( modBrowserUi, "SelectedItem", out modDownloadItem ) || modDownloadItem == null ) {
				LogHelpers.Warn( "No 'selectedItem' list item in "+modBrowserUi.GetType().ToString() );
				return null;
			}

			string modName;
			if( !ReflectionHelpers.Get(modDownloadItem, "ModName", out modName) ) {
				LogHelpers.Warn( "Invalid 'mod' data in mod browser listed entry." );
				return null;
			}

			return modName;
		}

		private static TmodFile GetLocalMod( object localmod ) {
			object rawModFile;
			if( !ReflectionHelpers.Get(localmod, "modFile", out rawModFile) || rawModFile == null ) {
				LogHelpers.Warn( "Empty 'modFile' field" );
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
			if( !ReflectionHelpers.Get( modUI, "_modInfo", out msgBox ) ) {
				output = "No _modInfo field.";
				return false;
			}

			string modDesc;
			if( !ReflectionHelpers.Get( msgBox, "_text", out modDesc ) ) {
				if( !ReflectionHelpers.Get( msgBox, "text", out modDesc ) ) {
					output = "No modInfo._text or text field.";
					return false;
				}
			}

			output = modDesc;
			return true;
		}
	}
}
