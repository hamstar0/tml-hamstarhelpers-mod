using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModBrowserUI : ModTagsUI {
		public void FilterMods() {
			bool success;
			ReflectionHelpers.SetField( this.MyUI, "updateNeeded", false, BindingFlags.NonPublic | BindingFlags.Instance, out success );
			if( !success ) {
				LogHelpers.Log( "ModHelpers.ModBrowserUI.FilterMods - No 'updateNeeded' field." );
				return;
			}

			IList<string> filtered_list = new List<string>();

			ISet<string> on_tags = this.GetTagsOfState( 1 );
			ISet<string> off_tags = this.GetTagsOfState( -1 );
			Type ui_type = this.MyUI.GetType();

			IEnumerable items = (IEnumerable)ReflectionHelpers.GetProperty( this.MyUI, "items", BindingFlags.Instance | BindingFlags.NonPublic, out success );
			PropertyInfo special_filter_prop = ui_type.GetProperty( "SpecialModPackFilter", BindingFlags.Instance | BindingFlags.Public );
			PropertyInfo filter_title_prop = ui_type.GetProperty( "SpecialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic );

			foreach( object item in items ) {
				string modname = (string)ReflectionHelpers.GetField( item, "mod", out success );    //UIModDownloadItem

				if( this.ModHasAllTags(modname, on_tags) && !this.ModHasAnyTags(modname, off_tags) ) {
					filtered_list.Add( modname );
				}
			}

			special_filter_prop.SetValue( this.MyUI, filtered_list );
			filter_title_prop.SetValue( this.MyUI, "c/AA8888:Custom Tags" );
		}
	}
}
