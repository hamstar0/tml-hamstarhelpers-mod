using System;
using Microsoft.Xna.Framework;
using Terraria;
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
		public static float GetValueOfSliderPercent( float percent, float minRange, float maxRange, int ticks ) {
			float rangeAmt = maxRange - minRange;
			float rangeValue = rangeAmt * percent;

			if( ticks > 0 ) {
				float tickRange = rangeAmt / ticks;
				int tickAmt = (int)( rangeValue / tickRange );
				rangeValue = (float)tickAmt * tickRange;
			}

			return minRange + rangeValue;
		}

		/// <summary>
		/// Gets the slider percent value of a given input value.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="minRange"></param>
		/// <param name="maxRange"></param>
		/// <returns></returns>
		public static float GetPercentOfSliderValue( float value, float minRange, float maxRange ) {
			float rangeAmt = maxRange - minRange;
			return ( value - minRange ) / rangeAmt;
		}


		////////////////

		/// <summary>
		/// Gets the input value from a given screen selection point.
		/// </summary>
		/// <param name="sliderRect"></param>
		/// <param name="screenAt"></param>
		/// <param name="minRange"></param>
		/// <param name="maxRange"></param>
		/// <param name="ticks"></param>
		/// <param name="isInt"></param>
		/// <returns></returns>
		public static float GetInputValue( Rectangle sliderRect, Point screenAt, float minRange, float maxRange, int ticks, bool isInt ) {
			float valuePercent = UISlider.GetInputPercentWithinArea( screenAt, sliderRect );
			float value = UISlider.GetValueOfSliderPercent( valuePercent, minRange, maxRange, ticks );
			if( isInt ) {
				value = (float)Math.Round( value );
			}
			return value;
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
