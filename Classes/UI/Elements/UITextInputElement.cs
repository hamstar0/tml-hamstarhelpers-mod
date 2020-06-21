using System;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a simpler append-only text field input element (no panel). Suited for main menu use.
	/// </summary>
	public partial class UITextInputElement : UIElement, IToggleable {
		/// <summary>
		/// Event handler for text input events
		/// </summary>
		/// <param name="input"></param>
		/// <returns>`true` if string is valid</returns>
		public delegate bool TextEventHandler( StringBuilder input );
		/// <summary>
		/// Event handler for focus change events.
		/// </summary>
		public delegate void FocusHandler();



		////////////////

		/// <summary>
		/// Fires on text change. Actions here should never alter current, sibling, or child elements.
		/// </summary>
		public event TextEventHandler OnTextChange;

		/// <summary>
		/// Fires on when input is no longer selected.
		/// </summary>
		public event FocusHandler OnUnfocus;


		////////////////

		/// <summary>
		/// Enables mouse interactivity.
		/// </summary>
		public bool IsInteractive { get; protected set; } = true;

		/// <summary>
		/// Controls visibility.
		/// </summary>
		public bool IsHidden { get; protected set; } = true;

		/// <summary>
		/// Indicates this input is selected.
		/// </summary>
		public bool IsSelected { get; private set; } = false;


		////////////////

		/// <summary>
		/// Text color.
		/// </summary>
		public Color TextColor = Color.White;

		/// <summary>
		/// "Default" text. Appears when no text is input. Not counted as input.
		/// </summary>
		private string HintText;

		private string Text = "";
		private uint CursorAnimation;



		////////////////

		/// <summary></summary>
		/// <param name="hintText">"Default" text. Appears when no text is input. Not counted as input.</param>
		public UITextInputElement( string hintText ) {
			this.HintText = hintText;
		}


		////////////////

		/// <summary>
		/// Enables or disables the current element.
		/// </summary>
		/// <param name="isEnabled"></param>
		public void Enable( bool isEnabled ) {
			this.IsInteractive = isEnabled;
		}

		/// <summary>
		/// Enables the current element.
		/// </summary>
		public void Enable() {
			this.IsInteractive = true;
		}

		/// <summary>
		/// Disables the current element.
		/// </summary>
		public void Disable() {
			this.IsInteractive = false;
		}

		////

		/// <summary>
		/// Enables or disables element hiding. Interactivity still enabled.
		/// </summary>
		/// <param name="isHidden"></param>
		public void Hide( bool isHidden ) {
			this.IsHidden = isHidden;
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public string GetText() {
			return this.Text;
		}

		/// <summary></summary>
		/// <param name="text"></param>
		public void SetText( string text ) {
			this.Text = text;
		}
	}
}
