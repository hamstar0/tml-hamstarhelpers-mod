using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal static ISet<string> RecentTaggedMods = new HashSet<string>();
		

		////////////////

		public static void Initialize() {
			new ModInfoTagsMenuContext();
		}



		////////////////

		internal UITagFinishButton FinishButton;
		internal UITagResetButton ResetButton;

		public string CurrentModName = "";

		////////////////

		public override string UIName => "UIModInfo";
		public override string ContextName => "Mod Info";



		////////////////

		private ModInfoTagsMenuContext() : base( false ) {
			this.InitializeContext();
			this.InitializeControls();
		}


		////////////////

		public override void OnTagStateChange( UITagButton tag_button ) {
			this.FinishButton.UpdateEnableState();
			this.ResetButton.UpdateEnableState();
		}
	}
}
