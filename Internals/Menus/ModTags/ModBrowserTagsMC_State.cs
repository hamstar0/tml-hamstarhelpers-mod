using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Timers;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public override void Show( UIState ui ) {
			base.Show( ui );
			
			this.BeginModBrowserPopulateCheck( ui );
			this.RecalculateMenuObjects();
			this.Panel.EnableTagButtons();

			this.InfoDisplay.SetDefaultText( "Click tags to filter the list. Right-click tags to filter without them." );

			UIElement elem;
			if( ReflectionHelpers.Get(ui, "_rootElement", out elem) ) {
				elem.Left.Pixels += UITagButton.ButtonWidth;
				elem.Recalculate();
			}

			//this.ApplyDefaultFiltersAsync( ui );
		}

		public override void Hide( UIState ui ) {
			base.Hide( ui );

			this.InfoDisplay?.SetDefaultText( "" );

			this.ResetMenuObjects();

			UIElement elem;
			if( ReflectionHelpers.Get( ui, "_rootElement", out elem ) && elem != null ) {
				elem.Left.Pixels -= UITagButton.ButtonWidth;
				elem.Recalculate();
			}
		}


		////////////////

		private void ApplyDefaultFiltersAsync( UIState ui ) {
			CustomLoadHooks.AddHook( GetModTags.TagsReceivedHookValidator, ( args ) => {
				Timers.SetTimer( "ModBrowserDefaultTagStates", 15, () => {
					if( this.MyUI != ui ) {
						return false;
					}

					bool isLoading;
					if( !ReflectionHelpers.Get( this.MyUI, "loading", out isLoading ) ) {
						LogHelpers.Warn( "ModBrowserTagsMenuContext - No 'loading'." );
						return false;
					}

					if( isLoading ) {
						return true;
					} else {
						//UITagButton button = this.TagButtons["Misleading Info"];
						//button.SetTagState( -1 );
						return false;
					}
				} );
				return true;
			} );
		}


		////////////////

		private void BeginModBrowserPopulateCheck( UIState modBrowserUi ) {
			UIList uiModList;

			if( !ReflectionHelpers.Get( modBrowserUi, "ModList", out uiModList ) ) {
				throw new ModHelpersException( "Invalid ModList" );
			}
			
			if( this.IsModBrowserListPopulated( uiModList ) ) {
				this.ApplyModBrowserModInfoBindings( uiModList );
				return;
			}

			if( Timers.GetTimerTickDuration( "ModHelpersModBrowserCheckLoop" ) <= 0 ) {
				Timers.SetTimer( "", 5, () => {
					if( !this.IsModBrowserListPopulated( uiModList ) ) {
						return true;
					}

					this.ApplyModBrowserModInfoBindings( uiModList );
					return false;
				} );
			}
		}


		private bool IsModBrowserListPopulated( UIList uiModList ) {
			int count;

			if( !ReflectionHelpers.Get( uiModList, "Count", out count ) ) {
				throw new ModHelpersException( "Invalid modList.Count" );
			}

			return count > 0;
		}


		private void ApplyModBrowserModInfoBindings( UIList uiModList ) {
			object modList;

			if( !ReflectionHelpers.Get(uiModList, "_items", out modList) || modList == null ) {
				throw new ModHelpersException( "Invalid modList._items" );
			}

			var itemsArr = (Array)modList.GetType()
				.GetMethod( "ToArray" )
				.Invoke( modList, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				var item = (UIElement)itemsArr.GetValue( i );
				if( item == null ) {
					LogHelpers.Warn( "Invalid modList._item[" + i + "] (out of "+itemsArr.Length+")" );
					continue;
				}

				//string modName;
				//if( !ReflectionHelpers.GetField( item, "mod", out modName ) || modName == null ) {
				//	throw new Exception( "Invalid modList._item["+i+"].mod" );
				//}
				
				UIPanel modInfoButton;
				if( !ReflectionHelpers.Get(item, "_moreInfoButton", out modInfoButton) || modInfoButton == null ) {
					LogHelpers.Alert( "Invalid modList._item[" + i + "]._moreInfoButton" );
					continue;
				}

				modInfoButton.OnClick += (evt, elem) => {
					if( this.MyUI == null ) { return; }
					if( !ReflectionHelpers.Set( this.MyUI, "selectedItem", item ) ) {
						LogHelpers.Alert( "Could not set selected item from the mod browser" );
					}
				};
			}
		}
	}
}
