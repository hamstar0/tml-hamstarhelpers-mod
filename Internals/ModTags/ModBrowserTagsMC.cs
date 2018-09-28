using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ModTags.UI;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public static void Initialize() {
			new ModBrowserTagsMenuContext();
		}



		////////////////

		protected override string UIName => "UIModBrowser";
		protected override string ContextName => "Mod Browser";



		////////////////

		private ModBrowserTagsMenuContext() {
			this.InitializeBase();
			this.InitializeTagButtons( true );
			this.InitializeContext();
			this.InitializeHoverText();
		}

		////////////////

		protected override void InitializeContext() {
			Action<UIState> ui_load = ui => {
				this.RecalculateMenuObjects();
				this.EnableTagButtons();
			};
			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: "+this.ContextName+" Load", ui_load, ui_unload );
		}


		////////////////

		public override void OnTagStateChange( UITagButton tag_button ) {
			this.FilterMods();
		}
	}
}
