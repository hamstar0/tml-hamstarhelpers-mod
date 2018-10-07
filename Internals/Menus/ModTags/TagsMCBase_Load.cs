using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : MenuContextBase {
		private void InitializeLinks() {
			UITheme support_url_theme = UITheme.Vanilla.Clone();
			support_url_theme.UrlColor = Color.Lerp( support_url_theme.UrlColor, Color.MediumSpringGreen, 0.5f );
			support_url_theme.UrlLitColor = Color.Lerp( support_url_theme.UrlLitColor, Color.MediumSpringGreen, 0.5f );

			var links_head = new UIText( "Tags via. ", 0.8f );
			links_head.Left.Set( 242f, 0.5f );
			links_head.Top.Set( 2f, 0f );
			var links_head_url = new UIWebUrl( UITheme.Vanilla, "Mod Helpers", "https://forums.terraria.org/index.php?threads/.63670/", true, 0.8f, false );
			links_head_url.Left.Set( 302f, 0.5f );
			links_head_url.Top.Set( 2f, 0f );
			var links_support_1 = new UIText( "Please", 0.8f );
			links_support_1.Left.Set( 242f, 0.5f );
			links_support_1.Top.Set( 22f, 0f );
			links_support_1.TextColor = Color.MediumSpringGreen;
			var links_support_url = new UIWebUrl( support_url_theme, "support", "https://www.patreon.com/hamstar0", true, 0.8f, false );
			links_support_url.Left.Set( 286f, 0.5f );
			links_support_url.Top.Set( 22f, 0f );
			var links_support_2 = new UIText( "my mods!", 0.8f );
			links_support_2.Left.Set( 338f, 0.5f );
			links_support_2.Top.Set( 22f, 0f );
			links_support_2.TextColor = Color.MediumSpringGreen;

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Links Head", links_head, false );
			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Links Head URL", links_head_url, false );
			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Links Support 1", links_support_1, false );
			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Links Support URL", links_support_url, false );
			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Links Support 2", links_support_2, false );
		}


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
