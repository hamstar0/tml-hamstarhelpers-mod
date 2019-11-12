using HamstarHelpers.Classes.UI.Theme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a simpler append-only text field input panel. Suited for main menu use.
	/// </summary>
	public class UITextInputPanel : UIThemedPanel {
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
		/// Fires on text change.
		/// </summary>
		public event TextEventHandler OnTextChange;
		/// <summary>
		/// Fires on when input is no longer selected.
		/// </summary>
		public event FocusHandler OnUnfocus;


		////////////////

		/// <summary>
		/// Text color.
		/// </summary>
		public Color TextColor;

		/// <summary>
		/// "Default" text. Appears when no text is input. Not counted as input.
		/// </summary>
		public string HintText { get; private set; }

		private string Text = "";
		private uint CursorAnimation;
		private bool IsSelected = false;



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="hintText">"Default" text. Appears when no text is input. Not counted as input.</param>
		public UITextInputPanel( UITheme theme, string hintText ) : base( theme, true ) {
			this.HintText = hintText;
			
			this.SetPadding( 6f );
			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public override void RefreshTheme() {
			this.Theme.ApplyInput( this );
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


		////////////////

		/// @private
		public override void Draw( SpriteBatch spriteBatch ) {
			if( !this.IsHidden ) {
				float opacity = this.ComputeCurrentOpacity();
				Color oldTextColor = this.TextColor;

				this.TextColor *= opacity;

				base.Draw( spriteBatch );

				this.TextColor = oldTextColor;
			}
		}

		/// <summary>
		/// Draws element. Also handles text input changes.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically given `Main.spriteBatch`.</param>
		protected override void DrawSelf( SpriteBatch sb ) {
			float opacity = this.ComputeCurrentOpacity();
			Color oldTextColor = this.TextColor;

			this.TextColor *= opacity;

			base.DrawSelf( sb );

			////

			CalculatedStyle dim = this.GetDimensions();

			// Detect if user selects this element
			if( Main.mouseLeft ) {
				bool isNowSelected = false;

				if( Main.mouseX >= dim.X && Main.mouseX < ( dim.X + dim.Width ) ) {
					if( Main.mouseY >= dim.Y && Main.mouseY < ( dim.Y + dim.Height ) ) {
						isNowSelected = true;
						Main.keyCount = 0;
					}
				}

				if( this.IsSelected && !isNowSelected ) {
					this.OnUnfocus?.Invoke();
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

					if( this.OnTextChange?.Invoke( newStrMuta ) ?? true ) {
						this.Text = newStrMuta.ToString();
					}
				}
			}

			var pos = new Vector2( dim.X + this.PaddingLeft, dim.Y + this.PaddingTop );

			// Draw text
			if( this.Text.Length == 0 ) {
				Utils.DrawBorderString( sb, this.HintText, pos, Color.Gray, 1f );
			} else {
				string displayStr = this.Text;

				// Draw cursor
				if( this.IsSelected ) {
					if( ++this.CursorAnimation % 40 < 20 ) {
						displayStr = displayStr + "|";
					}
				}

				Utils.DrawBorderString( sb, displayStr, pos, this.TextColor, 1f );
			}

			this.TextColor = oldTextColor;
		}
	}
}
