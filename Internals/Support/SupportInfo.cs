using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.Support {
	/// @private
	internal partial class SupportInfoDisplay {
		public static Color HeaderLabelColor = Color.Lerp( Color.White, Color.Gold, 0.25f );



		////////////////

		private float RowHeight;
		private float Scale;

		private Texture2D PatreonTex;

		private UIText HeadTextLabel;
		private UIWebUrl HeadUrl;
		private UIText ModderTextLabel;
		private UIWebUrl ModderUrl;
		//private UIText RecomInfoTextLabel;
		//private UIWebUrl RecomInfoUrl;
		//private UIText AuthorText1Label;
		//private UIWebUrl AuthorUrl;
		//private UIText AuthorText2Label;
		//private UIText SupportText1Label;
		//private UIWebUrl SupportUrl;
		//private UIText SupportText2Label;
		private UIText PatreonLabel;
		private UIImageUrl PatreonButton;
		private UIText ExtendTextLabel;
		private UIText EnableModTagsLabel = null;

		private IList<UIElement> Elements = new List<UIElement>();

		private bool IsHoveringBox = false;
		private bool IsClicking = false;
		private bool IsExtended = false;

		private float Width;

		private float ModTagsNewOverlayAnim = 0;



		////////////////

		internal SupportInfoDisplay( float width = 248f, float yBeg = 8f, float rowHeight = 30f, float scale = 0.8f ) {
			if( Main.dedServ ) { return; }

			var mymod = ModHelpersMod.Instance;
			float y = yBeg;
			float row = 0;
			this.Width = width;

			this.PatreonTex = ModHelpersMod.Instance.GetTexture( "patreon" );

			this.RowHeight = rowHeight;
			this.Scale = scale;

			////

			this.HeadTextLabel = new UIText( "Powered by:", 1.1f * scale );
			this.HeadTextLabel.Left.Set( -width, 1f );
			this.HeadTextLabel.Top.Set( (4f + y) * scale, 0f );
			this.HeadTextLabel.TextColor = SupportInfoDisplay.HeaderLabelColor;
			this.HeadTextLabel.Recalculate();

			this.HeadUrl = new UIWebUrl( UITheme.Vanilla, "Mod Helpers v" + mymod.Version.ToString(), "https://forums.terraria.org/index.php?threads/.63670/", true, 1.1f * scale );
			this.HeadUrl.Left.Set( -( width - ( 114f * scale ) ), 1f );
			this.HeadUrl.Top.Set( (4f + y) * scale, 0f );
			this.HeadUrl.Recalculate();

			this.ExtendTextLabel = new UIThemedText( UITheme.Vanilla, false, "..." );
			this.ExtendTextLabel.Left.Set( -(width * 0.5f) - 16f, 1f );
			this.ExtendTextLabel.Top.Set( (-14f + y + rowHeight) * scale, 0f );
			this.ExtendTextLabel.Recalculate();

			////

			row += 1;

			if( !mymod.Config.DisableModTags ) {
				if( !mymod.Data.ModTagsOpened ) {
					this.EnableModTagsLabel = new UIThemedText( UITheme.Vanilla, false, "Enable Mod Tags", 1f * ( scale + 0.2f ) );
					this.EnableModTagsLabel.TextColor = Color.Orange;
					this.EnableModTagsLabel.Left.Set( -( width - 48f ), 1f );
					this.EnableModTagsLabel.Top.Set( ( y + ( ( row + 1 ) * rowHeight ) ) * scale, 0f );
				}
			}

			////

			y += 6f * scale;

			this.ModderTextLabel = new UIText( "Do you make mods?", 1f * scale );
			this.ModderTextLabel.Left.Set( -width, 1f );
			this.ModderTextLabel.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );

			this.ModderUrl = new UIWebUrl( UITheme.Vanilla, "Read this.", "https://forums.terraria.org/index.php?threads/.63670/#modders", true, 1f * scale );
			this.ModderUrl.Left.Set( -( width - ( 166f * scale ) ), 1f );
			this.ModderUrl.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );

			////

			/*
			row += 1;

			this.RecomInfoTextUI = new UIText( "Feedback? Confused? Bored?", 1f * scale );
			this.RecomInfoTextUI.Left.Set( -xOff, 1f );
			this.RecomInfoTextUI.Top.Set( (y + (row * rowHeight)) * scale, 0f );

			this.RecomInfoUrlUI = new UIWebUrl( UITheme.Vanilla, "Discord", "https://discord.gg/a2AwYtj", true, 1f * scale );
			this.RecomInfoUrlUI.Left.Set( -( xOff - (242f * scale) ), 1f );
			this.RecomInfoUrlUI.Top.Set( (y + (row * rowHeight) * scale), 0f );*/

			////

			/*
			row += 1;

			this.AuthorText1UI = new UIText( "Looking for more", 1f * scale );
			this.AuthorText1UI.Left.Set( -xOff, 1f );
			this.AuthorText1UI.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );

			this.AuthorUrlUI = new UIWebUrl( UITheme.Vanilla, "specialized mods", "https://forums.terraria.org/index.php?threads/.63713/", true, 1f * scale );
			this.AuthorUrlUI.Left.Set( -( xOff - ( 144f * scale ) ), 1f );
			this.AuthorUrlUI.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );

			this.AuthorText2UI = new UIText( "?", 1f * scale );
			this.AuthorText2UI.Left.Set( -( xOff - ( 280f * scale ) ), 1f );
			this.AuthorText2UI.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );*/

			////

			/*row += 1;

			this.SupportText1Label = new UIText( "Please", 1f * scale );
			this.SupportText1Label.Left.Set( -width, 1f );
			this.SupportText1Label.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );
			//this.SupportText1UI.TextColor = Color.MediumSpringGreen;

			this.SupportUrl = new UIWebUrl( UITheme.Vanilla.Clone(), "support", "https://www.patreon.com/hamstar0", true, 1f * scale );
			this.SupportUrl.Left.Set( -( width - ( 54f * scale ) ), 1f );
			this.SupportUrl.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );

			this.SupportText2Label = new UIText( "upkeep & improvement", 1f * scale );
			this.SupportText2Label.Left.Set( -( width - ( 120f * scale ) ), 1f );
			this.SupportText2Label.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );
			//this.SupportText2UI.TextColor = Color.MediumSpringGreen;*/

			////

			row += 1;

			this.PatreonLabel = new UIText( "Find this useful?", 1f * scale );
			this.PatreonLabel.Left.Set( -width, 1f );
			this.PatreonLabel.Top.Set( ( y + (row * rowHeight)) * scale, 0f );

			this.PatreonButton = new UIImageUrl( UITheme.Vanilla, this.PatreonTex, "https://www.patreon.com/hamstar0" );	//"https://www.patreon.com/bePatron?u=8504613"
			this.PatreonButton.Left.Set( -134, 1f );
			this.PatreonButton.Top.Set( ((y + ( row * rowHeight )) * scale) - 4, 0f );

			////

			this.Elements.Add( this.HeadTextLabel );
			this.Elements.Add( this.HeadUrl );
			this.Elements.Add( this.ExtendTextLabel );
			if( this.EnableModTagsLabel != null ) {
				this.Elements.Add( this.EnableModTagsLabel );
			}

			Main.OnPostDraw += SupportInfoDisplay._Draw;
		}


		private void ExpandUI() {
			this.Elements.Add( this.ModderTextLabel );
			this.Elements.Add( this.ModderUrl );
			//this.Elements.Insert( 0, this.AuthorText1UI );
			//this.Elements.Add( this.AuthorUrlUI );
			//this.Elements.Insert( 0, this.AuthorText2UI );
			//this.Elements.Insert( 0, this.SupportText1Label );
			//this.Elements.Add( this.SupportUrl );
			//this.Elements.Insert( 0, this.SupportText2Label );
			this.Elements.Add( this.PatreonLabel );
			this.Elements.Add( this.PatreonButton );

			this.EnableModTagsLabel?.Top.Set( this.EnableModTagsLabel.Top.Pixels + ( 2 * this.RowHeight * this.Scale ), 0f );

			/*this.RecomInfoTextUI.Recalculate();
			this.Elements.Add( this.RecomInfoTextUI );
			this.RecomInfoUrlUI.Recalculate();
			this.Elements.Add( this.RecomInfoUrlUI );*/

			this.ModderTextLabel.Recalculate();
			this.ModderUrl.Recalculate();
			//this.AuthorText1UI.Recalculate();
			//this.AuthorUrlUI.Recalculate();
			//this.AuthorText2UI.Recalculate();
			//this.SupportText1Label.Recalculate();
			//this.SupportUrl.Recalculate();
			//this.SupportText2Label.Recalculate();
			this.PatreonLabel.Recalculate();
			this.PatreonButton.Recalculate();

			LoadHooks.AddModUnloadHook( () => {
				Main.OnPostDraw -= SupportInfoDisplay._Draw;
			} );
		}


		////////////////

		public Rectangle GetInnerBox() {
			return new Rectangle(
				Main.screenWidth - (int)this.Width - 4,
				4,
				(int)this.Width,
				(this.IsExtended ? 74 : 32 )   //104:40
			);
		}


		////////////////

		private void RemoveModTagsMessage() {
			this.EnableModTagsLabel.Remove();
			this.Elements.Remove( this.EnableModTagsLabel );
			this.EnableModTagsLabel = null;
		}
	}
}
