using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Draw;


namespace HamstarHelpers.Classes.UI.Elements.Slider {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedElement, IToggleable {
		/// <summary>
		/// Draws a slider bar.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="destRect"></param>
		/// <param name="valueAsPercent"></param>
		/// <param name="innerBarShader"></param>
		/// <param name="highlight">If `null`, highlight uses default behavior (mouse hover).</param>
		public static void DrawSlider(
					SpriteBatch sb,
					Rectangle destRect,
					float valueAsPercent,
					Func<float, Color> innerBarShader,
					bool? highlight = null ) {
			var outerBarRect = destRect;
			outerBarRect.Height = Main.colorBarTexture.Height;

			sb.Draw( Main.colorBarTexture, outerBarRect, Color.White );

			var innerBarRect = destRect;
			float scale = (float)destRect.Width / (UISlider.DefaultSliderWidth - UISlider.DefaultArrowsWidth);
			innerBarRect.X += (int)(4f * scale);
			innerBarRect.Y += 4;
			innerBarRect.Width -= (int)(8f * scale);
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

			if( highlight != false ) {
				if( highlight == true || destRect.Contains(Main.mouseX, Main.mouseY) ) {
					sb.Draw( Main.colorHighlightTexture, outerBarRect, Main.OurFavoriteColor );
				}
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
		protected override void DrawSelf( SpriteBatch sb ) {
			Rectangle fullRect = this.GetSliderRectangle();
			Rectangle leftRect = this.LeftArrowElem.GetInnerDimensions().ToRectangle();

			bool? highlight = null;

			if( leftRect.Contains(Main.mouseX, Main.mouseY) ) {
				highlight = false;
			}

			if( !highlight.HasValue ) {
				Rectangle rightRect = this.RightArrowElem.GetInnerDimensions().ToRectangle();
				if( rightRect.Contains(Main.mouseX, Main.mouseY) ) {
					highlight = false;
				}
			}

			Rectangle textRect = this.NumericInput.GetInnerDimensions().ToRectangle();
			textRect.X -= 12;
			textRect.Width += 12;

			if( !highlight.HasValue ) {
				if( textRect.Contains(Main.mouseX, Main.mouseY) ) {
					this.NumericInput.TextColor = Color.Yellow;
					highlight = false;
				}
			}

			if( highlight != false ) {
				this.NumericInput.TextColor = Color.White;
			}

			if( this.NumericInput.HasFocus ) {
				highlight = false;
			}

			float percentValue = UISlider.GetPercentOfSliderValue( this.RememberedInputValue, this.Range.Min, this.Range.Max );

			base.DrawSelf( sb );
			UISlider.DrawSlider( sb, fullRect, percentValue, this.InnerBarShader, highlight );

			if( textRect.Contains( Main.mouseX, Main.mouseY ) ) {
				if( !this.NumericInput.HasFocus ) {
					var inputLitRect = textRect;
					inputLitRect.Y += 6;
					inputLitRect.Height -= 6;

					DrawHelpers.DrawBorderedRect( sb, null, Color.Yellow * 0.5f, inputLitRect, 2 );
				}
			}
		}
	}
}
