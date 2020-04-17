using System;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Implements a UI slider bar element.
	/// </summary>
	public partial class UISlider : UIThemedElement {
		private void InitializeMe() {
			bool ProcessInput( StringBuilder fullInput ) {
				return !this.IsNowSettingValue
					&& this.SetValueFromInput( fullInput.ToString() );
			}

			//

			this.Width.Set( 167f, 0f );
			this.Height.Set( 24f, 0f );

			this.NumericInput = new UITextInputElement( "" );
			this.NumericInput.Enable( !this.IsTextInputHidden );
			this.NumericInput.Hide( this.IsTextInputHidden );
			this.NumericInput.Top.Set( -2f, 0f );
			this.NumericInput.Left.Set( 20f, 0f );
			this.NumericInput.Width.Set( 64f, 0f );
			this.NumericInput.Height.Set( 24f, 0f );
			this.NumericInput.OnTextChange += ProcessInput;
			this.Append( this.NumericInput );

			var leftArrowElem = new UIThemedText( this.Theme, true, " < " );
			leftArrowElem.Height.Set( 24f, 0f );
			leftArrowElem.OnMouseOver += ( _, __ ) => leftArrowElem.TextColor = Color.Yellow;
			leftArrowElem.OnMouseOut += ( _, __ ) => leftArrowElem.TextColor = Color.White;
			leftArrowElem.OnClick += ( _, __ ) => this.ScrollLeft();
			this.Append( (UIElement)leftArrowElem );

			var rightArrowElem = new UIThemedText( this.Theme, true, "  > " );
			rightArrowElem.Left.Set( -30f, 1f );
			rightArrowElem.Height.Set( 24f, 0f );
			rightArrowElem.OnMouseOver += ( _, __ ) => rightArrowElem.TextColor = Color.Yellow;
			rightArrowElem.OnMouseOut += ( _, __ ) => rightArrowElem.TextColor = Color.White;
			rightArrowElem.OnClick += ( _, __ ) => this.ScrollRight();
			this.Append( (UIElement)rightArrowElem );
		}
	}
}
