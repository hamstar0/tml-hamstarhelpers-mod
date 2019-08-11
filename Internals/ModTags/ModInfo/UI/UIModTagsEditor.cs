using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditor : UIModTagsPanel {
		public UIModTagsEditor( UITheme theme,
				ModTagsEditorManager manager,
				UIState uiContext )
				: base( theme, manager, uiContext, false ) {
		}
	}
}
