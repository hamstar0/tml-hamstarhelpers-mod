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
			bool success;

			var mod_list_item = ReflectionHelpers.GetField( mod_browser, "selectedItem", out success );
			if( !success ) {
				LogHelpers.Log( "No selected mod list item." );
				return null;
			}

			string mod_name = (string)ReflectionHelpers.GetField( mod_list_item, "mod", out success );
			if( !success ) {
				LogHelpers.Log( "Invalid mod data in mod browser listed entry." );
				return null;
			}

			return mod_name;
		}

		private static string GetLocalModName( object localmod ) {
			bool success;
			var modfile = (TmodFile)ReflectionHelpers.GetField( localmod, "modFile", out success );
			if( !success ) {
				LogHelpers.Log( "Empty 'mod' field" );
				return null;
			}

			return modfile.name;
		}
	}
}
