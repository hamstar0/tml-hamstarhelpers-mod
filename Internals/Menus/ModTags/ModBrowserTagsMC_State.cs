using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public override void Show( UIState ui ) {
			base.Show( ui );
			
			this.BeginModBrowserPopulateCheck( ui );
			this.RecalculateMenuObjects();
			this.EnableTagButtons();

			this.InfoDisplay.SetDefaultText( "Click tags to filter the list. Right-click tags to filter without them." );

			UIElement elem;
			if( ReflectionHelpers.Get<UIElement>( ui, "uIElement", out elem ) ) {
				elem.Left.Pixels += UITagButton.ColumnWidth;
				elem.Recalculate();
			}

			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				Timers.SetTimer( "ModBrowserDefaultTagStates", 60*3, () => {
					if( this.MyUI == ui ) {
						UITagButton button = this.TagButtons["Misleading Info"];
						button.SetTagState( -1 );
					}
					return false;
				} );
				return true;
			} );
		}

		public override void Hide( UIState ui ) {
			base.Hide( ui );

			this.InfoDisplay?.SetDefaultText( "" );

			this.ResetMenuObjects();

			UIElement elem;
			if( ReflectionHelpers.Get( ui, "uIElement", out elem ) && elem != null ) {
				elem.Left.Pixels -= UITagButton.ColumnWidth;
				elem.Recalculate();
			}
		}


		////////////////

		private void BeginModBrowserPopulateCheck( UIState modBrowserUi ) {
			UIList uiModList;

			if( !ReflectionHelpers.Get( modBrowserUi, "modList", out uiModList ) ) {
				throw new HamstarException( "Invalid modList" );
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
				throw new HamstarException( "Invalid modList.Count" );
			}

			return count > 0;
		}


		private void ApplyModBrowserModInfoBindings( UIList uiModList ) {
			object modList;

			if( !ReflectionHelpers.Get( uiModList, "_items", out modList ) || modList == null ) {
				throw new HamstarException( "Invalid modList._items" );
			}

			var itemsArr = (Array)modList.GetType()
				.GetMethod( "ToArray" )
				.Invoke( modList, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				var item = (UIElement)itemsArr.GetValue( i );
				if( item == null ) {
					LogHelpers.Warn( "Invalid modList._item[" + i + "]" );
					continue;
				}

				//string modName;
				//if( !ReflectionHelpers.GetField( item, "mod", out modName ) || modName == null ) {
				//	throw new Exception( "Invalid modList._item["+i+"].mod" );
				//}
				
				UIPanel modInfoButton;
				if( !ReflectionHelpers.Get( item, "moreInfoButton", out modInfoButton ) || modInfoButton == null ) {
					LogHelpers.Alert( "Invalid modList._item[" + i + "].moreInfoButton" );
					continue;
				}

				modInfoButton.OnClick += ( evt, elem ) => {
					if( this.MyUI == null ) { return; }
					ReflectionHelpers.Set( this.MyUI, "selectedItem", item );
				};
			}
		}
	}
}
