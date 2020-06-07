using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;


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
		public bool IsInteractive { get; private set; }



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
			this.IsInteractive = false;
			this.MaxLength = maxLength;

			this.SetTextDirect( "" );

			this.RefreshTheme();
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
		/// Draws the element. Animates cursor, draws hint text.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically given `Main.spriteBatch`.</param>
		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			try {
				CalculatedStyle innerDim = this.GetInnerDimensions();
				Vector2 pos = innerDim.Position();

				if( this.DisplayText != "" ) {
					Utils.DrawBorderString( sb, this.DisplayText, pos, this.TextColor, 1f, 0.0f, 0.0f, -1 );
				}

				if( this.HasFocus ) {
					var imePos = new Vector2(
						Main.screenWidth / 2,
						this.GetDimensions().ToRectangle().Bottom + 32
					);
					Main.instance.DrawWindowsIMEPanel( imePos, 0.5f );

					if( (this.CursorAnimation %= 40) <= 20 ) {
						// TODO cursor needs to be offset according to display text:
						
						float cursorOffsetX = this.DisplayText.Length == 0
							? 0f
							: Main.fontMouseText.MeasureString( this.DisplayText ).X;
						pos.X += cursorOffsetX + 2.0f;    //((innerDim.Width - this.TextSize.X) * 0.5f)

						Utils.DrawBorderString( sb, "|", pos, Color.White );
					}
				} else {
					if( this.DisplayText == "" && this.IsInteractive ) {
						Utils.DrawBorderString( sb, this.Hint, pos, this.HintColor );
					}
				}
			} catch( Exception e ) {
				LogHelpers.Log( e.ToString() );
			}
		}
	}
}
