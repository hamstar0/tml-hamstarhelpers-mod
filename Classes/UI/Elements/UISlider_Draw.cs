using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedPanel {
		/// <summary>
		/// Draws a slider bar and returns the value of the mouse's current position.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="destRect"></param>
		/// <param name="percentValue"></param>
		/// <param name="lockState"></param>
		public static void DrawSlider(
					SpriteBatch sb,
					Rectangle destRect,
					float percentValue,
					int lockState = 0 ) {
			var shiftedRect = destRect;

			sb.Draw( Main.colorBarTexture, destRect, Color.White );

			shiftedRect.X += 5;//* scale
			shiftedRect.Y += 4;//* scale
			shiftedRect.Width -= 10;

			for( int i = 0; i < shiftedRect.Width; i++ ) {
				float percent = (float)i / (float)destRect.Width;

				sb.Draw(
					texture: Main.colorBlipTexture,
					position: new Vector2( shiftedRect.X + i, shiftedRect.Y ),
					sourceRectangle: null,
					color: Utils.ColorLerp_BlackToWhite( percent ),
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
				sb.Draw( Main.colorHighlightTexture, destRect, Main.OurFavoriteColor );
			}

			sb.Draw(
				texture: Main.colorSliderTexture,
				position: new Vector2(
					shiftedRect.X + ((float)shiftedRect.Width * percentValue),
					shiftedRect.Y + 4f
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
	}
}
