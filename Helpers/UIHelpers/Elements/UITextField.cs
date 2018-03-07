using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;


namespace HamstarHelpers.Helpers.UIHelpers.Elements {
	public class UITextField : UIPanel {
		public delegate void EventHandler( Object sender, EventArgs e );

		public event EventHandler OnTextChange;
		public Color TextColor;

		private string HintText;
		internal string Text = "";
		private uint CursorAnimation;



		////////////////

		public UITextField( UITheme theme, string hint_text ) {
			this.HintText = hint_text;

			theme.ApplyInput( this );
			this.SetPadding( 6f );
		}


		////////////////

		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			PlayerInput.WritingText = true;
			Main.instance.HandleIME();

			string new_str = Main.GetInputText( this.Text );

			this.Text = new_str;

			if( !new_str.Equals( this.Text ) ) {
				this.OnTextChange( this, new EventArgs() );
			}
			
			CalculatedStyle dim = this.GetDimensions();
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
