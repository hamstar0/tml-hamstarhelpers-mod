using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.UI;


namespace HamstarHelpers.Helpers.TModLoader.Menus {
	/** <summary>Assorted static "helper" functions pertaining to the mod list menu.</summary> */
	public static partial class ModMenuHelpers {
		public static void ApplyModBrowserFilter( string filterName, bool isFiltered, List<string> modNames ) {
			Type interfaceType = Assembly.GetAssembly( typeof( ModLoader ) )
				.GetType( "Terraria.ModLoader.Interface" );

			UIState modBrowserUi;
			if( !ReflectionHelpers.Get(interfaceType, null, "modBrowser", out modBrowserUi) ) {
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
				LogHelpers.Warn( "Could not acquire run method 'Activate' for mod browser" );
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
				if( !ReflectionHelpers.Set( inputTextUi, "currentString", (object)"" ) ) {
					LogHelpers.Alert( "Could not acquire set 'currentString' of mod browser's filter box" );
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

		public static TmodFile GetLocalMod( UIState ui ) {
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


		public static string GetModName( UIState prevUi, UIState currUi ) {
			// = uiType.GetField( "_localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			object localmod;	// <- is a LocalMod class
			if( !ReflectionHelpers.Get(currUi, "_localMod", out localmod) || localmod == null ) {
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

		private static string GetSelectedModBrowserModName( UIState modBrowser ) {
			object modListItem;
			if( !ReflectionHelpers.Get( modBrowser, "selectedItem", out modListItem ) || modListItem == null ) {
				LogHelpers.Warn( "No 'selectedItem' list item in "+modBrowser.GetType().ToString() );
				return null;
			}

			string modName;
			if( !ReflectionHelpers.Get(modListItem, "mod", out modName) ) {
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
	}
}
