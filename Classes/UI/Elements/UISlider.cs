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
		/// <summary>
		/// Gets the slider value of a given point relative to a given slider rectangle area.
		/// </summary>
		/// <param name="point"></param>
		/// <param name="area"></param>
		/// <returns></returns>
		public static float GetInputValueWithinArea( Point point, Rectangle area ) {
			if( point.X >= area.X && point.X <= area.X + area.Width ) {
				return (float)( point.X - area.X ) / (float)area.Width;
			}

			if( area.X >= point.X ) {
				return 0f;
			}

			return 1f;
		}



		////////////////

		/// <summary>
		/// Element's current input value.
		/// </summary>
		public float InputPercentValue = 0f;

		////

		/// <summary>
		/// Allows defining a custom sort order value (for putting in an ordered list).
		/// </summary>
		public float Order = 0f;

		/// <summary>
		/// Enables mouse interactivity.
		/// </summary>
		public bool IsClickable = true;

		/// <summary>
		/// Mouse hover popup label.
		/// </summary>
		public string Title = "";



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="hoverText">Mouse hover popup label.</param>
		/// <param name="isClickable">Enables mouse interactivity.</param>
		public UISlider( UITheme theme,
				string hoverText,
				bool isClickable = true )
				: base( theme, true ) {
			this.Title = hoverText;
			this.IsClickable = isClickable;

			this.Width.Set( 167f, 0f );
			this.Height.Set( 16f, 0f );
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


		////////////////

		/// @private
		protected override void DrawSelf( SpriteBatch sb ) {
			Rectangle destRect = this.GetSliderRectangle();

			this.InputPercentValue = UISlider.GetInputValueWithinArea( new Point(Main.mouseX, Main.mouseY), destRect );

			UISlider.DrawSlider( sb, destRect, this.InputPercentValue );

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
