using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Utilities.UI {
	class UITextArea : UIPanel {
		public string Text { get; private set; }
		public Vector2 TextSize { get; private set; }
		public int CursorPos { get; private set; }
		public int CursorAnimation { get; private set; }
		public bool HasFocus { get; private set; }


		////////////////

		public UITextArea() {
			this.SetText( "" );
			this.CursorPos = 0;
			this.CursorAnimation = 0;
			this.HasFocus = false;
		}


		////////////////

		public void SetText( string text ) {
			this.TextSize = new Vector2( (Main.fontMouseText).MeasureString( text.ToString() ).X, 16f );
			this.Text = text;

			this.MinWidth.Set( this.TextSize.X + this.PaddingLeft + this.PaddingRight, 0.0f );
			this.MinHeight.Set( this.TextSize.Y + this.PaddingTop + this.PaddingBottom, 0.0f );
		}

		////////////////

		public override void Update( GameTime game_time ) {
			if( this.HasFocus ) {
				//Main.blockInput = true;

				this.CursorAnimation++;

				Terraria.GameInput.PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string new_text = Main.GetInputText( this.Text );

				if( !new_text.Equals( this.Text ) ) {
					this.CursorPos = new_text.Length;
					this.Text = new_text;
				}

				if( UIHelpers.UIHelpers.JustPressedKey( Keys.Escape ) || UIHelpers.UIHelpers.JustPressedKey(Keys.Enter) ) {
					this.Unfocus();
				}
			}

			if( this.HasFocus ) {
				Vector2 mouse = new Vector2( (float)Main.mouseX, (float)Main.mouseY );
				if( !this.ContainsPoint(mouse) && Main.mouseLeft ) {
					this.Unfocus();
				}
			}

			base.Update( game_time );
		}

		public override void Recalculate() {
			this.SetText( this.Text );
			base.Recalculate();
		}

		////////////////

		public override void Click( UIMouseEvent evt ) {
			this.Focus();
			base.Click( evt );
		}

		////////////////

		public void Focus() {
			if( this.HasFocus ) { return; }
			this.HasFocus = true;

			this.CursorAnimation = 0;

			//Main.blockInput = true;
			Main.clrInput();
		}

		public void Unfocus() {
			if( !this.HasFocus ) { return; }
			this.HasFocus = false;

			//Main.blockInput = false;
		}

		////////////////

		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			CalculatedStyle inner_dim = this.GetInnerDimensions();
			Vector2 pos = inner_dim.Position();
			
			Utils.DrawBorderString( sb, this.Text, pos, Color.White, 1f, 0.0f, 0.0f, -1 );
			
			if( this.HasFocus ) {
				Vector2 ime_pos = new Vector2( (float)(Main.screenWidth / 2), (float)(this.GetDimensions().ToRectangle().Bottom + 32) );
				Main.instance.DrawWindowsIMEPanel( ime_pos, 0.5f );

				if( (this.CursorAnimation %= 40) <= 20 ) {
					Vector2 offset = new Vector2( (Main.fontMouseText).MeasureString( this.Text.Substring( 0, this.CursorPos ) ).X, 16f );
					pos.X += (float)offset.X + 2.0f;    //((inner_dim.Width - this.TextSize.X) * 0.5f)

					Utils.DrawBorderString( sb, "|", pos, Color.White );
				}
			}
		}
	}
}
