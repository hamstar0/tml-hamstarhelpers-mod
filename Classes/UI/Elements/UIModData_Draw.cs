using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI panel element specialized for rendering and displaying a mod's data (especially as a list item).
	/// </summary>
	public partial class UIModData : UIThemedPanel {
		/// <summary>
		/// Matches a color with a given mod tag. Used to represent tags that should be emphasized.
		/// </summary>
		/// <param name="tag">Mod tag</param>
		/// <returns>Color of tag.</returns>
		public static Color GetTagColor( string tag ) {
			switch( tag ) {
			// Important tags:
			case "MP Compatible":
				return Color.Blue;
			case "Needs New World":
			case "Needs New Player":
				return Color.SkyBlue;
			// Negative tags:
			case "May Lag":
			case "Cheat-like":
				return Color.Yellow;
			case "Non-functional":
				return Color.Red;
			case "Misleading Info":
			case "Buggy":
				return Color.Purple;
			case "Unimaginative":
			case "Low Effort":
			case "Unoriginal Content":
				return Color.Tomato;
			case "Unmaintained":
			case "Unfinished":
				return Color.SlateGray;
			default:
				return Color.Silver;
			}
		}



		////////////////

		/// <summary>
		/// Expands on base.Draw(...) to implement custom mouse hover effects (if applicable).
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically given `Main.spriteBatch`.</param>
		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.IsMouseHovering && this.WillDrawOwnHoverElements ) {
				this.DrawHoverEffects( sb );
			}
		}

		/// <summary>
		/// Expands on base.Draw to add version overlay and render mod tags.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically given `Main.spriteBatch`.</param>
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

				this.Height.Set( 64 + 12, 0f );

				int i = 0;
				foreach( string tag in this.ModTags ) {
					string tagStr = tag + ((i++ < this.ModTags.Count-1) ? "," : "");
					Color tagColor = UIModData.GetTagColor( tag );

					Vector2 dim = Main.fontMouseText.MeasureString( tag ) * 0.75f;
					float addX = dim.X + 8;

					if( ((pos.X + addX) - innerDim.X) > innerDim.Width ) {
						pos.X = startPos.X;
						pos.Y += 12;

						this.Height.Set( this.Height.Pixels + 12, 0f );
					}

					Utils.DrawBorderString( sb, tagStr, pos, tagColor, 0.75f );
					
					pos.X += addX;
				}
			}
		}


		/// <summary>
		/// Draws mouse hover title.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically given `Main.spriteBatch`.</param>
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
