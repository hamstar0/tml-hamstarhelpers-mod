using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedElement {
		/// <summary>
		/// Draws a slider bar and returns the value of the mouse's current position.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="destRect"></param>
		/// <param name="valueAsPercent"></param>
		/// <param name="innerBarShader"></param>
		/// <param name="lockState"></param>
		public static void DrawSlider(
					SpriteBatch sb,
					Rectangle destRect,
					float valueAsPercent,
					Func<float, Color> innerBarShader,
					int lockState = 0 ) {
			var innerBarRect = destRect;
			var outerBarRect = destRect;
			outerBarRect.Height = Main.colorBarTexture.Height;

			sb.Draw( Main.colorBarTexture, outerBarRect, Color.White );

			innerBarRect.X += 4;//* scale
			innerBarRect.Y += 4;//* scale
			innerBarRect.Width -= 8;
			innerBarRect.Height -= 4;

			for( int i = 0; i < innerBarRect.Width; i++ ) {
				float percent = (float)i / (float)destRect.Width;

				sb.Draw(
					texture: Main.colorBlipTexture,
					position: new Vector2( innerBarRect.X + i, innerBarRect.Y ),
					sourceRectangle: null,
					color: innerBarShader( percent ),
					rotation: 0f,
					origin: Vector2.Zero,
					scale: 1f,
					effects: SpriteEffects.None,
					layerDepth: 0f
				);
			}

			bool isHovering = false;
			if( lockState != 2 ) {
				isHovering = destRect.Contains( new Point(Main.mouseX, Main.mouseY) );
			}

			if( isHovering || lockState == 1 ) {
				sb.Draw( Main.colorHighlightTexture, outerBarRect, Main.OurFavoriteColor );
			}

			sb.Draw(
				texture: Main.colorSliderTexture,
				position: new Vector2(
					innerBarRect.X + ((float)innerBarRect.Width * valueAsPercent),
					innerBarRect.Y + 4f
				),
				sourceRectangle: null,
				color: Color.White,
				rotation: 0f,
				origin: new Vector2( Main.colorSliderTexture.Width, Main.colorSliderTexture.Height ) * 0.5f,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}



		////////////////

		/// @private
		public override void Draw( SpriteBatch spriteBatch ) {
			base.Draw( spriteBatch );
		}

		/// @private
		protected override void DrawSelf( SpriteBatch sb ) {
			Rectangle rect = this.GetInnerDimensions().ToRectangle();
			float percentValue = UISlider.GetPercentOfSliderValue( this.RememberedInputValue, this.Range.Min, this.Range.Max );

			base.DrawSelf( sb );

			UISlider.DrawSlider( sb, rect, percentValue, this.InnerBarShader );
		}
	}
}
