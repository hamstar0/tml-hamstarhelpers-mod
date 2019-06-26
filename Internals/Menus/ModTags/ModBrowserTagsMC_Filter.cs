using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/** @private */
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		internal void FilterMods() {
			IList<string> modNames = new List<string>();
			UIState myUI = this.MyUI;

			object items;
			if( !ReflectionHelpers.Get( myUI, "_items", out items ) ) {
				LogHelpers.Warn( "No 'items' field in ui " + myUI );
				return;
			}

			var itemsArr = (Array)items.GetType()
				.GetMethod( "ToArray" )
				.Invoke( items, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				object item = itemsArr.GetValue( i );
				string modName;

				if( ReflectionHelpers.Get( item, "mod", out modName ) ) {
					modNames.Add( modName );
				}
			}

			this.FilterModsAsync( modNames, ( isFiltered, filteredModNameList, onTagCount, offTagCount ) => {
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

				if( ReflectionHelpers.Set( myUI, "updateFilterMode", (UpdateFilter)0 ) ) {
					ModMenuHelpers.ApplyModBrowserFilter( filterName, isFiltered, (List<string>)filteredModNameList );
				} else {
					LogHelpers.Alert( "Could not set updateFilterMode for the mod browser" );
				}
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

				this.FilterMods_Promised( modNames, callback, args.ModTags );
				return false;
			} );
		}


		private void FilterMods_Promised( IList<string> modNames, Action<bool, IList<string>, int, int> callback,
				IDictionary<string, ISet<string>> modTags ) {
			IList<string> filteredModNameList = new List<string>();
			ISet<string> onTags = this.GetTagsOfState( 1 );
			ISet<string> offTags = this.GetTagsOfState( -1 );
			bool isFiltered = onTags.Count > 0 || offTags.Count > 0;

			if( isFiltered ) {
				foreach( string modName in modNames ) {
					if( modTags.ContainsKey( modName ) ) {
						ISet<string> modHasTags = modTags[modName];

						if( modHasTags.Overlaps( offTags ) ) { continue; }
						if( onTags.Count > 0 && !modHasTags.IsSupersetOf( onTags ) ) { continue; }
					} else if( onTags.Count > 0 ) {
						continue;
					}

					filteredModNameList.Add( modName );
				}
			}

			if( ModHelpersMod.Instance.Config.DebugModeHelpersInfo ) {
				LogHelpers.Log( "Filtered to " + filteredModNameList.Count + " mods."
					+ ( onTags.Count > 0 ? "\nWith tags: " + string.Join( ", ", onTags ) : "" )
					+ ( offTags.Count > 0 ? "\nWithout tags: " + string.Join( ", ", offTags ) : "" )
				);
			}

			callback( isFiltered, filteredModNameList, onTags.Count, offTags.Count );
		}
	}
}
