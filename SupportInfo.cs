using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.Support {
	class SupportInfoDisplay {
		private readonly UIText HeadTextUI;
		private readonly UIWebUrl HeadUrlUI;
		private readonly UIText ModderTextUI; 
		private readonly UIWebUrl ModderUrlUI;
		//private readonly UIText RecomInfoTextUI;
		//private readonly UIWebUrl RecomInfoUrlUI;
		private readonly UIText AuthorText1UI;
		private readonly UIWebUrl AuthorUrlUI;
		private readonly UIText AuthorText2UI;
		private readonly UIText SupportText1UI;
		private readonly UIWebUrl SupportUrlUI;
		private readonly UIText SupportText2UI;

		private IList<UIElement> Elements = new List<UIElement>();

		private bool IsClicking = false;



		////////////////

		internal SupportInfoDisplay() {
			if( Main.dedServ ) { return; }

			var mymod = ModHelpersMod.Instance;
			
			float x_off = 312f;
			float y = 12f;
			float row_height = 30f;
			int row = 0;

			////

			this.HeadTextUI = new UIText( "Powered by:", 1.1f );
			this.HeadTextUI.Left.Set( -x_off, 1f );
			this.HeadTextUI.Top.Set( y + (row * row_height), 0f );
			this.HeadTextUI.TextColor = Color.Lerp( Color.White, Color.Gold, 0.25f );
			this.HeadTextUI.Recalculate();
			this.Elements.Add( this.HeadTextUI );

			this.HeadUrlUI = new UIWebUrl( UITheme.Vanilla, "Mod Helpers v "+mymod.Version.ToString(), "https://forums.terraria.org/index.php?threads/.63670/", true, 1.1f );
			this.HeadUrlUI.Left.Set( -( x_off - 114f ), 1f );
			this.HeadUrlUI.Top.Set( y + (row * row_height), 0f );
			this.HeadUrlUI.Recalculate();
			this.Elements.Add( this.HeadUrlUI );
			
			////

			y += 4;
			row++;

			this.ModderTextUI = new UIText( "Do you make mods?" );
			this.ModderTextUI.Left.Set( -x_off, 1f );
			this.ModderTextUI.Top.Set( y + (row * row_height), 0f );
			this.ModderTextUI.Recalculate();
			this.Elements.Add( this.ModderTextUI );

			this.ModderUrlUI = new UIWebUrl( UITheme.Vanilla, "Read this.", "https://forums.terraria.org/index.php?threads/.63670/#modders", true, 1f );
			this.ModderUrlUI.Left.Set( -(x_off - 166f), 1f );
			this.ModderUrlUI.Top.Set( y + (row * row_height), 0f );
			this.ModderUrlUI.Recalculate();
			this.Elements.Add( this.ModderUrlUI );

			////

			/*row++;

			this.RecomInfoTextUI = new UIText( "Feedback? Confused? Bored?" );
			this.RecomInfoTextUI.Left.Set( -x_off, 1f );
			this.RecomInfoTextUI.Top.Set( y + (row * row_height), 0f );
			this.RecomInfoTextUI.Recalculate();
			this.Elements.Add( this.RecomInfoTextUI );

			this.RecomInfoUrlUI = new UIWebUrl( UITheme.Vanilla, "Discord", "https://discord.gg/a2AwYtj", true, 1f );
			this.RecomInfoUrlUI.Left.Set( -( x_off - 242f ), 1f );
			this.RecomInfoUrlUI.Top.Set( y + (row * row_height), 0f );
			this.RecomInfoUrlUI.Recalculate();
			this.Elements.Add( this.RecomInfoUrlUI );*/

			////

			row++;

			this.AuthorText1UI = new UIText( "I make" );
			this.AuthorText1UI.Left.Set( -x_off, 1f );
			this.AuthorText1UI.Top.Set( y + (row * row_height), 0f );
			this.AuthorText1UI.Recalculate();
			this.Elements.Insert( 0, this.AuthorText1UI );

			this.AuthorUrlUI = new UIWebUrl( UITheme.Vanilla, "other specialized mods", "https://forums.terraria.org/index.php?threads/.63713/", true, 1f );
			this.AuthorUrlUI.Left.Set( -( x_off - 62f ), 1f );
			this.AuthorUrlUI.Top.Set( y + (row * row_height), 0f );
			this.AuthorUrlUI.Recalculate();
			this.Elements.Add( this.AuthorUrlUI );
			
			this.AuthorText2UI = new UIText( ", too." );
			this.AuthorText2UI.Left.Set( -( x_off - 244f ), 1f );
			this.AuthorText2UI.Top.Set( y + (row * row_height), 0f );
			this.AuthorText2UI.Recalculate();
			this.Elements.Insert( 0, this.AuthorText2UI );

			////

			row++;

			this.SupportText1UI = new UIText( "Please" );
			this.SupportText1UI.Left.Set( -x_off, 1f );
			this.SupportText1UI.Top.Set( y + (row * row_height), 0f );
			this.SupportText1UI.Recalculate();
			this.SupportText1UI.TextColor = Color.MediumSpringGreen;
			this.Elements.Insert( 0, this.SupportText1UI );

			this.SupportUrlUI = new UIWebUrl( UITheme.Vanilla.Clone(), "support", "https://www.patreon.com/hamstar0", true, 1f );
			this.SupportUrlUI.Left.Set( -(x_off - 54), 1f );
			this.SupportUrlUI.Top.Set( y + (row * row_height), 0f );
			this.SupportUrlUI.Recalculate();
			this.Elements.Add( this.SupportUrlUI );

			this.SupportText2UI = new UIText( "my mods!" );
			this.SupportText2UI.Left.Set( -(x_off - 119), 1f );
			this.SupportText2UI.Top.Set( y + (row * row_height), 0f );
			this.SupportText2UI.Recalculate();
			this.SupportText2UI.TextColor = Color.MediumSpringGreen;
			this.Elements.Insert( 0, this.SupportText2UI );

			Main.OnPostDraw += SupportInfoDisplay._Draw;
		}


		~SupportInfoDisplay() {
			Main.OnPostDraw -= SupportInfoDisplay._Draw;
		}


		////////////////

		public void Update() {
			bool is_clicking = Main.mouseLeft && !this.IsClicking;

			this.IsClicking = Main.mouseLeft;

			for( int i=0; i<this.Elements.Count; i++ ) {
				var url = this.Elements[i] as UIWebUrl;
				if( url == null ) { continue; }

				if( url.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
					if( is_clicking ) { url.Click( null ); }
					url.MouseOver( null );
				} else {
					if( url.IsMouseHovering ) {
						url.MouseOut( null );
					}
				}
			}
		}


		////////////////

		private static void _Draw( GameTime gt ) {
			if( !Main.gameMenu ) { return; }
			if( Main.spriteBatch == null ) { return; }

			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.Config == null || mymod.Config.HideTitleInfo ) { return; }

			if( Main.MenuUI.CurrentState != null ) {
				Type ui_type = Main.MenuUI.CurrentState.GetType();

				if( ui_type.Name != "UIMods" && MenuContextService.ContainsMenuLoaders(ui_type.Name) ) {
					return;
				}
			}

			bool is_begun = XnaHelpers.IsMainSpriteBatchBegun();

			if( !is_begun ) {
				Main.spriteBatch.Begin();
			}

			mymod.SupportInfo?.Update();
			mymod.SupportInfo?.Draw( Main.spriteBatch );

			if( !is_begun ) {
				Main.spriteBatch.End();
			}
		}


		public void Draw( SpriteBatch sb ) {
			foreach( var elem in this.Elements ) {
				elem.Recalculate();
			}

			this.SupportUrlUI.Theme.UrlColor = Color.Lerp( UITheme.Vanilla.UrlColor, AnimatedColors.Ether.CurrentColor, 0.35f );
			this.SupportUrlUI.Theme.UrlLitColor = Color.Lerp( UITheme.Vanilla.UrlLitColor, AnimatedColors.Strobe.CurrentColor, 0.5f );
			this.SupportUrlUI.Theme.UrlLitColor = Color.Lerp( this.SupportUrlUI.Theme.UrlLitColor, AnimatedColors.DiscoFast.CurrentColor, 0.75f );
			this.SupportUrlUI.RefreshTheme();

			foreach( var elem in this.Elements ) {
				if( elem is UIWebUrl ) { continue; }
				elem.Draw( sb );
			}
			foreach( var elem in this.Elements.Reverse() ) {
				if( !( elem is UIWebUrl ) ) { continue; }
				elem.Draw( sb );
			}
		}
	}
}
