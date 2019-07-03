using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public static void Initialize() {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }
			
			var ctx = new ModBrowserTagsMenuContext();
			MenuContextService.AddMenuContext( "UIModBrowser", "ModHelpers: Mod Browser", ctx );
		}



		////////////////

		//internal UITextPanelButton BlankButton;
		internal UITagResetButton ResetButton;



		////////////////

		private ModBrowserTagsMenuContext() : base( true ) {
			//this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -196f, 172f, 0.36f, true );
			this.ResetButton = new UITagResetButton( this );

			//this.BlankButton.Disable();
		}

		public override void OnContexualize( string uiClassName, string contextName ) {
			base.OnContexualize( uiClassName, contextName );

			var blankRecomPanel = new UIMenuPanel( UITheme.Vanilla, 198f, 132f, 202f, 40f );
			var blankDlButtonPanel = new UIMenuPanel( UITheme.Vanilla, 198f, 26f, 202f, 172f );

			//var blankButtonWidgetCtx = new WidgetMenuContext( this.BlankButton, false );
			var resetButtonWidgetCtx = new WidgetMenuContext( this.ResetButton, false );
			var blankRecomWidgetCtx = new WidgetMenuContext( blankRecomPanel, false );
			var blankDlWidgetCtx = new WidgetMenuContext( blankDlButtonPanel, false );

			//MenuContextService.AddMenuContext( uiClassName, contextName + " Tag Blank Button", blankButtonWidgetCtx );
			MenuContextService.AddMenuContext( uiClassName, contextName + " Tag Reset Button", resetButtonWidgetCtx );
			MenuContextService.AddMenuContext( uiClassName, contextName + " Blank Recommendations List", blankRecomWidgetCtx );
			MenuContextService.AddMenuContext( uiClassName, contextName + " Blank Download Button", blankDlWidgetCtx );
		}


		////////////////

		public override void OnTagStateChange( UITagButton tagButton ) {
			this.ApplyModsFilter();

			this.ResetButton.UpdateEnableState();
		}
	}
}
