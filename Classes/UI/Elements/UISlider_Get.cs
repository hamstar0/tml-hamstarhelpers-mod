using System;
using Microsoft.Xna.Framework;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedPanel {
		/// <summary>
		/// Gets the slider value of a given point relative to a given slider rectangle area.
		/// </summary>
		/// <param name="point"></param>
		/// <param name="area"></param>
		/// <returns></returns>
		public static float GetInputPercentWithinArea( Point point, Rectangle area ) {
			if( point.X >= area.X && point.X <= area.X + area.Width ) {
				return (float)( point.X - area.X ) / (float)area.Width;
			}

			if( area.X >= point.X ) {
				return 0f;
			}

			return 1f;
		}

		/// <summary>
		/// Gets the actual allowed value of a given slider percent position.
		/// </summary>
		/// <param name="percent"></param>
		/// <param name="minRange"></param>
		/// <param name="maxRange"></param>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static float GetValueOfSliderPositionPercent( float percent, float minRange, float maxRange, int ticks ) {
			float rangeAmt = maxRange - minRange;
			float rangeValue = rangeAmt * percent;

			if( ticks > 0 ) {
				float tickRange = rangeAmt / ticks;
				int tickAmt = (int)( rangeValue / tickRange );
				rangeValue = (float)tickAmt * tickRange;
			}

			return minRange + rangeValue;
		}



		////////////////

		/// <summary>
		/// Gets the screen space rectangle of the slider.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetSliderRectangle() {
			CalculatedStyle innerPos = this.GetInnerDimensions();

			return new Rectangle(
				(int)innerPos.X,
				((int)innerPos.Y - 5) - ((int)innerPos.Height / 2 ),//* scale
				(int)innerPos.Width,//* scale
				(int)innerPos.Height//* scale
			);
		}
	}
}
