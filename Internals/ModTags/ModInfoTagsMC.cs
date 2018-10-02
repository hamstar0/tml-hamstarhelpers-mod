using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ModTags.UI;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags {
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

		private ModInfoTagsMenuContext() : base(false) { }


		////////////////

		public override void OnTagStateChange( UITagButton tag_button ) {
			this.FinishButton.UpdateEnableState();
			this.ResetButton.UpdateEnableState();
		}
	}
}
