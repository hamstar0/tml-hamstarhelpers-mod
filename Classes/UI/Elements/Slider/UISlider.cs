using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Classes.UI.Elements.Slider {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedElement {
		private static UISlider SelectedSlider = null;



		////////////////

		/// <summary>
		/// Element's current input value.
		/// </summary>
		public float RememberedInputValue { get; protected set; } = 0f;


		////

		/// <summary>
		/// Total range of this slider element.
		/// </summary>
		public (float Min, float Max) Range { get; protected set; } = (0f, 1f);

		/// <summary>
		/// Number of ticks to snap to along slider's range.
		/// </summary>
		public int Ticks { get; protected set; } = 0;

		/// <summary>
		/// Constrain values to integers.
		/// </summary>
		public bool IsInt { get; protected set; } = true;


		////

		/// <summary>
		/// Allows defining a custom sort order value (for putting in an ordered list).
		/// </summary>
		public float Order { get; protected set; } = 0f;

		/// <summary>
		/// Enables mouse interactivity.
		/// </summary>
		public bool IsClickable { get; protected set; } = true;

		/// <summary>
		/// Indicates if text input is suppressed (hidden).
		/// </summary>
		public bool IsTextInputHidden { get; protected set; } = false;

		/// <summary>
		/// Mouse hover popup label.
		/// </summary>
		public string HoverText { get; protected set; } = "";

		/// <summary>
		/// Mouse hover popup label.
		/// </summary>
		public Func<float, Color> InnerBarShader { get; protected set; }


		////////////////

		private UITextInputElement NumericInput;
		private UIThemedText LeftArrowElem;
		private UIThemedText RightArrowElem;

		private bool IsNowSettingValue = false;



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="hoverText">Mouse hover popup label.</param>
		/// <param name="isInt">Indicates this slider uses integer values only. Default false.</param>
		/// <param name="ticks">Number of ticks to snap to along slider range. Default 0 (unlimited).</param>
		/// <param name="minRange">Beginning of slider range. Default 0.</param>
		/// <param name="maxRange">End of slider range. Default 1.</param>
		/// <param name="hideTextInput">Indicates if text input is suppressed (hidden).</param>
		/// <param name="innerBarShader">Allows specifying a color gradient for the inner bar. Defaults to `null`; the default black-to-white shader.</param>
		public UISlider( UITheme theme,
					string hoverText,
					bool isInt = false,
					int ticks = 0,
					float minRange = 0f,
					float maxRange = 1f,
					bool hideTextInput = false,
					Func<float, Color> innerBarShader = null )
					: base( theme, true ) {
			this.HoverText = hoverText;
			this.IsInt = isInt;
			this.Ticks = ticks;
			this.Range = (minRange, maxRange);
			this.IsTextInputHidden = hideTextInput;
			this.InnerBarShader = innerBarShader == null
				? Utils.ColorLerp_BlackToWhite
				: innerBarShader;

			this.InitializeMe();

			this.SetValue( minRange );
		}


		////////////////

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

			this.SetValueUnsafe( value );

			return true;
		}

		/// <summary>
		/// Sets the value. Applies necessary clamping.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue( float value ) {
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


		////////////////

		/// <summary>
		/// Decides sort order in a list.
		/// </summary>
		/// <param name="obj">Object to compare rank to.</param>
		/// <returns>Value representing greater-than or less-than sortion status relative to the given comparison object.</returns>
		public override int CompareTo( object obj ) {
			try {
				UICheckbox other = obj as UICheckbox;
				return this.Order.CompareTo( other.Order );
			} catch( Exception ) {
				return 0;
			}
		}
	}
}
