using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : MenuContextBase {
		private void InitializeTagButtons() {
			int i = 0;

			foreach( var kv in TagsMenuContextBase.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;

				var button = new UITagButton( this, i, tag_text, tag_desc, this.CanDisableTags );

				MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Tag " + i, button, false );
				this.TagButtons[ tag_text ] = button;

				i++;
			}
		}
	}
}
