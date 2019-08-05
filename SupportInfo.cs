using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Helpers.XNA;
using HamstarHelpers.Internals.Menus.ModTags;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.Timers;
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
		public static Color HeaderLabelColor = Color.Lerp( Color.White, Color.Gold, 0.25f );



		////////////////

		private float RowHeight;
		private float Scale;

		private readonly UIText HeadTextLabel;
		private readonly UIWebUrl HeadUrl;
		private readonly UIText ModderTextLabel;
		private readonly UIWebUrl ModderUrl;
		//private readonly UIText RecomInfoTextLabel;
		//private readonly UIWebUrl RecomInfoUrl;
		//private readonly UIText AuthorText1Label;
		//private readonly UIWebUrl AuthorUrl;
		//private readonly UIText AuthorText2Label;
		private readonly UIText SupportText1Label;
		private readonly UIWebUrl SupportUrl;
		private readonly UIText SupportText2Label;
		private readonly UIText ExtendTextLabel;
		private readonly UIText EnableModTagsLabel = null;

		private IList<UIElement> Elements = new List<UIElement>();

		private bool IsHovingBox = false;
		private bool IsClicking = false;
		private bool IsExtended = false;

		private float Width;



		////////////////

		internal SupportInfoDisplay( float width = 248f, float yBeg = 8f, float rowHeight = 30f, float scale = 0.8f ) {
			if( Main.dedServ ) { return; }

			var mymod = ModHelpersMod.Instance;
			float y = yBeg;
			float row = 0;
			this.Width = width;

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

			this.ExtendTextLabel = new UIText( "..." );
			this.ExtendTextLabel.Left.Set( -(width * 0.5f) - 16f, 1f );
			this.ExtendTextLabel.Top.Set( (-14f + y + rowHeight) * scale, 0f );
			this.ExtendTextLabel.Recalculate();

			////

			row += 1;

			if( !mymod.Config.DisableModTags ) {
				this.EnableModTagsLabel = new UIText( "Enable Mod Tags", 1f * ( scale + 0.2f ) );
				this.EnableModTagsLabel.TextColor = Color.Orange;
				this.EnableModTagsLabel.Left.Set( -( width - 48f ), 1f );
				this.EnableModTagsLabel.Top.Set( ( y + ( (row+1) * rowHeight ) ) * scale, 0f );
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

			row += 1;

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
			//this.SupportText2UI.TextColor = Color.MediumSpringGreen;

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
			this.Elements.Insert( 0, this.SupportText1Label );
			this.Elements.Add( this.SupportUrl );
			this.Elements.Insert( 0, this.SupportText2Label );

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
			this.SupportText1Label.Recalculate();
			this.SupportUrl.Recalculate();
			this.SupportText2Label.Recalculate();
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

			for( int i = 0; i < this.Elements.Count; i++ ) {
				var elem = this.Elements[i];
				if( !( elem is UIWebUrl ) ) {
					if( !( elem is UIText ) || ( (UIText)elem ).Text != "..." ) {
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

			if( isClicking && this.IsHovingBox ) {
				if( this.IsExtended ) { return; }
				this.IsExtended = true;

				this.ExtendTextLabel.Remove();
				this.Elements.Remove( this.ExtendTextLabel );
				this.ExpandUI();
			}

			if( this.EnableModTagsLabel != null ) {
				if( isClicking && this.EnableModTagsLabel.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
					this.EnableModTagsLabel.Remove();
					this.Elements.Remove( this.EnableModTagsLabel );

					ModInfoTagsMenuContext.Initialize( false );
					ModBrowserTagsMenuContext.Initialize( false );

					Timers.SetTimer( "ModHelpersModBrowserActivate", 5, () => {
						MainMenuHelpers.LoadModBrowser();
						return false;
					} );
				}
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

			//var boxColor = new Color( 256, 0, 32 );
			//var boxEdgeColor = new Color( 255, 224, 224 );
			//float colorMul = 0.25f;
			float textColorMul = 1f;

			if( this.IsHovingBox ) {
				this.ExtendTextLabel.TextColor = Color.White;
				//colorMul = 0.3f;
			} else {
				if( !this.IsExtended ) {
					textColorMul = 0.8f;
				}
				this.ExtendTextLabel.TextColor = AnimatedColors.Air.CurrentColor;
			}

			////var rect = new Rectangle( Main.screenWidth - 252, 4, 248, (this.IsExtended ? 104 : 40) );
			//var rect = this.GetInnerBox();
			//HUDHelpers.DrawBorderedRect( sb, boxColor * colorMul, boxEdgeColor * colorMul, rect, 4 );

			if( this.SupportUrl != null ) {
				this.SupportUrl.Theme.UrlColor = Color.Lerp( UITheme.Vanilla.UrlColor, AnimatedColors.Ether.CurrentColor, 0.25f );
				this.SupportUrl.Theme.UrlLitColor = Color.Lerp( UITheme.Vanilla.UrlLitColor, AnimatedColors.Strobe.CurrentColor, 0.5f );
				this.SupportUrl.Theme.UrlLitColor = Color.Lerp( this.SupportUrl.Theme.UrlLitColor, AnimatedColors.DiscoFast.CurrentColor, 0.75f );
				this.SupportUrl.RefreshTheme();
			}

			if( this.EnableModTagsLabel != null ) {
				if( !this.EnableModTagsLabel.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
					this.EnableModTagsLabel.TextColor = Color.Orange * 0.8f;
				} else {
					this.EnableModTagsLabel.TextColor = Color.Orange;
				}
			}

			foreach( var elem in this.Elements ) {
				if( elem is UIWebUrl ) { continue; }
				if( elem is UIText ) {
					if( elem == this.HeadTextLabel ) {
						((UIText)elem).TextColor = SupportInfoDisplay.HeaderLabelColor * textColorMul;
					} else if( elem != this.ExtendTextLabel && elem != this.EnableModTagsLabel ) {
						((UIText)elem).TextColor = Color.White * textColorMul;
					}
				}
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
