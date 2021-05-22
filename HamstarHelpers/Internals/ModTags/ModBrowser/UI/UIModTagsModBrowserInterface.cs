using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.DotNET.Extensions;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Internals.ModTags.ModBrowser.Manager;
using System;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.UI {
	partial class UIModTagsModBrowserInterface : UIModTagsInterface {
		public UIModTagsModBrowserInterface( UITheme theme, ModTagsModBrowserManager manager )
				: base( theme, manager ) {
			foreach( (string tag, UITagMenuButton tagButton) in this.TagButtons ) {
				tagButton.Enable();
			}

			this.ResetButton.Enable();
		}
	}
}
