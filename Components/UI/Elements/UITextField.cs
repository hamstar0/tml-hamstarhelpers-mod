using HamstarHelpers.Helpers.UIHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	public class TextInputEventArgs : EventArgs {
		public string Text;

		public TextInputEventArgs( string text ) : base() {
			this.Text = text;
		}
	}



	public class UITextField : UIPanel {
		public delegate void EventHandler( Object sender, EventArgs e );


		public UITheme Theme { get; protected set; }

		public event EventHandler OnTextChange;
		public Color TextColor;

		private string HintText;
		internal string Text = "";
		private uint CursorAnimation;

		private bool IsSelected = false;



		////////////////

		public UITextField( UITheme theme, string hintText ) {
			this.Theme = theme;
			this.HintText = hintText;
			
			this.SetPadding( 6f );
			this.RefreshTheme();
		}


		////////////////

		public virtual void RefreshTheme() {
			this.Theme.ApplyInput( this );
		}


		////////////////

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
