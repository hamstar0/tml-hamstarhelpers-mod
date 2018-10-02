using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ModTags.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public static void Initialize() {
			new ModBrowserTagsMenuContext();
		}



		////////////////

		internal UITextPanelButton BlankButton;
		internal UITagResetButton ResetButton;

		////////////////

		public override string UIName => "UIModBrowser";
		public override string ContextName => "Mod Browser";



		////////////////

		private ModBrowserTagsMenuContext() : base( true ) {
			this.InitializeContext();
			this.InitializeControls();
		}


		////////////////

		public override void OnTagStateChange( UITagButton tag_button ) {
			this.FilterMods();

			this.ResetButton.UpdateEnableState();
		}
	}
}
