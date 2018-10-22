using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Timers;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	class ModBrowserTagsMenuLoader : MenuLoader {
		public override void Show( UIState ui ) {
			this.RecalculateMenuObjects();
			this.EnableTagButtons();

			this.BeginModBrowserPopulateCheck( ui );

			SessionMenuContext.InfoDisplay.SetDefaultText( "Click tags to filter the list. Right-click tags to filter without them." );
		}

		public override void Hide( UIState ui ) {
			SessionMenuContext.InfoDisplay.SetDefaultText( "" );

			this.ResetMenuObjects();
		}
	}




	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		protected void InitializeContext() {
			Action<UIState> ui_load = ui => {
				this.RecalculateMenuObjects();
				this.EnableTagButtons();

				this.BeginModBrowserPopulateCheck( ui );
			};
			Action<UIState> ui_unload = ui => {
			};

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: "+this.SubContextName+" Load", ui_load, ui_unload );
		}


		protected void InitializeControls() {
			this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -298f, 172f, 0.36f, true );
			this.ResetButton = new UITagResetButton( this );

			this.BlankButton.Disable();

			MenuContextService.AddMenuLoader( this.UIName, this.SubContextName + " Tag Blank Button", this.BlankButton, false );
			MenuContextService.AddMenuLoader( this.UIName, this.SubContextName + " Tag Reset Button", this.ResetButton, false );
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
