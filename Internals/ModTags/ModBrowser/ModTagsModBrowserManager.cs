using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.ModBrowser.UI;
using System;


namespace HamstarHelpers.Internals.ModTags.ModBrowser {
	partial class ModTagsModBrowserManager : ModTagsManager {
		public UIModTagsModBrowser MyTagsUI {
			get {
				return (UIModTagsModBrowser)Convert.ChangeType( this.TagsUI, typeof(UIModTagsModBrowser) );
			}
		}



		////////////////

		public ModTagsModBrowserManager( UIInfoDisplay infoDisplay ) : base( infoDisplay, true ) {
			var tagsUi = new UIModTagsModBrowser( UITheme.Vanilla, this );
			this.TagsUI = (UIModTags<ModTagsManager>)Convert.ChangeType( tagsUi, typeof(UIModTags<ModTagsManager>) );
		}
	}
}
