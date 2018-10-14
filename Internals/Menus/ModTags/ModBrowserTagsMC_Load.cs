using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;
using Microsoft.Xna.Framework;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		protected void InitializeContext() {
			Action<UIState> ui_load = ui => {
				this.RecalculateMenuObjects();
				this.EnableTagButtons();

				MenuContextBase.InfoDisplay.SetText( "Click tags to filter the list. Right-click tags to filter without them.", Color.Gray );
			};
			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: "+this.ContextName+" Load", ui_load, ui_unload );
		}


		protected void InitializeControls() {
			this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -298f, 172f, 0.36f, true );
			this.ResetButton = new UITagResetButton( this );

			this.BlankButton.Disable();

			MenuContextService.AddMenuLoader( this.UIName, this.ContextName + " Tag Blank Button", this.BlankButton, false );
			MenuContextService.AddMenuLoader( this.UIName, this.ContextName + " Tag Reset Button", this.ResetButton, false );
		}
	}
}
