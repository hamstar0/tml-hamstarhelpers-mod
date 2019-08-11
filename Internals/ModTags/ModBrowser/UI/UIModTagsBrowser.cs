using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Internals.ModTags.UI;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.UI {
	partial class UIModTagsBrowser : UIModTagsPanel {
		public UIModTagsBrowser( UITheme theme,
				ModTagsManager manager,
				UIState uiContext,
				TagDefinition[] tags,
				bool canExcludeTags )
				: base( theme, manager, uiContext, tags, canExcludeTags ) {
		}
	}
}
