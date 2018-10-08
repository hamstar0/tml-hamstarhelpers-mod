using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.Menus.Support {
	class SupportInfoDisplay {
		private readonly UIText HeadTextUI;
		private readonly UIWebUrl HeadUrlUI;
		private readonly UIText ModderTextUI; 
		private readonly UIWebUrl ModderUrlUI;
		private readonly UIText RecomInfoTextUI;
		private readonly UIWebUrl RecomInfoUrlUI;
		private readonly UIText SupportText1UI;
		private readonly UIWebUrl SupportUrlUI;
		private readonly UIText SupportText2UI;

		private bool IsClicking = false;



		////////////////

		internal SupportInfoDisplay() {
			if( Main.dedServ ) { return; }

			var mymod = ModHelpersMod.Instance;
			
			float x_off = 312f;
			float y = 4f;
			float row_height = 28f;

			////

			this.HeadTextUI = new UIText( "Powered by:", 1.1f );
			this.HeadTextUI.Left.Set( -x_off, 1f );
			this.HeadTextUI.Top.Set( y, 0f );
			this.HeadTextUI.TextColor = Color.Lerp( Color.White, Color.Gold, 0.25f );
			this.HeadTextUI.Recalculate();

			y += 4;

			this.HeadUrlUI = new UIWebUrl( UITheme.Vanilla, "Mod Helpers  v"+mymod.Version.ToString(), "https://forums.terraria.org/index.php?threads/.63670/", true, 1.1f );
			this.HeadUrlUI.Left.Set( -( x_off - 116f ), 1f );
			this.HeadUrlUI.Top.Set( y, 0f );
			this.HeadUrlUI.Recalculate();

			////
			
			this.ModderTextUI = new UIText( "Do you make mods?" );
			this.ModderTextUI.Left.Set( -x_off, 1f );
			this.ModderTextUI.Top.Set( y + row_height, 0f );
			this.ModderTextUI.Recalculate();

			this.ModderUrlUI = new UIWebUrl( UITheme.Vanilla, "Read this.", "https://forums.terraria.org/index.php?threads/.63670/#modders", true );
			this.ModderUrlUI.Left.Set( -(x_off - 164f), 1f );
			this.ModderUrlUI.Top.Set( y + row_height, 0f );
			this.ModderUrlUI.Recalculate();

			////

			this.RecomInfoTextUI = new UIText( "Discussion:" );
			this.RecomInfoTextUI.Left.Set( -x_off, 1f );
			this.RecomInfoTextUI.Top.Set( y + ( row_height * 2 ), 0f );
			this.RecomInfoTextUI.Recalculate();

			this.RecomInfoUrlUI = new UIWebUrl( UITheme.Vanilla, "Discord", "https://discord.gg/a2AwYtj", true );
			this.RecomInfoUrlUI.Left.Set( -( x_off - 96f ), 1f );
			this.RecomInfoUrlUI.Top.Set( y + ( row_height * 2 ), 0f );
			this.RecomInfoUrlUI.Recalculate();

			////

			this.SupportText1UI = new UIText( "Support" );
			this.SupportText1UI.Left.Set( -x_off, 1f );
			this.SupportText1UI.Top.Set( y + (row_height * 3), 0f );
			this.SupportText1UI.Recalculate();
			this.SupportText1UI.TextColor = Color.MediumSpringGreen;

			this.SupportUrlUI = new UIWebUrl( UITheme.Vanilla.Clone(), "hamstar's", "https://www.patreon.com/hamstar0", true, 1f );
			this.SupportUrlUI.Left.Set( -(x_off - 70), 1f );
			this.SupportUrlUI.Top.Set( y + (row_height * 3), 0f );
			this.SupportUrlUI.Recalculate();

			this.SupportText2UI = new UIText( "mods!" );
			this.SupportText2UI.Left.Set( -(x_off - 156), 1f );
			this.SupportText2UI.Top.Set( y + (row_height * 3), 0f );
			this.SupportText2UI.Recalculate();
			this.SupportText2UI.TextColor = Color.MediumSpringGreen;

			Main.OnPostDraw += SupportInfoDisplay._Draw;
		}


		~SupportInfoDisplay() {
			Main.OnPostDraw -= SupportInfoDisplay._Draw;
		}


		////////////////

		public void Update() {
			bool is_clicking = Main.mouseLeft && !this.IsClicking;

			this.IsClicking = Main.mouseLeft;

			if( this.HeadUrlUI.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
				if( is_clicking ) { this.HeadUrlUI.Click( null ); }
				this.HeadUrlUI.MouseOver( null );
			} else {
				if( this.HeadUrlUI.IsMouseHovering ) {
					this.HeadUrlUI.MouseOut( null );
				}
			}

			if( this.ModderUrlUI.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
				if( is_clicking ) { this.ModderUrlUI.Click( null ); }
				this.ModderUrlUI.MouseOver( null );
			} else {
				if( this.ModderUrlUI.IsMouseHovering ) {
					this.ModderUrlUI.MouseOut( null );
				}
			}

			if( this.RecomInfoUrlUI.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
				if( is_clicking ) { this.RecomInfoUrlUI.Click( null ); }
				this.RecomInfoUrlUI.MouseOver( null );
			} else {
				if( this.RecomInfoUrlUI.IsMouseHovering ) {
					this.RecomInfoUrlUI.MouseOut( null );
				}
			}

			if( this.SupportUrlUI.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
				if( is_clicking ) { this.SupportUrlUI.Click( null ); }
				this.SupportUrlUI.MouseOver( null );
			} else {
				if( this.SupportUrlUI.IsMouseHovering ) {
					this.SupportUrlUI.MouseOut( null );
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
			this.HeadTextUI.Recalculate();
			this.HeadUrlUI.Recalculate();
			this.ModderTextUI.Recalculate();
			this.ModderUrlUI.Recalculate();
			this.RecomInfoTextUI.Recalculate();
			this.RecomInfoUrlUI.Recalculate();
			this.SupportText1UI.Recalculate();
			this.SupportUrlUI.Recalculate();
			this.SupportText2UI.Recalculate();

			this.SupportUrlUI.Theme.UrlColor = Color.Lerp( UITheme.Vanilla.UrlColor, AnimatedColors.Ether.CurrentColor, 0.5f );
			this.SupportUrlUI.Theme.UrlLitColor = Color.Lerp( UITheme.Vanilla.UrlLitColor, AnimatedColors.Ether.CurrentColor, 0.5f );
			this.SupportUrlUI.RefreshTheme();

			this.SupportText2UI.Draw( sb );
			this.SupportUrlUI.Draw( sb );
			this.SupportText1UI.Draw( sb );
			this.RecomInfoUrlUI.Draw( sb );
			this.RecomInfoTextUI.Draw( sb );
			this.ModderUrlUI.Draw( sb );
			this.ModderTextUI.Draw( sb );
			this.HeadUrlUI.Draw( sb );
			this.HeadTextUI.Draw( sb );
		}
	}
}
