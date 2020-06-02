using System;
using Microsoft.Xna.Framework;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements.Slider {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedElement, IToggleable {
		/// <summary>
		/// Gets the inner rectangle of a slider's rectangle (where the slider knob is interacted with).
		/// </summary>
		/// <returns></returns>
		public static Rectangle GetInnerSliderRectangle( Rectangle outerSliderRect ) {
			Rectangle rect = outerSliderRect;

			float scale = (float)rect.Width / (UISlider.DefaultSliderWidth - UISlider.DefaultArrowsWidth);
			rect.X += (int)( 4f * scale );
			rect.Y += 4;
			rect.Width -= (int)( 8f * scale );
			rect.Height -= 4;

			return rect;
		}

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
				float rangePerTick = rangeAmt / ticks;
				int rangeValueTickCount = (int)( rangeValue / rangePerTick );
				rangeValue = (float)rangeValueTickCount * rangePerTick;
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
		public static float GetInputValue(
					Rectangle sliderRect,
					Point screenAt,
					float minRange,
					float maxRange,
					int ticks,
					bool isInt ) {
			float valuePercent = UISlider.GetInputPercentWithinArea( screenAt, sliderRect );
			float value = UISlider.GetValueOfSliderPercent( valuePercent, minRange, maxRange, ticks );
			if( isInt ) {
				value = (float)Math.Round( value );
			}
			return value;
		}



		////////////////

		/// <summary>
		/// Processes a given value to best fit as a value of the current slider's available set.
		/// </summary>
		/// <param name="rawValue"></param>
		/// <returns></returns>
		public float GetConstrainedValue( float rawValue ) {
			float value = rawValue;

			if( this.Ticks > 0 ) {
				float rangeAmt = this.Range.Max - this.Range.Min;
				float rangeValue = value - this.Range.Min;

				float rangePerTick = rangeAmt / this.Ticks;
				int rangeValueTickCount = (int)( rangeValue / rangePerTick );
				rangeValue = (float)rangeValueTickCount * rangePerTick;

				value = this.Range.Min + rangeValue;
			}

			if( this.IsInt ) {
				value = (float)Math.Round( value );
				value = MathHelper.Clamp( (int)value, (int)Math.Ceiling( this.Range.Min ), (int)Math.Floor( this.Range.Max ) );
			} else {
				value = MathHelper.Clamp( value, this.Range.Min, this.Range.Max );
			}

			return value;
		}


		////////////////

		/// <summary>
		/// Gets the screen rectangle of the inner slider. Does not include arrows.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetSliderRectangle() {
			Rectangle rect = this.GetInnerDimensions().ToRectangle();
			rect.Width -= (int)UISlider.DefaultArrowsWidth;
			return rect;
		}
	}
}
