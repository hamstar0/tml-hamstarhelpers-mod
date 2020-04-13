using System;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedPanel {
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

		private UITextInputAreaPanel NumericInput;

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
			this.Height.Set( 40f, 0f );

			this.SetValue( minRange );

			this.NumericInput = new UITextInputAreaPanel( this.Theme, "0", 24 );
			this.NumericInput.Width.Set( 96f, 0f );
			this.NumericInput.Height.Set( 16f, 0f );
			this.NumericInput.Top.Set( 4f, 0f );
			this.NumericInput.Left.Set( 4f, 0f );
			this.NumericInput.OnPreTextChange += ProcessInput;
			//this.Append( this.NumericInput );
		}


		////////////////

		/// <summary>
		/// Enables or disables the current element.
		/// </summary>
		/// <param name="isEnabled"></param>
		public void Enable( bool isEnabled ) {
			this.IsClickable = isEnabled;
		}


		////////////////

		private bool SetValueFromInput( string rawInput ) {
			float input;
			if( !float.TryParse( rawInput, out input ) ) {
				return false;
			}

			this.SetValue( input );
			return true;
		}

		/// <summary>
		/// Sets the value. Applies necessary clamping.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue( float value ) {
			this.IsNowSettingValue = true;

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
				value = MathHelper.Clamp( (int)value, (int)Math.Ceiling(this.Range.Min), (int)Math.Floor(this.Range.Max) );
			} else {
				value = MathHelper.Clamp( value, this.Range.Min, this.Range.Max );
			}

			this.RememberedInputValue = value;

			if( this.IsInt ) {
				this.NumericInput?.SetText( value.ToString("N0") );
			} else {
				this.NumericInput?.SetText( value.ToString() );
			}

			this.IsNowSettingValue = false;
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			this.UpdateMouseInteractivity();
		}

		private void UpdateMouseInteractivity() {
			if( !this.IsClickable ) {
				return;
			}
			if( !Main.mouseLeft ) {
				return;
			}
			if( UISlider.SelectedSlider != null ) {
				return;
			}

			Rectangle rect = this.GetInnerDimensions().ToRectangle();
			if( !rect.Contains( Main.mouseX, Main.mouseY ) ) {
				return;
			}

			UISlider.SelectedSlider = this;

			Timers.RunUntil( () => {
				if( !this.UpdateSliderMouseDrag(rect) ) {
					UISlider.SelectedSlider = null;
					return false;
				}
				return true;
			}, true );
		}

		private bool UpdateSliderMouseDrag( Rectangle sliderArea ) {
			if( !this.IsClickable ) {
				return false;
			}
			if( !Main.mouseLeft ) {
				return false;
			}

			if( UISlider.SelectedSlider == this ) {
				float value = UISlider.GetInputValue(
					sliderArea,
					new Point( Main.mouseX, Main.mouseY ),
					this.Range.Min,
					this.Range.Max,
					this.Ticks,
					this.IsInt
				);
				this.SetValue( value );
			}

			return true;
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
