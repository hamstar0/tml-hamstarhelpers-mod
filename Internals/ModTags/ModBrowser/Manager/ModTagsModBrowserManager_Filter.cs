using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.Manager {
	/// @private
	partial class ModTagsModBrowserManager : ModTagsManager {
		/// @private
		public delegate void FilteredModsHandler(
			UIState _menuUi,
			bool isFiltered,
			IList<string> filteredModNameList,
			int onTagCount,
			int offTagCount
		);



		////////////////

		private void ApplyModsFilter() {
			UIState menuUi = MainMenuHelpers.GetMenuUI( this.MenuDefinition );

			IList<string> modNames = new List<string>();

			object items;
			if( !ReflectionHelpers.Get( menuUi, "_items", out items ) ) {
				LogHelpers.Warn( "No 'items' field in ui " + menuUi );
				return;
			}

			var itemsArr = (Array)items.GetType()
				.GetMethod( "ToArray" )
				.Invoke( items, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				object item = itemsArr.GetValue( i );
				string modName;

				if( ReflectionHelpers.Get(item, "ModName", out modName) ) {
					modNames.Add( modName );
				} else {
					LogHelpers.Warn( "No 'ModName' field or property in mod browser list ('_items')." );
					return;
				}
			}

			this.FilterModsAsync( menuUi, modNames, this.HandleFilteredMods );
		}


		private void HandleFilteredMods( UIState _menuUi,
				bool isFiltered,
				IList<string> filteredModNameList,
				int onTagCount,
				int offTagCount ) {
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

			if( ReflectionHelpers.Set( _menuUi, "UpdateFilterMode", (UpdateFilter)0 ) ) {
				ModMenuHelpers.ApplyModBrowserFilter( filterName, isFiltered, (List<string>)filteredModNameList );
			} else {
				LogHelpers.Alert( "Could not set UpdateFilterMode for the mod browser" );
			}
		}


		////////////////

		private void FilterModsAsync( UIState _menuUi, IList<string> modNames, FilteredModsHandler callback ) {
			CustomLoadHooks.AddHook( GetModTags.TagsReceivedHookValidator, ( args ) => {
				if( !args.Found ) {
					this.SetInfoText( "Could not acquire mod data." );
					callback( _menuUi, false, new List<string>(), 0, 0 );
					return false;
				}

				this.FilterMods( _menuUi, modNames, args.ModTags, callback );
				return false;
			} );
		}


		private void FilterMods( UIState _menuUi,
				IList<string> modNames,
				IDictionary<string, ISet<string>> modTagsOfModNames,
				FilteredModsHandler callback ) {
			IList<string> filteredModNameList = new List<string>();
			ISet<string> onTags = this.GetTagsWithGivenState( 1 );
			ISet<string> offTags = this.GetTagsWithGivenState( -1 );
			bool isFiltered = onTags.Count > 0 || offTags.Count > 0;

			if( isFiltered ) {
				foreach( string modName in modNames ) {
					if( modTagsOfModNames.ContainsKey( modName ) ) {
						ISet<string> myModsTags = modTagsOfModNames[modName];

						if( myModsTags.Overlaps( offTags ) ) {
							continue;
						}
						if( onTags.Count > 0 && !myModsTags.IsSupersetOf( onTags ) ) {
							continue;
						}
					} else if( onTags.Count > 0 ) {
						continue;
					}

					filteredModNameList.Add( modName );
				}
			}

			if( ModHelpersConfig.Instance.DebugModeHelpersInfo ) {
				LogHelpers.Log( "Filtered to " + filteredModNameList.Count + " mods."
					+ ( onTags.Count > 0 ? "\nWith tags: " + string.Join( ", ", onTags ) : "" )
					+ ( offTags.Count > 0 ? "\nWithout tags: " + string.Join( ", ", offTags ) : "" )
				);
			}

			callback( _menuUi, isFiltered, filteredModNameList, onTags.Count, offTags.Count );
		}
	}
}
