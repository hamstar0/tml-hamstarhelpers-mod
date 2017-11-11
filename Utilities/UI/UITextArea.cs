using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Utilities.UI {
	public class UITextArea : UIPanel {
		public string Text { get; private set; }
		public string Hint { get; private set; }

		public Vector2 TextSize { get; private set; }
		public Color TextColor = Color.White;
		public Color HintColor = Color.Gray;

		public int CursorPos { get; private set; }
		public int CursorAnimation { get; private set; }

		public bool HasFocus { get; private set; }
		public bool IsEnabled { get; private set; }

		private UITheme Theme;


		////////////////

		public UITextArea( UITheme theme, string hint ) {
			this.Theme = theme;

			this.SetText( "" );
			this.Hint = hint;
			this.CursorPos = 0;
			this.CursorAnimation = 0;
			this.HasFocus = false;
			this.IsEnabled = false;
		}


		////////////////

		public void SetText( string text ) {
			this.TextSize = new Vector2( (Main.fontMouseText).MeasureString( text ).X, 16f );
			this.Text = text;

			this.MinWidth.Set( this.TextSize.X + this.PaddingLeft + this.PaddingRight, 0.0f );
			this.MinHeight.Set( this.TextSize.Y + this.PaddingTop + this.PaddingBottom, 0.0f );
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
			if( !this.IsEnabled ) { return; }
			if( this.HasFocus ) { return; }
			this.HasFocus = true;

			this.CursorAnimation = 0;

			Main.blockInput = true;
			Main.clrInput();
		}

		public void Unfocus() {
			if( !this.HasFocus ) { return; }
			this.HasFocus = false;

			Main.blockInput = false;
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
						Vector2 offset = new Vector2( (Main.fontMouseText).MeasureString( substr ).X, 16f );
						pos.X += (float)offset.X + 2.0f;    //((inner_dim.Width - this.TextSize.X) * 0.5f)

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
