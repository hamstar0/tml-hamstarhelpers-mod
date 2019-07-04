using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	/// <summary>
	/// Defines a custom event for UITextField use.
	/// </summary>
	public class TextInputEventArgs : EventArgs {
		/// <summary>
		/// Input text.
		/// </summary>
		public string Text;

		/// <param name="text">Input text.</param>
		public TextInputEventArgs( string text ) : base() {
			this.Text = text;
		}
	}




	/// <summary>
	/// Defines a simpler append-only text field input panel. Suited for main menu use.
	/// </summary>
	public class UITextField : UIPanel {
		/// <summary>
		/// Event handler for text input events
		/// </summary>
		/// <param name="sender">Context of text change (AKA current element).</param>
		/// <param name="e">Changed text (wrapped).</param>
		public delegate void EventHandler( Object sender, EventArgs e );


		/// <summary>
		/// Appearance style.
		/// </summary>
		public UITheme Theme { get; protected set; }

		/// <summary>
		/// Fires on text change.
		/// </summary>
		public event EventHandler OnTextChange;
		/// <summary>
		/// Text color.
		/// </summary>
		public Color TextColor;

		/// <summary>
		/// "Default" text. Appears when no text is input. Not counted as input.
		/// </summary>
		private string HintText;

		private string Text = "";
		private uint CursorAnimation;
		private bool IsSelected = false;



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="hintText">"Default" text. Appears when no text is input. Not counted as input.</param>
		public UITextField( UITheme theme, string hintText ) {
			this.Theme = theme;
			this.HintText = hintText;
			
			this.SetPadding( 6f );
			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public virtual void RefreshTheme() {
			this.Theme.ApplyInput( this );
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

			if( Main.mouseLeft ) {
				this.IsSelected = false;

				if( Main.mouseX >= dim.X && Main.mouseX < ( dim.X + dim.Width ) ) {
					if( Main.mouseY >= dim.Y && Main.mouseY < ( dim.Y + dim.Height ) ) {
						this.IsSelected = true;
					}
				}
			}
			
			if( this.IsSelected ) {
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newStr = Main.GetInputText( this.Text );

				if( !newStr.Equals( this.Text ) ) {
					this.OnTextChange( this, new TextInputEventArgs( newStr ) );
				}

				this.Text = newStr;
			}

			var pos = new Vector2( dim.X + this.PaddingLeft, dim.Y + this.PaddingTop );

			if( this.Text.Length == 0 ) {
				Utils.DrawBorderString( sb, this.HintText, pos, Color.Gray, 1f );
			} else {
				string displayStr = this.Text;

				if( ++this.CursorAnimation % 40 < 20 ) {
					displayStr = displayStr + "|";
				}

				Utils.DrawBorderString( sb, displayStr, pos, this.TextColor, 1f );
			}
		}
	}
}
