using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Classes.UI.Elements.Slider {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedElement {
		/// @private
		public override void Update( GameTime gameTime ) {
			if( !this.NumericInput.IsMouseHovering && !this.NumericInput.IsSelected ) {
				this.UpdateMouseInteractivity();
			}

			if( UISlider.SelectedSlider != null ) {
				this.NumericInput.Enable( false );
			} else {
				this.NumericInput.Enable( this.IsClickable );
			}
		}


		////

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

			Rectangle outerRect = this.GetSliderRectangle();
			Rectangle innerRect = UISlider.GetInnerSliderRectangle( outerRect );
			if( !innerRect.Contains( Main.mouseX, Main.mouseY ) ) {
				return;
			}

			if( this.LeftArrowElem.GetOuterDimensions().ToRectangle().Contains(Main.mouseX, Main.mouseY) ) {
				return;
			}
			if( this.RightArrowElem.GetOuterDimensions().ToRectangle().Contains(Main.mouseX, Main.mouseY) ) {
				return;
			}
			if( this.NumericInput.GetOuterDimensions().ToRectangle().Contains(Main.mouseX, Main.mouseY) ) {
				return;
			}

			UISlider.SelectedSlider = this;

			Timers.RunUntil( () => {
				if( !this.UpdateSliderMouseDrag(innerRect) ) {
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
					new Point(
						(int)((float)Main.mouseX / Main.UIScale),
						(int)((float)Main.mouseY / Main.UIScale)
					),	//UIHelpers.ConvertToScreenPosition( Main.MouseWorld ).ToPoint(),
					this.Range.Min,
					this.Range.Max,
					this.Ticks,
					this.IsInt
				);

				this.SetValue( value );
			}

			return true;
		}
	}
}
