using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;


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
		public float InputValue { get; protected set; } = 0f;


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

		/// <param name="theme">Appearance style.</param>
		/// <param name="hoverText">Mouse hover popup label.</param>
		/// <param name="isClickable">Enables mouse interactivity.</param>
		/// <param name="isInt">Indicates this slider uses integer values only. Default false.</param>
		/// <param name="ticks">Number of ticks to snap to along slider range. Default 0 (unlimited).</param>
		/// <param name="minRange">Beginning of slider range. Default 0.</param>
		/// <param name="maxRange">End of slider range. Default 1.</param>
		public UISlider( UITheme theme,
					string hoverText,
					bool isClickable = true,
					bool isInt = false,
					int ticks = 0,
					float minRange = 0f,
					float maxRange = 1f )
					: base( theme, true ) {
			this.HoverText = hoverText;
			this.IsClickable = isClickable;
			this.IsInt = isInt;
			this.Ticks = ticks;
			this.Range = (minRange, maxRange);

			this.Width.Set( 167f, 0f );
			this.Height.Set( 16f, 0f );
		}


		////////////////

		/// @private
		public override void Draw( SpriteBatch spriteBatch ) {
			if( this.IsClickable && Main.mouseLeft ) {
				if( UISlider.SelectedSlider == null ) {
					if( this.GetSliderRectangle().Contains(Main.mouseX, Main.mouseY) ) {
						UISlider.SelectedSlider = this;
					}
				}
			} else {
				UISlider.SelectedSlider = null;
			}

			base.Draw( spriteBatch );
		}

		/// @private
		protected override void DrawSelf( SpriteBatch sb ) {
			Rectangle destRect = this.GetSliderRectangle();

			if( this.IsClickable && Main.mouseLeft && UISlider.SelectedSlider == this ) {
				this.InputValue = UISlider.GetInputPercentWithinArea( new Point(Main.mouseX, Main.mouseY), destRect );
				this.InputValue = UISlider.GetValueOfSliderPositionPercent( this.InputValue, this.Range.Min, this.Range.Max, this.Ticks );

				if( this.IsInt ) {
					this.InputValue = (float)Math.Round( this.InputValue );
				}
			}

			float rangeAmt = this.Range.Max - this.Range.Min;
			float percentValue = (this.InputValue - this.Range.Min) / rangeAmt;

			UISlider.DrawSlider( sb, destRect, percentValue );

			base.DrawSelf( sb );
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
