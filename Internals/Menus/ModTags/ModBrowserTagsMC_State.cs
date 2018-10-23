using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.Timers;
using System;
using System.Reflection;
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
		}

		public override void Hide( UIState ui ) {
			base.Hide( ui );

			this.InfoDisplay.SetDefaultText( "" );

			this.ResetMenuObjects();
		}

		////////////////

		private void ShowGeneral( UIState ui ) {
			this.RecalculateMenuObjects();
			this.EnableTagButtons();

			this.BeginModBrowserPopulateCheck( ui );
		}


		////////////////

		private void BeginModBrowserPopulateCheck( UIState mod_browser_ui ) {
			UIList ui_mod_list;

			if( !ReflectionHelpers.GetField( mod_browser_ui, "modList", out ui_mod_list ) ) {
				throw new Exception( "Invalid modList" );
			}
			
			if( this.IsModBrowserListPopulated( ui_mod_list ) ) {
				this.ApplyModBrowserModInfoBindings( ui_mod_list );
				return;
			}

			if( Timers.GetTimerTickDuration( "ModHelpersModBrowserCheckLoop" ) <= 0 ) {
				Timers.SetTimer( "", 5, () => {
					if( !this.IsModBrowserListPopulated( ui_mod_list ) ) {
						return true;
					}

					this.ApplyModBrowserModInfoBindings( ui_mod_list );
					return false;
				} );
			}
		}


		private bool IsModBrowserListPopulated( UIList ui_mod_list ) {
			int count;

			if( !ReflectionHelpers.GetProperty( ui_mod_list, "Count", out count ) ) {
				throw new Exception( "Invalid modList.Count" );
			}

			return count > 0;
		}


		private void ApplyModBrowserModInfoBindings( UIList ui_mod_list ) {
			object mod_list;

			if( !ReflectionHelpers.GetField( ui_mod_list, "_items", out mod_list ) || mod_list == null ) {
				throw new Exception( "Invalid modList._items" );
			}

			var items_arr = (Array)mod_list.GetType()
				.GetMethod( "ToArray" )
				.Invoke( mod_list, new object[] { } );

			for( int i = 0; i < items_arr.Length; i++ ) {
				var item = (UIElement)items_arr.GetValue( i );
				if( item == null ) {
					LogHelpers.Log( "Invalid modList._item[" + i + "]" );
					continue;
				}

				//string mod_name;
				//if( !ReflectionHelpers.GetField( item, "mod", out mod_name ) || mod_name == null ) {
				//	throw new Exception( "Invalid modList._item["+i+"].mod" );
				//}

				UIPanel mod_info_button;
				if( !ReflectionHelpers.GetField( item, "moreInfoButton", BindingFlags.Instance | BindingFlags.NonPublic, out mod_info_button )
						|| mod_info_button == null ) {
					LogHelpers.Log( "Invalid modList._item[" + i + "].moreInfoButton" );
					continue;
				}

				mod_info_button.OnClick += ( evt, elem ) => {
					if( this.MyUI == null ) { return; }
					ReflectionHelpers.SetField( this.MyUI, "selectedItem", item );
				};
			}
		}
	}
}
