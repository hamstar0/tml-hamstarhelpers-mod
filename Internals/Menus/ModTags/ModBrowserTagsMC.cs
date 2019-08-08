using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.UI;
using HamstarHelpers.Services.UI.Menus;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public static void Initialize( bool onModLoad ) {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			if( !onModLoad ) {
				var ctx = new ModBrowserTagsMenuContext();
				MenuContextService.AddMenuContext( MenuUIDefinition.UIModBrowser, "ModHelpers: Mod Browser", ctx );
			}
		}



		////////////////

		//internal UITextPanelButton BlankButton;
		internal UITagResetButton ResetButton;



		////////////////

		private ModBrowserTagsMenuContext() : base( true ) {
			//this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -196f, 172f, 0.36f, true );
			this.ResetButton = new UITagResetButton( UITheme.Vanilla, this.Manager );

			//this.BlankButton.Disable();
		}

		public override void OnContexualize( MenuUIDefinition menuDef, string contextName ) {
			base.OnContexualize( menuDef, contextName );

			var blankRecomPanel = new UIMenuPanel( UITheme.Vanilla, 198f, 132f, 202f, 40f );
			var blankDlButtonPanel = new UIMenuPanel( UITheme.Vanilla, 198f, 26f, 202f, 172f );

			//var blankButtonWidgetCtx = new WidgetMenuContext( this.BlankButton, false );
			var resetButtonWidgetCtx = new WidgetMenuContext( this.ResetButton, false );
			var blankRecomWidgetCtx = new WidgetMenuContext( blankRecomPanel, false );
			var blankDlWidgetCtx = new WidgetMenuContext( blankDlButtonPanel, false );

			//MenuContextService.AddMenuContext( menuDef, contextName + " Tag Blank Button", blankButtonWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Tag Reset Button", resetButtonWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Blank Recommendations List", blankRecomWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Blank Download Button", blankDlWidgetCtx );
		}


		////////////////

		public override void OnTagStateChange( UITagButton tagButton ) {
			this.ApplyModsFilter();

			this.ResetButton.UpdateEnableState();
		}
	}
}
