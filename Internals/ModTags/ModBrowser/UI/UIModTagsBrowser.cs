using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Internals.ModTags.Base;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.UI {
	partial class UIModTagsBrowser : UIModTagsPanel {
		public UIModTagsBrowser( UITheme theme,
				ModTagsModBrowserManager manager,
				UIState uiContext,
				bool canExcludeTags )
				: base( theme, manager, uiContext, canExcludeTags ) {
		}
	}
}
