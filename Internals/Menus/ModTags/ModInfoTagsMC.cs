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
			MenuContextService.AddMenuContext( "UIModInfo", "ModHelpers: Mod Info", ctx );
		}



		////////////////

		internal UITagFinishButton FinishButton;
		internal UITagResetButton ResetButton;

		public string CurrentModName = "";

		internal IDictionary<string, ISet<string>> AllModTagsSnapshot = null;



		////////////////

		private ModInfoTagsMenuContext() : base( false ) {
			this.FinishButton = new UITagFinishButton( this );
			this.ResetButton = new UITagResetButton( this );
		}
		
		public override void OnContexualize( string ui_class_name, string context_name ) {
			var finish_button_widget_ctx = new WidgetMenuContext( this.FinishButton, false );
			var reset_button_widget_ctx = new WidgetMenuContext( this.ResetButton, false );

			MenuContextService.AddMenuContext( ui_class_name, context_name + " Tag Finish Button", finish_button_widget_ctx );
			MenuContextService.AddMenuContext( ui_class_name, context_name + " Tag Reset Button", reset_button_widget_ctx );
		}


		////////////////

		public override void OnTagStateChange( UITagButton tag_button ) {
			this.FinishButton.UpdateEnableState();
			this.ResetButton.UpdateEnableState();
		}
	}
}
