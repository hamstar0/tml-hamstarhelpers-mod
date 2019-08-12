using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.UI {
	partial class UIModTagsBrowser : UIModTags<ModTagsModBrowserManager> {
		public UIModTagsBrowser( UITheme theme, ModTagsModBrowserManager manager )
				: base( theme, manager, true ) {
		}
	}
}
