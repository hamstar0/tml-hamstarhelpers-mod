using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal static ISet<string> RecentTaggedMods = new HashSet<string>();
		

		////////////////

		public static void Initialize() {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			var ctx = new ModInfoTagsMenuContext();
			MenuContextService.AddMenuContext( ctx.UIName, "ModHelpers: " + ctx.SubContextName + " Tag Finish Button", ctx );
		}



		////////////////

		internal UITagFinishButton FinishButton;
		internal UITagResetButton ResetButton;

		public string CurrentModName = "";

		internal IDictionary<string, ISet<string>> AllModTagsSnapshot = null;

		////////////////

		public override string UIName => "UIModInfo";
		public override string SubContextName => "Mod Info";



		////////////////

		private ModInfoTagsMenuContext() : base( false ) {
			this.FinishButton = new UITagFinishButton( this );
			this.ResetButton = new UITagResetButton( this );

			var finish_button_widget_ctx = new WidgetMenuContext( this.FinishButton, false );
			var reset_button_widget_ctx = new WidgetMenuContext( this.ResetButton, false );

			MenuContextService.AddMenuContext( this.UIName, "ModHelpers: " + this.SubContextName + " Tag Finish Button", finish_button_widget_ctx );
			MenuContextService.AddMenuContext( this.UIName, "ModHelpers: " + this.SubContextName + " Tag Reset Button", reset_button_widget_ctx );
		}


		////////////////

		public override void OnTagStateChange( UITagButton tag_button ) {
			this.FinishButton.UpdateEnableState();
			this.ResetButton.UpdateEnableState();
		}
	}
}
