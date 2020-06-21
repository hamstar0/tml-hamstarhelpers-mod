using System;
using System.Text;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a simpler append-only text field input element (no panel). Not theme-able. Suited for main menu use.
	/// </summary>
	public partial class UITextInputElement : UIElement, IToggleable {
		private void UpdateInteractivity() {
			CalculatedStyle dim = this.GetDimensions();

			// Detect if user selects this element
			if( Main.mouseLeft ) {
				bool isNowSelected = false;

				if( Main.mouseX >= dim.X && Main.mouseX < (dim.X + dim.Width) ) {
					if( Main.mouseY >= dim.Y && Main.mouseY < (dim.Y + dim.Height) ) {
						isNowSelected = true;
						Main.keyCount = 0;
					}
				}

				if( this.IsSelected && !isNowSelected ) {
					Timers.RunNow( () => { this.OnUnfocus?.Invoke(); } );
				}
				this.IsSelected = isNowSelected;
			}

			// Apply text inputs
			if( this.IsSelected ) {
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newStr = Main.GetInputText( this.Text );

				if( !newStr.Equals( this.Text ) ) {
					var newStrMuta = new StringBuilder( newStr );

					Timers.RunNow( () => {
						if( this.OnTextChange?.Invoke( newStrMuta ) ?? true ) {
							this.Text = newStrMuta.ToString();
						}
					} );
				}
			}
		}
	}
}
