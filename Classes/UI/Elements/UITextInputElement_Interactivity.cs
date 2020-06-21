using System;
using System.Text;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.UI;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a simpler append-only text field input element (no panel). Suited for main menu use.
	/// </summary>
	public partial class UITextInputElement : UIElement, IToggleable {
		private void UpdateInteractivity() {
			if( this.IsSelected ) {
				bool uiAvailable = UIHelpers.IsUIAvailable(
					mouseNotInUseElsewhere: true,
					noFullscreenMap: true
				);

				if( !this.IsInteractive || !uiAvailable ) {
					this.IsSelected = false;
					this.OnUnfocus?.Invoke();
					return;
				}
			}

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
					// Delay OnUnfocus (lets Update (Draw?) code finish, first)
					Timers.RunNow( () => { this.OnUnfocus?.Invoke(); } );
				}

				this.IsSelected = isNowSelected;
			}

			if( this.IsSelected ) {
				this.UpdateInput();
			}
		}


		private void UpdateInput() {
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
