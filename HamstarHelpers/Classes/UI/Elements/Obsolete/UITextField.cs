using HamstarHelpers.Classes.UI.Theme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// @private
	[Obsolete("use UITextInputPanel", true)]
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




	/// @private
	[Obsolete( "use UITextInputPanel", true )]
	public class UITextField : UIThemedPanel {
		/// @private
		public delegate void TextEventHandler( Object sender, TextInputEventArgs e );


		/// @private
		public event TextEventHandler OnTextChange;
		/// @private
		public Color TextColor;

		/// @private
		private string HintText;

		private string Text = "";
		private uint CursorAnimation;
		private bool IsSelected = false;



		////////////////

		/// @private
		public UITextField( UITheme theme, string hintText ) : base( theme, true ) {
			this.HintText = hintText;
			
			this.SetPadding( 6f );
			this.RefreshTheme();
		}


		////////////////

		/// @private
		public override void RefreshTheme() {
			this.Theme.ApplyInput( (UIPanel)this );
		}


		////////////////

		/// @private
		public string GetText() {
			return this.Text;
		}

		/// @private
		public void SetText( string text ) {
			this.Text = text;
		}


		////////////////

		/// @private
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
