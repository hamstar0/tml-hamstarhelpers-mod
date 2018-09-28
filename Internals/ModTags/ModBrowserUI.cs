using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModBrowserUI : ModTagsUI {
		public static void Initialize() {
			new ModBrowserUI();
		}



		////////////////

		protected override string UIName => "UIModBrowser";
		protected override string ContextName => "Mod Browser";



		////////////////

		private ModBrowserUI() {
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

		public override void OnTagStateChange( UIModTagButton tag_button ) {
			this.FilterMods();
		}
	}
}
