using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	public class UITextArea : UIPanel {
		public delegate void TextChangeEvent( StringBuilder new_text );


		////////////////

		public string Text { get; private set; }
		public string DisplayText { get; private set; }
		public string Hint { get; private set; }
		public int MaxLength { get; private set; }

		public event TextChangeEvent OnPreChange;

		public Color TextColor = Color.White;
		public Color HintColor = Color.Gray;

		public int CursorPos { get; private set; }
		public int CursorAnimation { get; private set; }

		public bool HasFocus { get; private set; }
		public bool IsEnabled { get; private set; }

		private UITheme Theme;


		////////////////
		
		public UITextArea( UITheme theme, string hint, int max_length=2024 ) {
			// TODO Add multiline support

			this.Theme = theme;

			this.Hint = hint;
			this.CursorPos = 0;
			this.CursorAnimation = 0;
			this.HasFocus = false;
			this.IsEnabled = false;
			this.MaxLength = max_length;

			this.SetText( "" );

			this.RefreshTheme();
		}


		////////////////
		
		public void SetText( string text ) {
			var str_bldr = new StringBuilder( text );
			if( this.OnPreChange != null ) {
				this.OnPreChange.Invoke( str_bldr );
			}

			text = str_bldr.ToString();

			if( text.Length > this.MaxLength ) {
				text = text.Substring( 0, this.MaxLength );
			}

			this.Text = text;
			this.CursorPos = text.Length; // TODO: Allow cursor moving
			this.DisplayText = UITextArea.GetFittedText( text, this.CursorPos, this.GetInnerDimensions().Width );
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
					this.SetText( new_text );
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

			this.RefreshTheme();
		}

		public void Enable() {
			this.IsEnabled = true;

			this.RefreshTheme();
		}


		////////////////

		public virtual void RefreshTheme() {
			if( this.IsEnabled ) {
				this.Theme.ApplyInput( this );
			} else {
				this.Theme.ApplyInputDisable( this );
			}
		}


		////////////////

		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			try {
				CalculatedStyle inner_dim = this.GetInnerDimensions();
				Vector2 pos = inner_dim.Position();

				if( this.DisplayText != "" ) {
					Utils.DrawBorderString( sb, this.DisplayText, pos, this.TextColor, 1f, 0.0f, 0.0f, -1 );
				}

				if( this.HasFocus ) {
					Vector2 ime_pos = new Vector2( (float)(Main.screenWidth / 2), (float)(this.GetDimensions().ToRectangle().Bottom + 32) );
					Main.instance.DrawWindowsIMEPanel( ime_pos, 0.5f );

					if( (this.CursorAnimation %= 40) <= 20 ) {
						// TODO cursor needs to be offset according to display text:
						
						float cursor_offset_x = this.DisplayText.Length == 0 ? 0f : Main.fontMouseText.MeasureString( this.DisplayText ).X;
						pos.X += cursor_offset_x + 2.0f;    //((inner_dim.Width - this.TextSize.X) * 0.5f)

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

		public static string GetFittedText( string text, int cursor_pos, float width ) {
			int start = 0;
			int end = text.Length;
			string substr = text;

			while( Main.fontMouseText.MeasureString( substr ).X > width ) {
				if( cursor_pos >= end ) {
					substr = substr.Substring( 1 );
					start++;
				} else if( cursor_pos <= start ) {
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
