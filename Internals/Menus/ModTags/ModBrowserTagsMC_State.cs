using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Timers;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public override void Show( UIState ui ) {
			base.Show( ui );
			this.ShowGeneral( ui );

			this.RecalculateMenuObjects();
			this.EnableTagButtons();

			this.BeginModBrowserPopulateCheck( ui );

			this.InfoDisplay.SetDefaultText( "Click tags to filter the list. Right-click tags to filter without them." );

			//this.ShowGeneral( ui );	TODO Verify!
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

		private bool ModBrowserPopulateCheck( UIState mod_browser_ui ) {
			object list;

			if( !ReflectionHelpers.GetField( mod_browser_ui, "modList", out list ) ) {
				return false;
			}
			
			f

			return true;
		}

		private void BeginModBrowserPopulateCheck( UIState mod_browser_ui ) {
			if( this.ModBrowserPopulateCheck(mod_browser_ui) ) {
				return;
			}

			if( Timers.GetTimerTickDuration( "ModHelpersModBrowserCheckLoop" ) <= 0 ) {
				Timers.SetTimer( "", 2, () => {
					return this.ModBrowserPopulateCheck( mod_browser_ui );
				} );
			}
		}
	}
}
