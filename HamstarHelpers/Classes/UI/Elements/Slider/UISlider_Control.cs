using System;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Classes.UI.Elements.Slider {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedElement, IToggleable {
		/// <summary>
		/// Enables or disables the current element.
		/// </summary>
		/// <param name="isEnabled"></param>
		public void Enable( bool isEnabled ) {
			this.IsClickable = isEnabled;

			this.NumericInput.Enable( isEnabled && !this.IsTextInputHidden );
			this.NumericInput.Hide( !isEnabled || this.IsTextInputHidden );
		}

		////

		/// <summary>
		/// Adjusts the available range of values for this element.
		/// </summary>
		/// <param name="minRange"></param>
		/// <param name="maxRange"></param>
		public void SetRange( float minRange, float maxRange ) {
			this.Range = (minRange, maxRange);

			this.SetValueUnsafe( this.RememberedInputValue );
		}

		/// <summary>
		/// Adjusts the number of ticks to use for this element.
		/// </summary>
		/// <param name="ticks">Set to 0 for no ticks.</param>
		public void SetTicks( int ticks ) {
			this.Ticks = ticks;

			this.SetValueUnsafe( this.RememberedInputValue );
		}


		////////////////

		private bool SetValueFromInput( string rawInput ) {
			float value;
			if( !float.TryParse(rawInput, out value) ) {
				return true;
			}

			float processedValue = this.GetConstrainedValue( value );
			if( processedValue != value ) {
				return false;
			}

			if( value != this.RememberedInputValue && this.PreOnChange != null ) {
				float? newval = this.PreOnChange.Invoke(value);
				if( !newval.HasValue ) {
					return false;
				} else {
					value = newval.Value;
				}
			}

			this.SetValueUnsafe( value );

			return true;
		}

		/// <summary>
		/// Sets the value. Applies necessary clamping.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue( float value ) {
			if( this.PreOnChange != null ) {
				float? newval = this.PreOnChange?.Invoke( value );
				if( !newval.HasValue ) {
					return;
				} else {
					value = newval.Value;
				}
			}

			this.IsNowSettingValue = true;
			this.SetValueUnsafe( value );
			this.IsNowSettingValue = false;
		}

		////

		private void SetValueUnsafe( float value ) {
			this.RememberedInputValue = this.GetConstrainedValue( value );

			if( this.IsInt ) {
				this.NumericInput?.SetText( this.RememberedInputValue.ToString("N0") );
			} else {
				this.NumericInput?.SetText( this.RememberedInputValue.ToString() );
			}
		}


		////////////////

		/// <summary>
		/// Scrolls the slider 1 "unit" left.
		/// </summary>
		public void ScrollLeft() {
			if( UISlider.SelectedSlider != null ) {
				return;
			}

			float range = this.Range.Max - this.Range.Min;
			float unit;

			if( this.Ticks == 0 ) {
				unit = this.IsInt ? 1 : range * 0.01f;
			} else {
				unit = range / (float)this.Ticks;
			}

			this.SetValue( this.RememberedInputValue - unit );
		}

		/// <summary>
		/// Scrolls the slider 1 "unit" right.
		/// </summary>
		public void ScrollRight() {
			if( UISlider.SelectedSlider != null ) {
				return;
			}

			float range = this.Range.Max - this.Range.Min;
			float unit;

			if( this.Ticks == 0 ) {
				unit = this.IsInt ? 1 : range * 0.01f;
			} else {
				unit = range / (float)this.Ticks;
			}

			this.SetValue( this.RememberedInputValue + unit );
		}
	}
}
