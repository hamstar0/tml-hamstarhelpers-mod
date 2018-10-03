using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Reflection;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	public static class MenuModGet {
		public static object GetLocalMod( UIState ui ) {
			Type ui_type = ui.GetType();
			FieldInfo ui_localmod_field = ui_type.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( ui_localmod_field == null ) {
				LogHelpers.Log( "No 'localMod' field in " + ui_type );
				return null;
			}

			object localmod = ui_localmod_field.GetValue( ui );
			if( localmod != null ) {
				return MenuModGet.GetLocalModName( localmod );
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
				return MenuModGet.GetLocalModName( localmod );
			}

			if( prev_ui?.GetType().Name == "UIModBrowser" ) {
				return MenuModGet.GetSelectedModBrowserMod( prev_ui );
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
