using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Terraria.UI;


namespace HamstarHelpers.Helpers.TmlHelpers.Menus {
	public static class MenuModHelper {
		public static void ApplyModBrowserFilter( string filterName, bool isFiltered, List<string> modNames ) {
			Type interfaceType = Assembly.GetAssembly( typeof( ModLoader ) )
				.GetType( "Terraria.ModLoader.Interface" );

			UIState modBrowserUi;
			if( !ReflectionHelpers.GetField<UIState>( interfaceType, null, "modBrowser", BindingFlags.Static | BindingFlags.NonPublic, out modBrowserUi ) ) {
				LogHelpers.Log( "!ModHelpers.MenuModHelpers.ApplyModBrowserFilter - Could not acquire mod browser UI." );
				return;
			}

			Type uiType = modBrowserUi.GetType();
			PropertyInfo specialFilterProp = uiType.GetProperty( "SpecialModPackFilter", BindingFlags.Instance | BindingFlags.Public );
			PropertyInfo filterTitleProp = uiType.GetProperty( "SpecialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic );

			object _;
			ReflectionHelpers.RunMethod<object>( modBrowserUi, "Activate", new object[] { }, out _ );
			ReflectionHelpers.SetField( modBrowserUi, "updateNeeded", BindingFlags.Instance | BindingFlags.NonPublic, true );
			ReflectionHelpers.SetField( modBrowserUi, "updateFilterMode", BindingFlags.Instance | BindingFlags.Public, (UpdateFilter)0 );

			UIElement inputTextUi;    //UIInputTextField
			if( ReflectionHelpers.GetField<UIElement>( modBrowserUi, "filterTextBox", BindingFlags.Instance | BindingFlags.NonPublic, out inputTextUi ) && inputTextUi != null ) {
				ReflectionHelpers.SetField( inputTextUi, "currentString", BindingFlags.Instance | BindingFlags.NonPublic, (object)"" );
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
				LogHelpers.Log( "!ModHelpers.MenuModHelpers.GetLocalMod - No 'localMod' field in " + uiType );
				return null;
			}

			object localmod = uiLocalmodField.GetValue( ui );
			if( localmod != null ) {
				return MenuModHelper.GetLocalModName( localmod );
			}

			LogHelpers.Log( "No mod loaded." );
			return null;
		}


		public static string GetModName( UIState prevUi, UIState currUi ) {
			Type uiType = currUi.GetType();
			FieldInfo uiLocalmodField = uiType.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( uiLocalmodField == null ) {
				LogHelpers.Log( "!ModHelpers.MenuModHelpers.GetModName - No 'localMod' field in " + uiType );
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

			LogHelpers.Log( "No mod loaded." );
			return null;
		}


		private static string GetSelectedModBrowserMod( UIState modBrowser ) {
			object modListItem;
			if( !ReflectionHelpers.GetField( modBrowser, "selectedItem", out modListItem ) || modListItem == null ) {
				LogHelpers.Log( "!ModHelpers.MenuModHelpers.GetSelectedModBrowserMod - No selected mod list item." );
				return null;
			}

			string modName;
			if( !ReflectionHelpers.GetField( modListItem, "mod", out modName ) ) {
				LogHelpers.Log( "!ModHelpers.MenuModHelpers.GetSelectedModBrowserMod - Invalid mod data in mod browser listed entry." );
				return null;
			}

			return modName;
		}

		private static string GetLocalModName( object localmod ) {
			object rawModFile;
			if( !ReflectionHelpers.GetField( localmod, "modFile", out rawModFile ) ) {
				LogHelpers.Log( "!ModHelpers.MenuModHelpers.GetLocalModName - Empty 'mod' field" );
				return null;
			}

			return ((TmodFile)rawModFile).name;
		}
	}
}
