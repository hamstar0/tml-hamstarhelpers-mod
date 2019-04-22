using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Terraria.UI;


namespace HamstarHelpers.Helpers.TmlHelpers.Menus {
	public static partial class ModMenuHelpers {
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

		public static TmodFile GetLocalMod( UIState ui ) {
			Type uiType = ui.GetType();
			FieldInfo uiLocalmodField = uiType.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( uiLocalmodField == null ) {
				LogHelpers.Warn( "No 'localMod' field in " + uiType );
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
			Type uiType = currUi.GetType();
			FieldInfo uiLocalmodField = uiType.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( uiLocalmodField == null ) {
				LogHelpers.Warn( "No 'localMod' field in " + uiType );
				return null;
			}

			object localmod = uiLocalmodField.GetValue( currUi );

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
				LogHelpers.Warn( "No 'selectedItem' list item." );
				return null;
			}

			string modName;
			if( !ReflectionHelpers.Get( modListItem, "mod", out modName ) ) {
				LogHelpers.Warn( "Invalid 'mod' data in mod browser listed entry." );
				return null;
			}

			return modName;
		}

		private static TmodFile GetLocalMod( object localmod ) {
			object rawModFile;
			if( !ReflectionHelpers.Get( localmod, "modFile", out rawModFile ) ) {
				LogHelpers.Warn( "Empty 'modFile' field" );
				return null;
			}

			return (TmodFile)rawModFile;
		}
	}
}
