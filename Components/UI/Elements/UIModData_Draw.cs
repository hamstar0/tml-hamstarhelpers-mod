using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	public partial class UIModData : UIPanel {
		public static Color GetTagColor( string tag ) {
			switch( tag ) {
			case "May Lag":
				return Color.Yellow;
			case "Non-functional":
			case "Cheat-like":
				return Color.Red;
			case "Misleading Info":
			case "Buggy":
				return Color.Purple;
			case "Unmaintained":
			case "Unfinished":
				return Color.SlateGray;
			default:
				return Color.Silver;
			}
		}



		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.IsMouseHovering && this.WillDrawOwnHoverElements ) {
				this.DrawHoverEffects( sb );
			}
		}
		
		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			CalculatedStyle innerDim = base.GetInnerDimensions();
			Vector2 innerPos = innerDim.Position();

			if( this.LatestAvailableVersion > this.Mod.Version ) {
				Color color = AnimatedColors.Fire.CurrentColor;
				var pos = new Vector2( innerPos.X + 128f, innerPos.Y );
			
				sb.DrawString( Main.fontDeathText, this.LatestAvailableVersion.ToString()+" Available", pos, color, 0f, default( Vector2 ), 1f, SpriteEffects.None, 1f );
			}

			if( this.ModTags.Count > 0 ) {
				var startPos = new Vector2( innerPos.X, innerPos.Y + 56 );
				var pos = startPos;

				int i = 0;
				foreach( string tag in this.ModTags ) {
					string tagStr = tag + ((i++ < this.ModTags.Count-1) ? "," : "");
					Color tagColor = UIModData.GetTagColor( tag );

					Vector2 dim = Main.fontMouseText.MeasureString( tag ) * 0.65f;
					float addX = dim.X + 8;

					if( ( (pos.X + addX) - innerDim.X ) > innerDim.Width ) {
						pos.X = startPos.X;
						pos.Y += 12;
					}

					Utils.DrawBorderString( sb, tagStr, pos, tagColor, 0.65f );
					
					pos.X += addX;
				}
			}
		}


		public void DrawHoverEffects( SpriteBatch sb ) {
			if( this.TitleElem.IsMouseHovering ) {
				if( this.TitleElem is UIWebUrl ) {
					var titleUrl = (UIWebUrl)this.TitleElem;

					if( !titleUrl.WillDrawOwnHoverUrl ) {
						titleUrl.DrawHoverEffects( sb );
					}
				}
			}
		}
	}
}
