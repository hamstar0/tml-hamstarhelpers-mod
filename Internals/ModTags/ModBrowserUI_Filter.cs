﻿using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModBrowserUI : ModTagsUI {
		public void FilterMods() {
			bool success;
			ReflectionHelpers.SetField( this.MyUI, "updateNeeded", true, BindingFlags.Instance | BindingFlags.NonPublic, out success );
			if( !success ) {
				LogHelpers.Log( "ModHelpers.ModBrowserUI.FilterMods - No 'updateNeeded' field." );
				return;
			}

			IList<string> mod_names = new List<string>();
			Type ui_type = this.MyUI.GetType();
			
			object items = ReflectionHelpers.GetField( this.MyUI, "items", BindingFlags.Instance | BindingFlags.NonPublic, out success );
			var items_arr = (Array)items.GetType().GetMethod( "ToArray" ).Invoke( items, new object[] { } );

			PropertyInfo special_filter_prop = ui_type.GetProperty( "SpecialModPackFilter", BindingFlags.Instance | BindingFlags.Public );
			PropertyInfo filter_title_prop = ui_type.GetProperty( "SpecialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic );

			for( int i=0; i< items_arr.Length; i++ ) {
				object item = items_arr.GetValue( i );

				string modname = (string)ReflectionHelpers.GetField( item, "mod", out success );    //UIModDownloadItem
				mod_names.Add( modname );
			}

			this.FilterModsAsync( mod_names, ( filtered_list ) => {
				special_filter_prop.SetValue( this.MyUI, filtered_list );
				filter_title_prop.SetValue( this.MyUI, "Custom Tags" ); //c/AA8888: <- ?
			} );
		}


		////////////////

		public void FilterModsAsync( IList<string> mod_names, Action<IList<string>> callback ) {
			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				IList<string> filtered_list = new List<string>();
				ISet<string> on_tags = this.GetTagsOfState( 1 );
				ISet<string> off_tags = this.GetTagsOfState( -1 );

				foreach( string mod_name in mod_names ) {
					if( !args.ModTags.ContainsKey(mod_name) ) { continue; }

					ISet<string> mod_tags = args.ModTags[mod_name];

					if( mod_tags.Overlaps( off_tags ) ) { continue; }
					if( !mod_tags.IsSupersetOf( on_tags ) ) { continue; }

					filtered_list.Add( mod_name );
				}

				callback( filtered_list );
				
				return false;
			} );
		}
	}
}
