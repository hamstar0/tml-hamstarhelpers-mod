using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.ModBrowser.UI;
using System;


namespace HamstarHelpers.Internals.ModTags.ModBrowser {
	partial class ModTagsModBrowserManager : ModTagsManager {
		public UIModTagsBrowser MyTagsUI {
			get {
				return (UIModTagsBrowser)Convert.ChangeType( this.TagsUI, typeof(UIModTagsBrowser) );
			}
		}



		////////////////

		public ModTagsModBrowserManager() : base( true ) {
			var tagsUi = new UIModTagsBrowser( UITheme.Vanilla, this );
			this.TagsUI = (UIModTags<ModTagsManager>)Convert.ChangeType( tagsUi, typeof(UIModTags<ModTagsManager>) );
		}
	}
}
