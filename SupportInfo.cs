using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
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
		private readonly UIText ExtendTextUI;

		private IList<UIElement> Elements = new List<UIElement>();

		private bool IsHovingBox = false;
		private bool IsClicking = false;
		private bool IsExtended = false;



		////////////////

		internal SupportInfoDisplay( float x_off = 244f, float y_beg = 12f, float row_height = 30f, float scale = 0.8f ) {
			if( Main.dedServ ) { return; }

			float y = y_beg;
			float row = 0;
			var mymod = ModHelpersMod.Instance;

			////

			this.HeadTextUI = new UIText( "Powered by:", 1.1f * scale );
			this.HeadTextUI.Left.Set( -x_off, 1f );
			this.HeadTextUI.Top.Set( (4f + y) * scale, 0f );
			this.HeadTextUI.TextColor = Color.Lerp( Color.White, Color.Gold, 0.25f );
			this.HeadTextUI.Recalculate();

			this.HeadUrlUI = new UIWebUrl( UITheme.Vanilla, "Mod Helpers v " + mymod.Version.ToString(), "https://forums.terraria.org/index.php?threads/.63670/", true, 1.1f * scale );
			this.HeadUrlUI.Left.Set( -( x_off - ( 114f * scale ) ), 1f );
			this.HeadUrlUI.Top.Set( (4f + y) * scale, 0f );
			this.HeadUrlUI.Recalculate();

			this.ExtendTextUI = new UIText( "..." );
			this.ExtendTextUI.Left.Set( -(x_off * 0.5f) - 16f, 1f );
			this.ExtendTextUI.Top.Set( (-8f + y + row_height) * scale, 0f );
			this.ExtendTextUI.Recalculate();

			////

			y += 4f * scale;
			row += 1;

			this.ModderTextUI = new UIText( "Do you make mods?", 1f * scale );
			this.ModderTextUI.Left.Set( -x_off, 1f );
			this.ModderTextUI.Top.Set( ( y + ( row * row_height ) ) * scale, 0f );

			this.ModderUrlUI = new UIWebUrl( UITheme.Vanilla, "Read this.", "https://forums.terraria.org/index.php?threads/.63670/#modders", true, 1f * scale );
			this.ModderUrlUI.Left.Set( -( x_off - ( 166f * scale ) ), 1f );
			this.ModderUrlUI.Top.Set( ( y + ( row * row_height ) ) * scale, 0f );

			////

			/*
			row += 1;

			this.RecomInfoTextUI = new UIText( "Feedback? Confused? Bored?", 1f * scale );
			this.RecomInfoTextUI.Left.Set( -x_off, 1f );
			this.RecomInfoTextUI.Top.Set( (y + (row * row_height)) * scale, 0f );

			this.RecomInfoUrlUI = new UIWebUrl( UITheme.Vanilla, "Discord", "https://discord.gg/a2AwYtj", true, 1f * scale );
			this.RecomInfoUrlUI.Left.Set( -( x_off - (242f * scale) ), 1f );
			this.RecomInfoUrlUI.Top.Set( (y + (row * row_height) * scale), 0f );*/

			////

			row += 1;

			this.AuthorText1UI = new UIText( "I make", 1f * scale );
			this.AuthorText1UI.Left.Set( -x_off, 1f );
			this.AuthorText1UI.Top.Set( ( y + ( row * row_height ) ) * scale, 0f );

			this.AuthorUrlUI = new UIWebUrl( UITheme.Vanilla, "other specialized mods", "https://forums.terraria.org/index.php?threads/.63713/", true, 1f * scale );
			this.AuthorUrlUI.Left.Set( -( x_off - ( 60f * scale ) ), 1f );
			this.AuthorUrlUI.Top.Set( ( y + ( row * row_height ) ) * scale, 0f );

			this.AuthorText2UI = new UIText( ", too.", 1f * scale );
			this.AuthorText2UI.Left.Set( -( x_off - ( 242f * scale ) ), 1f );
			this.AuthorText2UI.Top.Set( ( y + ( row * row_height ) ) * scale, 0f );

			////

			row += 1;

			this.SupportText1UI = new UIText( "Please", 1f * scale );
			this.SupportText1UI.Left.Set( -x_off, 1f );
			this.SupportText1UI.Top.Set( ( y + ( row * row_height ) ) * scale, 0f );
			//this.SupportText1UI.TextColor = Color.MediumSpringGreen;

			this.SupportUrlUI = new UIWebUrl( UITheme.Vanilla.Clone(), "support", "https://www.patreon.com/hamstar0", true, 1f * scale );
			this.SupportUrlUI.Left.Set( -( x_off - ( 54f * scale ) ), 1f );
			this.SupportUrlUI.Top.Set( ( y + ( row * row_height ) ) * scale, 0f );

			this.SupportText2UI = new UIText( "my mods!", 1f * scale );
			this.SupportText2UI.Left.Set( -( x_off - ( 119f * scale ) ), 1f );
			this.SupportText2UI.Top.Set( ( y + ( row * row_height ) ) * scale, 0f );
			//this.SupportText2UI.TextColor = Color.MediumSpringGreen;

			////

			this.Elements.Add( this.HeadTextUI );
			this.Elements.Add( this.HeadUrlUI );
			this.Elements.Add( this.ExtendTextUI );

			Main.OnPostDraw += SupportInfoDisplay._Draw;
		}


		private void ExpandUI() {
			this.Elements.Add( this.ModderTextUI );
			this.Elements.Add( this.ModderUrlUI );
			this.Elements.Insert( 0, this.AuthorText1UI );
			this.Elements.Add( this.AuthorUrlUI );
			this.Elements.Insert( 0, this.AuthorText2UI );
			this.Elements.Insert( 0, this.SupportText1UI );
			this.Elements.Add( this.SupportUrlUI );
			this.Elements.Insert( 0, this.SupportText2UI );

			/*this.RecomInfoTextUI.Recalculate();
			this.Elements.Add( this.RecomInfoTextUI );
			this.RecomInfoUrlUI.Recalculate();
			this.Elements.Add( this.RecomInfoUrlUI );*/

			this.ModderTextUI.Recalculate();
			this.ModderUrlUI.Recalculate();
			this.AuthorText1UI.Recalculate();
			this.AuthorUrlUI.Recalculate();
			this.AuthorText2UI.Recalculate();
			this.SupportText1UI.Recalculate();
			this.SupportUrlUI.Recalculate();
			this.SupportText2UI.Recalculate();
		}


		~SupportInfoDisplay() {
			Main.OnPostDraw -= SupportInfoDisplay._Draw;
		}


		////////////////

		public Rectangle GetBox() {
			return new Rectangle( Main.screenWidth - 252, 4, 248, ( this.IsExtended ? 104 : 40 ) );
		}


		////////////////

		public void Update() {
			bool is_clicking = Main.mouseLeft && !this.IsClicking;

			this.IsClicking = Main.mouseLeft;
			this.IsHovingBox = this.GetBox().Contains( Main.mouseX, Main.mouseY );

			for( int i=0; i<this.Elements.Count; i++ ) {
				var elem = this.Elements[i];
				if( !(elem is UIWebUrl) ) {
					if( !(elem is UIText) || ((UIText)elem).Text != "..." ) {
						continue;
					}
				}

				if( elem.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
					if( is_clicking ) { elem.Click( null ); }
					elem.MouseOver( null );
				} else {
					if( elem.IsMouseHovering ) {
						elem.MouseOut( null );
					}
				}
			}
			
			if( this.IsHovingBox && is_clicking ) {
				if( this.IsExtended ) { return; }
				this.IsExtended = true;

				this.ExtendTextUI.Remove();
				this.ExpandUI();
			}
		}


		////////////////

		private static void _Draw( GameTime gt ) {
			if( !Main.gameMenu ) { return; }
			if( Main.spriteBatch == null ) { return; }

			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.Config == null || mymod.Config.DisableSupportLinks ) { return; }

			if( Main.MenuUI.CurrentState != null ) {
				Type ui_type = Main.MenuUI.CurrentState.GetType();

				if( ui_type.Name != "UIMods" && MenuContextService.ContainsMenuContexts(ui_type.Name) ) {
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

			var box_color = new Color( 256, 0, 32 );
			var box_edge_color = new Color( 255, 224, 224 );
			float color_mul = 0.25f;

			if( this.IsHovingBox ) {
				this.ExtendTextUI.TextColor = Color.White;
				color_mul = 0.3f;
			} else {
				this.ExtendTextUI.TextColor = AnimatedColors.Ether.CurrentColor;
			}

			var rect = new Rectangle( Main.screenWidth - 252, 4, 248, (this.IsExtended ? 104 : 40) );
			HudHelpers.DrawBorderedRect( sb, box_color * color_mul, box_edge_color * color_mul, rect, 4 );

			if( this.SupportUrlUI != null ) {
				this.SupportUrlUI.Theme.UrlColor = Color.Lerp( UITheme.Vanilla.UrlColor, AnimatedColors.Ether.CurrentColor, 0.25f );
				this.SupportUrlUI.Theme.UrlLitColor = Color.Lerp( UITheme.Vanilla.UrlLitColor, AnimatedColors.Strobe.CurrentColor, 0.5f );
				this.SupportUrlUI.Theme.UrlLitColor = Color.Lerp( this.SupportUrlUI.Theme.UrlLitColor, AnimatedColors.DiscoFast.CurrentColor, 0.75f );
				this.SupportUrlUI.RefreshTheme();
			}

			foreach( var elem in this.Elements ) {
				if( elem is UIWebUrl ) { continue; }
				elem.Draw( sb );
			}
			foreach( var elem in this.Elements.Reverse() ) {
				if( !( elem is UIWebUrl ) ) { continue; }
				elem.Draw( sb );
			}
			
			Vector2 bonus = Main.DrawThickCursor( false );
			Main.DrawCursor( bonus, false );
		}
	}
}
