using HamstarHelpers.UIHelpers;
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


		private UITheme Theme;

		public event EventHandler OnTextChange;
		public Color TextColor;

		private string HintText;
		internal string Text = "";
		private uint CursorAnimation;

		private bool IsSelected = false;



		////////////////

		public UITextField( UITheme theme, string hint_text ) {
			this.Theme = theme;
			this.HintText = hint_text;
			
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

				string new_str = Main.GetInputText( this.Text );

				if( !new_str.Equals( this.Text ) ) {
					this.OnTextChange( this, new TextInputEventArgs( new_str ) );
				}

				this.Text = new_str;
			}

			var pos = new Vector2( dim.X + this.PaddingLeft, dim.Y + this.PaddingTop );

			if( this.Text.Length == 0 ) {
				Utils.DrawBorderString( sb, this.HintText, pos, Color.Gray, 1f );
			} else {
				string display_str = this.Text;

				if( ++this.CursorAnimation % 40 < 20 ) {
					display_str = display_str + "|";
				}

				Utils.DrawBorderString( sb, display_str, pos, this.TextColor, 1f );
			}
		}
	}
}
