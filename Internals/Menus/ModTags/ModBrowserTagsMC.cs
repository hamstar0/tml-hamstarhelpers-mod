using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public static void Initialize() {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }
			
			var ctx = new ModBrowserTagsMenuContext();
			MenuContextService.AddMenuContext( "UIModBrowser", "ModHelpers: Mod Browser", ctx );
		}



		////////////////

		internal UITextPanelButton BlankButton;
		internal UITagResetButton ResetButton;



		////////////////

		private ModBrowserTagsMenuContext() : base( true ) {
			this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -298f, 172f, 0.36f, true );
			this.ResetButton = new UITagResetButton( this );

			this.BlankButton.Disable();
		}

		public override void OnContexualize( string ui_class_name, string context_name ) {
			var blank_button_widget_ctx = new WidgetMenuContext( this.BlankButton, false );
			var reset_button_widget_ctx = new WidgetMenuContext( this.ResetButton, false );

			MenuContextService.AddMenuContext( ui_class_name, context_name + " Tag Blank Button", blank_button_widget_ctx );
			MenuContextService.AddMenuContext( ui_class_name, context_name + " Tag Reset Button", reset_button_widget_ctx );
		}


		////////////////

		public override void OnTagStateChange( UITagButton tag_button ) {
			this.FilterMods();

			this.ResetButton.UpdateEnableState();
		}
	}
}
