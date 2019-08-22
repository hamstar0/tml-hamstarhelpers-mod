using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a focusable text area UI panel element with crop-to-fit text input. Does not currently implement multi-line support (yet).
	/// </summary>
	public class UITextArea : UIThemedPanel {
		/// <summary>
		/// Event type that fires every time the text changes.
		/// </summary>
		/// <param name="newText">Changed text.</param>
		public delegate void TextChangeEvent( StringBuilder newText );


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
		/// Fires as the text input changes.
		/// </summary>
		public event TextChangeEvent OnPreChange;

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
		/// <summary>
		/// Indicates element is able to be edited.
		/// </summary>
		public bool IsEnabled { get; private set; }



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="hint">Default text. Overridden with any input text.</param>
		/// <param name="maxLength">Maximum length of text input.</param>
		public UITextArea( UITheme theme, string hint, int maxLength=2024 ) : base( theme, true ) {
			// TODO Add multiline support

			this.Hint = hint;
			this.CursorPos = 0;
			this.CursorAnimation = 0;
			this.HasFocus = false;
			this.IsEnabled = false;
			this.MaxLength = maxLength;

			this.SetText( "" );

			this.RefreshTheme();
		}


		////////////////
		
		/// <summary>
		/// Manually sets the input text, accommodating cursor position.
		/// </summary>
		/// <param name="text">New text.</param>
		public void SetText( string text ) {
			var strBldr = new StringBuilder( text );
			if( this.OnPreChange != null ) {
				this.OnPreChange.Invoke( strBldr );
			}

			text = strBldr.ToString();

			if( text.Length > this.MaxLength ) {
				text = text.Substring( 0, this.MaxLength );
			}

			this.Text = text;
			this.CursorPos = text.Length; // TODO: Allow cursor moving
			this.DisplayText = UITextArea.GetFittedText( text, this.CursorPos, this.GetInnerDimensions().Width );
		}


		////////////////

		/// <summary>
		/// Updates state of input, including cursor animation.
		/// </summary>
		/// <param name="gameTime">Unused.</param>
		public override void Update( GameTime gameTime ) {
			if( this.HasFocus ) {
				Main.blockInput = true;	// Force the point!

				this.CursorAnimation++;

				Terraria.GameInput.PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newText = Main.GetInputText( this.Text );

				if( !newText.Equals( this.Text ) ) {
					this.SetText( newText );
				}

				if( UIHelpers.JustPressedKey(Keys.Escape) || UIHelpers.JustPressedKey(Keys.Enter) ) {
					this.Unfocus();
				}
			}

			if( this.HasFocus ) {
				Vector2 mouse = new Vector2( Main.mouseX, Main.mouseY );
				if( !this.ContainsPoint(mouse) && Main.mouseLeft ) {
					this.Unfocus();
				}
			}

			base.Update( gameTime );
		}

		/// <summary>
		/// Recalculates element positions.
		/// </summary>
		public override void Recalculate() {
			this.SetText( this.Text );
			base.Recalculate();
		}


		////////////////

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
			if( !this.IsEnabled ) { return false; }
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

			return true;
		}


		////////////////

		/// <summary>
		/// Disables the text area (prevents text input).
		/// </summary>
		public void Disable() {
			this.IsEnabled = false;

			if( this.HasFocus ) {
				this.Unfocus();
			}

			this.RefreshTheme();
		}

		/// <summary>
		/// Enables the text area.
		/// </summary>
		public void Enable() {
			this.IsEnabled = true;

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public override void RefreshTheme() {
			if( this.IsEnabled ) {
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
					var imePos = new Vector2( (float)(Main.screenWidth / 2), (float)(this.GetDimensions().ToRectangle().Bottom + 32) );
					Main.instance.DrawWindowsIMEPanel( imePos, 0.5f );

					if( (this.CursorAnimation %= 40) <= 20 ) {
						// TODO cursor needs to be offset according to display text:
						
						float cursorOffsetX = this.DisplayText.Length == 0 ? 0f : Main.fontMouseText.MeasureString( this.DisplayText ).X;
						pos.X += cursorOffsetX + 2.0f;    //((innerDim.Width - this.TextSize.X) * 0.5f)

						Utils.DrawBorderString( sb, "|", pos, Color.White );
					}
				} else {
					if( this.DisplayText == "" && this.IsEnabled ) {
						Utils.DrawBorderString( sb, this.Hint, pos, this.HintColor );
					}
				}
			} catch( Exception e ) {
				LogHelpers.Log( e.ToString() );
			}
		}


		////////////////

		/// <summary>
		/// Applies a cursor to a given string.
		/// </summary>
		/// <param name="text">Input string.</param>
		/// <param name="cursorPos">Cursor's position.</param>
		/// <param name="width">Width of input.</param>
		/// <returns>Cursor-added input string.</returns>
		public static string GetFittedText( string text, int cursorPos, float width ) {
			int start = 0;
			int end = text.Length;
			string substr = text;

			while( Main.fontMouseText.MeasureString( substr ).X > width ) {
				if( cursorPos >= end ) {
					substr = substr.Substring( 1 );
					start++;
				} else if( cursorPos <= start ) {
					end--;
					substr = substr.Substring( 0, end - start );
				} else {
					start++;
					end--;
					substr = substr.Substring( 1, end - start );
				}
			}

			//if( end < text.Length && end > 0 ) {
			//	substr = substr.Substring( 0, substr.Length - 1 ) + '…';
			//}
			//if( start > 0 && substr.Length > 0 ) {
			//	substr = '…' + substr.Substring( 1 );
			//}

			return substr;
		}
	}
}
