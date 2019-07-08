using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Helpers.XNA;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.Support {
	/// @private
	internal class SupportInfoDisplay {
		private readonly UIText HeadTextUI;
		private readonly UIWebUrl HeadUrlUI;
		private readonly UIText ModderTextUI; 
		private readonly UIWebUrl ModderUrlUI;
		//private readonly UIText RecomInfoTextUI;
		//private readonly UIWebUrl RecomInfoUrlUI;
		//private readonly UIText AuthorText1UI;
		//private readonly UIWebUrl AuthorUrlUI;
		//private readonly UIText AuthorText2UI;
		private readonly UIText SupportText1UI;
		private readonly UIWebUrl SupportUrlUI;
		private readonly UIText SupportText2UI;
		private readonly UIText ExtendTextUI;

		private IList<UIElement> Elements = new List<UIElement>();

		private bool IsHovingBox = false;
		private bool IsClicking = false;
		private bool IsExtended = false;

		private float Width;



		////////////////

		internal SupportInfoDisplay( float width = 248f, float yBeg = 8f, float rowHeight = 30f, float scale = 0.8f ) {
			if( Main.dedServ ) { return; }

			float y = yBeg;
			float row = 0;
			var mymod = ModHelpersMod.Instance;
			this.Width = width;

			////

			this.HeadTextUI = new UIText( "Powered by:", 1.1f * scale );
			this.HeadTextUI.Left.Set( -width, 1f );
			this.HeadTextUI.Top.Set( (4f + y) * scale, 0f );
			this.HeadTextUI.TextColor = Color.Lerp( Color.White, Color.Gold, 0.25f );
			this.HeadTextUI.Recalculate();

			this.HeadUrlUI = new UIWebUrl( UITheme.Vanilla, "Mod Helpers v" + mymod.Version.ToString(), "https://forums.terraria.org/index.php?threads/.63670/", true, 1.1f * scale );
			this.HeadUrlUI.Left.Set( -( width - ( 114f * scale ) ), 1f );
			this.HeadUrlUI.Top.Set( (4f + y) * scale, 0f );
			this.HeadUrlUI.Recalculate();

			this.ExtendTextUI = new UIText( "..." );
			this.ExtendTextUI.Left.Set( -(width * 0.5f) - 16f, 1f );
			this.ExtendTextUI.Top.Set( (-14f + y + rowHeight) * scale, 0f );
			this.ExtendTextUI.Recalculate();

			////

			y += 6f * scale;
			row += 1;

			this.ModderTextUI = new UIText( "Do you make mods?", 1f * scale );
			this.ModderTextUI.Left.Set( -width, 1f );
			this.ModderTextUI.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );

			this.ModderUrlUI = new UIWebUrl( UITheme.Vanilla, "Read this.", "https://forums.terraria.org/index.php?threads/.63670/#modders", true, 1f * scale );
			this.ModderUrlUI.Left.Set( -( width - ( 166f * scale ) ), 1f );
			this.ModderUrlUI.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );

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

			row += 1;

			this.SupportText1UI = new UIText( "Please", 1f * scale );
			this.SupportText1UI.Left.Set( -width, 1f );
			this.SupportText1UI.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );
			//this.SupportText1UI.TextColor = Color.MediumSpringGreen;

			this.SupportUrlUI = new UIWebUrl( UITheme.Vanilla.Clone(), "support", "https://www.patreon.com/hamstar0", true, 1f * scale );
			this.SupportUrlUI.Left.Set( -( width - ( 54f * scale ) ), 1f );
			this.SupportUrlUI.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );

			this.SupportText2UI = new UIText( "upkeep & improvement", 1f * scale );
			this.SupportText2UI.Left.Set( -( width - ( 120f * scale ) ), 1f );
			this.SupportText2UI.Top.Set( ( y + ( row * rowHeight ) ) * scale, 0f );
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
			//this.Elements.Insert( 0, this.AuthorText1UI );
			//this.Elements.Add( this.AuthorUrlUI );
			//this.Elements.Insert( 0, this.AuthorText2UI );
			this.Elements.Insert( 0, this.SupportText1UI );
			this.Elements.Add( this.SupportUrlUI );
			this.Elements.Insert( 0, this.SupportText2UI );

			/*this.RecomInfoTextUI.Recalculate();
			this.Elements.Add( this.RecomInfoTextUI );
			this.RecomInfoUrlUI.Recalculate();
			this.Elements.Add( this.RecomInfoUrlUI );*/

			this.ModderTextUI.Recalculate();
			this.ModderUrlUI.Recalculate();
			//this.AuthorText1UI.Recalculate();
			//this.AuthorUrlUI.Recalculate();
			//this.AuthorText2UI.Recalculate();
			this.SupportText1UI.Recalculate();
			this.SupportUrlUI.Recalculate();
			this.SupportText2UI.Recalculate();
		}


		~SupportInfoDisplay() {
			Main.OnPostDraw -= SupportInfoDisplay._Draw;
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

		public void Update() {
			bool isClicking = Main.mouseLeft && !this.IsClicking;

			this.IsClicking = Main.mouseLeft;
			this.IsHovingBox = this.GetInnerBox().Contains( Main.mouseX, Main.mouseY );

			for( int i=0; i<this.Elements.Count; i++ ) {
				var elem = this.Elements[i];
				if( !(elem is UIWebUrl) ) {
					if( !(elem is UIText) || ((UIText)elem).Text != "..." ) {
						continue;
					}
				}

				if( elem.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
					if( isClicking ) { elem.Click( null ); }
					elem.MouseOver( null );
				} else {
					if( elem.IsMouseHovering ) {
						elem.MouseOut( null );
					}
				}
			}
			
			if( this.IsHovingBox && isClicking ) {
				if( this.IsExtended ) { return; }
				this.IsExtended = true;
				
				this.ExtendTextUI.Remove();
				this.Elements.Remove( this.ExtendTextUI );
				this.ExpandUI();
			}
		}


		////////////////

		private static void _Draw( GameTime gt ) {
			if( !Main.gameMenu ) { return; }
			if( Main.spriteBatch == null ) { return; }

			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.Config == null || mymod.Config.DisableSupportLinks || Main.MenuUI == null ) { return; }

			try {
				if( Main.MenuUI.CurrentState != null ) {
					Type uiType = Main.MenuUI.CurrentState.GetType();

					if( uiType.Name != "UIMods" && MenuContextService.ContainsMenuContexts( uiType.Name ) ) {
						return;
					}
				}
				
				bool _;
				XNAHelpers.DrawBatch( ( sb ) => {
					mymod.SupportInfo?.Update();
					mymod.SupportInfo?.Draw( sb );
				}, out _ );
			} catch( Exception e ) {
				LogHelpers.LogOnce( e.ToString() );
			}
		}


		public void Draw( SpriteBatch sb ) {
			foreach( var elem in this.Elements ) {
				elem.Recalculate();
			}

			var boxColor = new Color( 256, 0, 32 );
			var boxEdgeColor = new Color( 255, 224, 224 );
			float colorMul = 0.25f;

			if( this.IsHovingBox ) {
				this.ExtendTextUI.TextColor = Color.White;
				colorMul = 0.3f;
			} else {
				this.ExtendTextUI.TextColor = AnimatedColors.Air.CurrentColor;
			}

			//var rect = new Rectangle( Main.screenWidth - 252, 4, 248, (this.IsExtended ? 104 : 40) );
			var rect = this.GetInnerBox();
			HUDHelpers.DrawBorderedRect( sb, boxColor * colorMul, boxEdgeColor * colorMul, rect, 4 );

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
