using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a text area UI panel element with crop-to-fit text input. Captures focus while in use. Does not currently implement
	/// multi-line support (yet).
	/// </summary>
	public partial class UITextInputAreaPanel : UIThemedPanel, IToggleable {
		/// <summary>
		/// Implements click behavior. Focuses on the input element.
		/// </summary>
		/// <param name="evt">Mouse event.</param>
		public override void Click( UIMouseEvent evt ) {
			this.Focus();
			base.Click( evt );
		}

		////////////////

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

		private void UpdateFocus() {
			if( this.HasFocus ) {
				Main.blockInput = true; // Force the point!

				this.CursorAnimation++;

				Terraria.GameInput.PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newText = Main.GetInputText( this.Text );

				if( !newText.Equals( this.Text ) ) {
					this.SetTextWithValidation( newText );
				}

				if( UIHelpers.JustPressedKey( Keys.Escape ) || UIHelpers.JustPressedKey( Keys.Enter ) ) {
					this.Unfocus();
				}
			}

			if( this.HasFocus ) {
				var mouse = new Vector2( Main.mouseX, Main.mouseY );
				if( !this.ContainsPoint( mouse ) && Main.mouseLeft ) {
					this.Unfocus();
				}
			}
		}
	}
}
