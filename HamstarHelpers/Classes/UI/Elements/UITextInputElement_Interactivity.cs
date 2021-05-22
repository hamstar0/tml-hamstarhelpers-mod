using System;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.UI;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a simpler append-only text field input element (no panel). Suited for main menu use.
	/// </summary>
	public partial class UITextInputElement : UIElement, IToggleable {
		/// <summary>
		/// Sets input to be captured by the current element.
		/// </summary>
		/// <returns>`true` if able to capture focus.</returns>
		public bool Focus() {
			if( !this.IsInteractive ) { return false; }
			if( this.HasFocus ) { return false; }
			this.HasFocus = true;

			this.CursorAnimation = 0;

			Main.blockInput = true;
			Main.clrInput();

			return true;
		}

		/// <summary>
		/// Removes input capture.
		/// </summary>
		/// <returns></returns>
		public bool Unfocus() {
			if( !this.HasFocus ) { return false; }
			this.HasFocus = false;

			Main.blockInput = false;

			this.OnUnfocus?.Invoke();

			return true;
		}


		////////////////

		private void UpdateInteractivity() {
			if( this.HasFocus ) {
				// Hackish:
				if( Main.drawingPlayerChat ) {
					Main.drawingPlayerChat = false;
					Main.chatText = "";
					Main.chatRelease = false;
				}
				if( !this.IsInteractive || !UILibraries.IsUIAvailable(keyboardNotInVanillaUI: true) ) {
					this.Unfocus();
					return;
				}
				if( UILibraries.JustPressedKey(Keys.Escape) || UILibraries.JustPressedKey(Keys.Enter) ) {
					this.Unfocus();
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

				if( this.HasFocus && !isNowSelected ) {
					// Delay Unfocus (lets Update (Draw?) code finish, first)
					Timers.RunNow( () => { this.Unfocus(); } );
					return;
				}

				this.HasFocus = isNowSelected;
			}

			if( this.HasFocus ) {
				this.UpdateInput();
			}
		}


		private void UpdateInput() {
			Main.blockInput = true; // Force the point!

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
