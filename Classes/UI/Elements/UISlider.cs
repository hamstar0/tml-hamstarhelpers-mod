﻿using System;
using System.Text;
using Microsoft.Xna.Framework;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Classes.UI.Elements {
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
		/// Mouse hover popup label.
		/// </summary>
		public string HoverText { get; protected set; } = "";


		////////////////

		private UITextInputElement NumericInput;

		private bool IsNowSettingValue = false;



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="hoverText">Mouse hover popup label.</param>
		/// <param name="isInt">Indicates this slider uses integer values only. Default false.</param>
		/// <param name="ticks">Number of ticks to snap to along slider range. Default 0 (unlimited).</param>
		/// <param name="minRange">Beginning of slider range. Default 0.</param>
		/// <param name="maxRange">End of slider range. Default 1.</param>
		public UISlider( UITheme theme,
					string hoverText,
					bool isInt = false,
					int ticks = 0,
					float minRange = 0f,
					float maxRange = 1f )
					: base( theme, true ) {
			bool ProcessInput( StringBuilder fullInput ) {
				return !this.IsNowSettingValue
					&& this.SetValueFromInput( fullInput.ToString() );
			}

			//

			this.HoverText = hoverText;
			this.IsInt = isInt;
			this.Ticks = ticks;
			this.Range = (minRange, maxRange);

			this.Width.Set( 167f, 0f );
			this.Height.Set( 24f, 0f );

			this.NumericInput = new UITextInputElement( "" );
			this.NumericInput.Top.Set( -2f, 0f );
			this.NumericInput.Left.Set( 6f, 0f );
			this.NumericInput.Width.Set( 64f, 0f );
			this.NumericInput.Height.Set( 24f, 0f );
			this.NumericInput.OnTextChange += ProcessInput;
			this.Append( this.NumericInput );

			this.SetValue( minRange );
		}


		////////////////

		/// <summary>
		/// Enables or disables the current element.
		/// </summary>
		/// <param name="isEnabled"></param>
		public void Enable( bool isEnabled ) {
			this.IsClickable = isEnabled;
			this.NumericInput.Enable( isEnabled );
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

		private void SetValueUnsafe( float value ) {
			this.RememberedInputValue = this.GetConstrainedValue( value );

			if( this.IsInt ) {
				this.NumericInput?.SetText( value.ToString( "N0" ) );
			} else {
				this.NumericInput?.SetText( value.ToString() );
			}
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
