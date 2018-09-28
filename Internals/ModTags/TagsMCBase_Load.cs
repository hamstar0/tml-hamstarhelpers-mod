using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ModTags {
	abstract partial class TagsMenuContextBase {
		protected void InitializeBase() {
			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Set UI",
				ui => { this.MyUI = ui; },
				ui => { this.MyUI = null; }
			);
		}


		protected abstract void InitializeContext();


		protected void InitializeHoverText() {
			this.HoverElement = new UIText( "" );
			this.HoverElement.Width.Set( 0, 0 );
			this.HoverElement.Height.Set( 0, 0 );
			this.HoverElement.TextColor = Color.Aquamarine;

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Tag Hover", this.HoverElement, false );
		}


		protected void InitializeTagButtons( bool can_disable_tags ) {
			int i = 0;

			foreach( var kv in TagsMenuContextBase.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;

				var button = new UIModTagButton( this, i, tag_text, tag_desc, can_disable_tags );

				MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Tag " + i, button, false );
				this.TagButtons[tag_text] = button;

				i++;
			}
		}
	}
}
