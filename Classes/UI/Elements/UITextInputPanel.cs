using HamstarHelpers.Classes.UI.Theme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
		/// <param name="input">Changed text.</param>
		public delegate void TextEventHandler( string input );


		/// <summary>
		/// Fires on text change.
		/// </summary>
		public event TextEventHandler OnTextChange;
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

		/// <summary>
		/// Draws element. Also handles text input changes.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically given `Main.spriteBatch`.</param>
		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			////

			CalculatedStyle dim = this.GetDimensions();

			// Detect if user selects this element
			if( Main.mouseLeft ) {
				this.IsSelected = false;

				if( Main.mouseX >= dim.X && Main.mouseX < ( dim.X + dim.Width ) ) {
					if( Main.mouseY >= dim.Y && Main.mouseY < ( dim.Y + dim.Height ) ) {
						this.IsSelected = true;
					}
				}
			}

			// Apply text inputs
			if( this.IsSelected ) {
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newStr = Main.GetInputText( this.Text );

				if( !newStr.Equals( this.Text ) ) {
					this.OnTextChange( newStr );
				}

				this.Text = newStr;
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
		}
	}
}
