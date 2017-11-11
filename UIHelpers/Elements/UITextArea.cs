using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.UIHelpers.Elements {
	public class UITextArea : UIPanel {
		public string Text { get; private set; }
		public string Hint { get; private set; }
		
		public Color TextColor = Color.White;
		public Color HintColor = Color.Gray;

		public int CursorPos { get; private set; }
		public int CursorAnimation { get; private set; }

		public bool HasFocus { get; private set; }
		public bool IsEnabled { get; private set; }

		private UITheme Theme;


		////////////////

		public UITextArea( UITheme theme, string hint ) {
			// TODO Add multiline support

			this.Theme = theme;

			this.SetText( "", true );
			this.Hint = hint;
			this.CursorPos = 0;
			this.CursorAnimation = 0;
			this.HasFocus = false;
			this.IsEnabled = false;
		}


		////////////////

		public void SetText( string text, bool allow_overflow ) {
			string substr = text;

			if( !allow_overflow ) {
				try {
					float width = this.GetDimensions().Width;
					float text_len = Main.fontMouseText.MeasureString( substr ).X;

					while( text_len > width ) {
						substr = substr.Substring( 0, substr.Length - 1 );
						text_len = Main.fontMouseText.MeasureString( substr ).X;
					}
				} catch( Exception e ) {
					ErrorLogger.Log( e.ToString() );
				}
			}
			
			this.Text = substr;
			this.CursorPos = this.CursorPos > substr.Length ? substr.Length : this.CursorPos;
		}


		////////////////

		public override void Update( GameTime game_time ) {
			if( this.HasFocus ) {
				Main.blockInput = true;	// Force the point!

				this.CursorAnimation++;

				Terraria.GameInput.PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string new_text = Main.GetInputText( this.Text );

				if( !new_text.Equals( this.Text ) ) {
					this.CursorPos = new_text.Length;
					this.Text = new_text;
				}

				if( UIHelpers.JustPressedKey( Keys.Escape ) || UIHelpers.JustPressedKey(Keys.Enter) ) {
					this.Unfocus();
				}
			}

			if( this.HasFocus ) {
				Vector2 mouse = new Vector2( Main.mouseX, Main.mouseY );
				if( !this.ContainsPoint(mouse) && Main.mouseLeft ) {
					this.Unfocus();
				}
			}

			base.Update( game_time );
		}

		public override void Recalculate() {
			this.SetText( this.Text, false );
			base.Recalculate();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			this.Focus();
			base.Click( evt );
		}

		////////////////

		public bool Focus() {
			if( !this.IsEnabled ) { return false; }
			if( this.HasFocus ) { return false; }
			this.HasFocus = true;

			this.CursorAnimation = 0;

			Main.blockInput = true;
			Main.clrInput();

			return true;
		}

		public bool Unfocus() {
			if( !this.HasFocus ) { return false; }
			this.HasFocus = false;

			Main.blockInput = false;

			return true;
		}


		////////////////

		public void Disable() {
			this.IsEnabled = false;

			if( this.HasFocus ) {
				this.Unfocus();
			}

			this.Theme.ApplyInputDisable( this );
		}

		public void Enable() {
			this.IsEnabled = true;

			this.Theme.ApplyInput( this );
		}


		////////////////

		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			try {
				CalculatedStyle inner_dim = this.GetInnerDimensions();
				Vector2 pos = inner_dim.Position();

				if( this.Text != "" ) {
					Utils.DrawBorderString( sb, this.Text, pos, this.TextColor, 1f, 0.0f, 0.0f, -1 );
				}

				if( this.HasFocus ) {
					Vector2 ime_pos = new Vector2( (float)(Main.screenWidth / 2), (float)(this.GetDimensions().ToRectangle().Bottom + 32) );
					Main.instance.DrawWindowsIMEPanel( ime_pos, 0.5f );

					if( (this.CursorAnimation %= 40) <= 20 ) {
						string substr = string.IsNullOrEmpty(this.Text) ? "" : this.Text.Substring( 0, this.CursorPos );
						float cursor_offset_x = substr.Length == 0 ? 0f : Main.fontMouseText.MeasureString( substr ).X;

						pos.X += cursor_offset_x + 2.0f;    //((inner_dim.Width - this.TextSize.X) * 0.5f)

						Utils.DrawBorderString( sb, "|", pos, Color.White );
					}
				} else {
					if( this.Text == "" && this.IsEnabled ) {
						Utils.DrawBorderString( sb, this.Hint, pos, this.HintColor );
					}
				}
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}
		}
	}
}
