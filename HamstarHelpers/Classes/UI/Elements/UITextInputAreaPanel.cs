using System;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a text area UI panel element with crop-to-fit text input. Captures focus while in use. Does not currently implement
	/// multi-line support (yet).
	/// </summary>
	public partial class UITextInputAreaPanel : UIThemedPanel, IToggleable {
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
		/// Fires as the text input changes.
		/// </summary>
		public event TextEventHandler OnPreTextChange;
		/// <summary>
		/// Fires on when input is no longer selected.
		/// </summary>
		public event FocusHandler OnUnfocus;


		////////////////

		/// <summary>
		/// Current text.
		/// </summary>
		public string Text { get; private set; }
		/// <summary>
		/// Text in its displayed form (includes cursor).
		/// </summary>
		public string DisplayText { get; private set; }
		/// <summary>
		/// "Default" text. Appears when no text is input. Not counted as input.
		/// </summary>
		public string Hint { get; private set; }
		/// <summary>
		/// Maximum length of text input.
		/// </summary>
		public int MaxLength { get; private set; }

		/// <summary>
		/// Color of input text.
		/// </summary>
		public Color TextColor = Color.White;
		/// <summary>
		/// Color of mouse hover text.
		/// </summary>
		public Color HintColor = Color.Gray;

		/// <summary>
		/// Position of input cursor in input string.
		/// </summary>
		public int CursorPos { get; private set; }
		/// <summary>
		/// State of input cursor flashing animation.
		/// </summary>
		public int CursorAnimation { get; private set; }

		/// <summary>
		/// Indicates text is being input.
		/// </summary>
		public bool HasFocus { get; private set; }

		/// @private
		[Obsolete( "use IsInteractive", true )]
		public bool IsEnabled {
			get => this.IsInteractive;
			private set => this.IsInteractive = value;
		}

		/// <summary>
		/// Indicates element is able to be edited.
		/// </summary>
		public bool IsInteractive { get; private set; } = true;



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="hint">Default text. Overridden with any input text.</param>
		/// <param name="maxLength">Maximum length of text input.</param>
		public UITextInputAreaPanel( UITheme theme, string hint, int maxLength=2024 ) : base( theme, true ) {
			// TODO Add multiline support

			this.Hint = hint;
			this.CursorPos = 0;
			this.CursorAnimation = 0;
			this.HasFocus = false;
			this.IsInteractive = true;
			this.MaxLength = maxLength;

			this.SetTextDirect( "" );

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Disables the text area (prevents text input).
		/// </summary>
		public override void Disable() {
			base.Disable();

			this.IsInteractive = false;

			if( this.HasFocus ) {
				this.Unfocus();
			}

			this.RefreshTheme();
		}

		/// <summary>
		/// Enables the text area.
		/// </summary>
		public override void Enable() {
			base.Enable();

			this.IsInteractive = true;

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public override void RefreshTheme() {
			if( this.IsInteractive ) {
				this.Theme.ApplyInput( this );
			} else {
				this.Theme.ApplyInputDisable( this );
			}
		}


		////////////////

		/// <summary>
		/// Updates state of input, including cursor animation.
		/// </summary>
		/// <param name="gameTime">Unused.</param>
		public override void Update( GameTime gameTime ) {
			this.UpdateFocus();
			base.Update( gameTime );
		}

		/// <summary>
		/// Recalculates element positions.
		/// </summary>
		public override void Recalculate() {
			this.SetTextDirect( this.Text );
			base.Recalculate();
		}
	}
}
