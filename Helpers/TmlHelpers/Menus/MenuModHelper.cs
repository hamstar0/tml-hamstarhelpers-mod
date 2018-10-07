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
		public static void ApplyModBrowserFilter( string filter_name, bool is_filtered, List<string> mod_names ) {
			Type interface_type = Assembly.GetAssembly( typeof( ModLoader ) )
				.GetType( "Terraria.ModLoader.Interface" );

			UIState mod_browser_ui;
			if( !ReflectionHelpers.GetField<UIState>( interface_type, null, "modBrowser", BindingFlags.Static | BindingFlags.NonPublic, out mod_browser_ui ) ) {
				LogHelpers.Log( "Could not acquire mod browser UI." );
				return;
			}

			Type ui_type = mod_browser_ui.GetType();
			PropertyInfo special_filter_prop = ui_type.GetProperty( "SpecialModPackFilter", BindingFlags.Instance | BindingFlags.Public );
			PropertyInfo filter_title_prop = ui_type.GetProperty( "SpecialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic );

			object _;
			ReflectionHelpers.RunMethod<object>( mod_browser_ui, "Activate", new object[] { }, out _ );
			ReflectionHelpers.SetField( mod_browser_ui, "updateNeeded", BindingFlags.Instance | BindingFlags.NonPublic, true );
			ReflectionHelpers.SetField( mod_browser_ui, "updateFilterMode", BindingFlags.Instance | BindingFlags.Public, (UpdateFilter)0 );

			//UIElement filter_toggle;
			//ReflectionHelpers.GetProperty<UIElement>( mod_browser_ui, "UpdateFilterToggle", out filter_toggle );
			//ReflectionHelpers.SetProperty( filter_toggle, "CurrentState", 0 );

			if( is_filtered ) {
				special_filter_prop.SetValue( mod_browser_ui, mod_names );
				filter_title_prop.SetValue( mod_browser_ui, filter_name );
			} else {
				special_filter_prop.SetValue( mod_browser_ui, null );
				filter_title_prop.SetValue( mod_browser_ui, "" );
			}
		}


		////////////////

		public static object GetLocalMod( UIState ui ) {
			Type ui_type = ui.GetType();
			FieldInfo ui_localmod_field = ui_type.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( ui_localmod_field == null ) {
				LogHelpers.Log( "No 'localMod' field in " + ui_type );
				return null;
			}

			object localmod = ui_localmod_field.GetValue( ui );
			if( localmod != null ) {
				return MenuModHelper.GetLocalModName( localmod );
			}

			LogHelpers.Log( "No mod loaded." );
			return null;
		}


		public static string GetModName( UIState prev_ui, UIState curr_ui ) {
			Type ui_type = curr_ui.GetType();
			FieldInfo ui_localmod_field = ui_type.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( ui_localmod_field == null ) {
				LogHelpers.Log( "No 'localMod' field in " + ui_type );
				return null;
			}

			object localmod = ui_localmod_field.GetValue( curr_ui );
			if( localmod != null ) {
				return MenuModHelper.GetLocalModName( localmod );
			}

			if( prev_ui?.GetType().Name == "UIModBrowser" ) {
				return MenuModHelper.GetSelectedModBrowserMod( prev_ui );
			}

			LogHelpers.Log( "No mod loaded." );
			return null;
		}


		private static string GetSelectedModBrowserMod( UIState mod_browser ) {
			object mod_list_item;
			if( !ReflectionHelpers.GetField( mod_browser, "selectedItem", out mod_list_item ) ) {
				LogHelpers.Log( "No selected mod list item." );
				return null;
			}

			object raw_mod_name;
			if( !ReflectionHelpers.GetField( mod_list_item, "mod", out raw_mod_name ) ) {
				LogHelpers.Log( "Invalid mod data in mod browser listed entry." );
				return null;
			}

			return (string)raw_mod_name;
		}

		private static string GetLocalModName( object localmod ) {
			object raw_mod_file;
			if( !ReflectionHelpers.GetField( localmod, "modFile", out raw_mod_file ) ) {
				LogHelpers.Log( "Empty 'mod' field" );
				return null;
			}

			return ((TmodFile)raw_mod_file).name;
		}
	}
}
