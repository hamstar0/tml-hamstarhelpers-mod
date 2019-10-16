using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Helpers.XNA;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.Menus.Support {
	/// @private
	internal partial class SupportInfoDisplay {
		private static bool CanDraw() {
			if( !Main.gameMenu ) { return false; }
			if( Main.spriteBatch == null ) {
				return false;
			}

			var mymod = ModHelpersMod.Instance;
			if( mymod == null || ModHelpersMod.Config == null || Main.MenuUI == null ) {
				return false;
			}

			if( Main.MenuUI.CurrentState != null ) {
				Type uiType = Main.MenuUI.CurrentState.GetType();
				if( mymod.Data.ModTagsOpened ) {
					if( uiType.Name == "UIModInfo" || uiType.Name == "UIModBrowser" ) {
						return false;
					}
				}

				MenuUIDefinition menuDef;
				if( !Enum.TryParse( uiType.Name, out menuDef ) ) {
					return false;
				}
			}

			return true;
		}


		////////////////

		private static void _Draw( GameTime gt ) {
			try {
				if( !SupportInfoDisplay.CanDraw() ) {
					return;
				}

				var mymod = ModHelpersMod.Instance;

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
			bool isHoveringModTags = false;

			foreach( var elem in this.Elements ) {
				elem.Recalculate();
			}

			//var boxColor = new Color( 256, 0, 32 );
			//var boxEdgeColor = new Color( 255, 224, 224 );
			//float colorMul = 0.25f;
			float textColorMul = 1f;

			if( this.IsHoveringBox ) {
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

			//if( this.SupportUrl != null ) {
			//	this.SupportUrl.Theme.UrlColor = Color.Lerp( UITheme.Vanilla.UrlColor, AnimatedColors.Ether.CurrentColor, 0.25f );
			//	this.SupportUrl.Theme.UrlLitColor = Color.Lerp( UITheme.Vanilla.UrlLitColor, AnimatedColors.Strobe.CurrentColor, 0.5f );
			//	this.SupportUrl.Theme.UrlLitColor = Color.Lerp( this.SupportUrl.Theme.UrlLitColor, AnimatedColors.DiscoFast.CurrentColor, 0.75f );
			//	this.SupportUrl.RefreshTheme();
			//}

			if( this.EnableModTagsLabel != null ) {
				isHoveringModTags = this.EnableModTagsLabel.GetOuterDimensions()
					.ToRectangle()
					.Contains( Main.mouseX, Main.mouseY );

				if( !isHoveringModTags ) {
					this.EnableModTagsLabel.TextColor = AnimatedColors.Ember.CurrentColor;
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

			if( isHoveringModTags && this.EnableModTagsLabel != null ) {
				this.DrawModTagsNewOverlay();
			}
			
			Vector2 bonus = Main.DrawThickCursor( false );
			Main.DrawCursor( bonus, false );
		}


		private void DrawModTagsNewOverlay() {
			float anim = this.ModTagsNewOverlayAnim < 0.5f ?
				this.ModTagsNewOverlayAnim * 2f :
				1f - ((this.ModTagsNewOverlayAnim - 0.5f) * 2f);

			this.ModTagsNewOverlayAnim += 1f / 60f;
			if( this.ModTagsNewOverlayAnim >= 1 ) {
				this.ModTagsNewOverlayAnim = 0;
			}

			Vector2 pos = this.EnableModTagsLabel.GetOuterDimensions().Position();
			pos.Y += 8f;
			Vector2 origin = Main.fontMouseText.MeasureString( "NEW!" ) * 0.5f;
			Color color = AnimatedColors.Alert.CurrentColor;
			float rot = -MathHelper.Lerp( 0.26f, 0.16f, anim );
			float scale = 1.2f;

			Main.spriteBatch.DrawString( Main.fontMouseText, "NEW!", pos+new Vector2(1,0), Color.Black, rot, origin, scale, SpriteEffects.None, 1f );
			Main.spriteBatch.DrawString( Main.fontMouseText, "NEW!", pos+new Vector2(0,1), Color.Black, rot, origin, scale, SpriteEffects.None, 1f );
			Main.spriteBatch.DrawString( Main.fontMouseText, "NEW!", pos+new Vector2(-1,0), Color.Black, rot, origin, scale, SpriteEffects.None, 1f );
			Main.spriteBatch.DrawString( Main.fontMouseText, "NEW!", pos+new Vector2(0,-1), Color.Black, rot, origin, scale, SpriteEffects.None, 1f );
			Main.spriteBatch.DrawString( Main.fontMouseText, "NEW!", pos, color, rot, origin, scale, SpriteEffects.None, 1f );
		}
	}
}
