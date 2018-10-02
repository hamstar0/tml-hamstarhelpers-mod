using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ModTags.UI;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		protected void InitializeContext() {
			Action<UIState> ui_load = ui => {
				this.RecalculateMenuObjects();
				this.EnableTagButtons();
			};
			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: "+this.ContextName+" Load", ui_load, ui_unload );
		}


		protected void InitializeControls() {
			this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 72f, 40f, -286f, 172f, 0.55f, true );
			this.ResetButton = new UITagResetButton( this );

			this.BlankButton.Disable();

			MenuUI.AddMenuLoader( this.UIName, this.ContextName + " Tag Blank Button", this.BlankButton, false );
			MenuUI.AddMenuLoader( this.UIName, this.ContextName + " Tag Reset Button", this.ResetButton, false );
		}
	}
}
