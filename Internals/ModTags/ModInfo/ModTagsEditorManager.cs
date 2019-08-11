using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base;
using HamstarHelpers.Internals.ModTags.ModInfo.UI;
using System;


namespace HamstarHelpers.Internals.ModTags.ModInfo {
	partial class ModTagsEditorManager : ModTagsManager {
		public ModTagsEditorManager() : base( false ) {
			this.TagsUI = new UIModTagsEditor( UITheme.Vanilla, this, uiContext, this.CanExcludeTags );
		}
	}
}
