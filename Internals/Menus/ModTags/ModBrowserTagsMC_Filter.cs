using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader.UI;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		internal void FilterMods() {
			IList<string> modNames = new List<string>();
			UIState myUid = this.MyUI;

			object items;
			if( !ReflectionHelpers.GetField( myUid, "items", BindingFlags.Instance | BindingFlags.NonPublic, out items ) ) {
				throw new HamstarException( "No 'items' field in ui " + myUid );
			}

			var itemsArr = (Array)items.GetType()
				.GetMethod( "ToArray" )
				.Invoke( items, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				object item = itemsArr.GetValue( i );
				string modName;

				if( ReflectionHelpers.GetField<string>( item, "mod", out modName ) ) {
					modNames.Add( modName );
				}
			}

			this.FilterModsAsync( modNames, ( isFiltered, filteredList, onTagCount, offTagCount ) => {
				string filterName = "Tags";
				if( onTagCount > 0 || offTagCount > 0 ) {
					filterName += " ";

					if( onTagCount > 0 ) {
						filterName += "+" + onTagCount;
						if( offTagCount > 0 ) {
							filterName += " ";
						}
					}
					if( offTagCount > 0 ) {
						filterName += "-" + offTagCount;
					}
				}

				ReflectionHelpers.SetField( myUid, "updateFilterMode", BindingFlags.Instance | BindingFlags.Public, (UpdateFilter)0 );
				MenuModHelper.ApplyModBrowserFilter( filterName, isFiltered, (List<string>)filteredList );
			} );
		}


		////////////////

		public void FilterModsAsync( IList<string> modNames, Action<bool, IList<string>, int, int> callback ) {
			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				if( !args.Found ) {
					this.InfoDisplay?.SetText( "Could not acquire mod data." );
					callback( false, new List<string>(), 0, 0 );
					return false;
				}
				
				IList<string> filteredList = new List<string>();
				ISet<string> onTags = this.GetTagsOfState( 1 );
				ISet<string> offTags = this.GetTagsOfState( -1 );
				bool isFiltered = onTags.Count > 0 || offTags.Count > 0;

				if( isFiltered ) {
					foreach( string modName in modNames ) {
						if( !args.ModTags.ContainsKey( modName ) ) { continue; }

						ISet<string> modTags = args.ModTags[modName];

						if( modTags.Overlaps( offTags ) ) { continue; }
						if( onTags.Count > 0 && !modTags.IsSupersetOf( onTags ) ) { continue; }

						filteredList.Add( modName );
					}
				}

				callback( isFiltered, filteredList, onTags.Count, offTags.Count );

				return false;
			} );
		}
	}
}
