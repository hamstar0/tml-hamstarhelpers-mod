using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public void FilterMods() {
			bool success;
			IList<string> mod_names = new List<string>();

			object items = ReflectionHelpers.GetField( this.MyUI, "items", BindingFlags.Instance | BindingFlags.NonPublic, out success );
			var items_arr = (Array)items.GetType().GetMethod( "ToArray" ).Invoke( items, new object[] { } );

			for( int i = 0; i < items_arr.Length; i++ ) {
				object item = items_arr.GetValue( i );

				string mod_name = (string)ReflectionHelpers.GetField( item, "mod", out success );    //UIModDownloadItem
				mod_names.Add( mod_name );
			}

			this.FilterModsAsync( mod_names, ( is_filtered, filtered_list ) => {
				Type ui_type = this.MyUI.GetType();
				PropertyInfo special_filter_prop = ui_type.GetProperty( "SpecialModPackFilter", BindingFlags.Instance | BindingFlags.Public );
				PropertyInfo filter_title_prop = ui_type.GetProperty( "SpecialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic );

				ReflectionHelpers.SetField( this.MyUI, "updateNeeded", true, BindingFlags.Instance | BindingFlags.NonPublic, out success );

				if( is_filtered ) {
					special_filter_prop.SetValue( this.MyUI, filtered_list );
					filter_title_prop.SetValue( this.MyUI, "Custom Tags" ); //c/AA8888: <- ?
				} else {
					special_filter_prop.SetValue( this.MyUI, null );
					filter_title_prop.SetValue( this.MyUI, "" );
				}
			} );
		}


		////////////////

		public void FilterModsAsync( IList<string> mod_names, Action<bool, IList<string>> callback ) {
			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				IList<string> filtered_list = new List<string>();
				ISet<string> on_tags = this.GetTagsOfState( 1 );
				ISet<string> off_tags = this.GetTagsOfState( -1 );
				bool is_filtered = on_tags.Count > 0 || off_tags.Count > 0;

				if( is_filtered ) {
					foreach( string mod_name in mod_names ) {
						if( !args.ModTags.ContainsKey( mod_name ) ) { continue; }

						ISet<string> mod_tags = args.ModTags[mod_name];

						if( mod_tags.Overlaps( off_tags ) ) { continue; }
						if( on_tags.Count > 0 && !mod_tags.IsSupersetOf( on_tags ) ) { continue; }

						filtered_list.Add( mod_name );
					}
				}

				callback( is_filtered, filtered_list );

				return false;
			} );
		}
	}
}
