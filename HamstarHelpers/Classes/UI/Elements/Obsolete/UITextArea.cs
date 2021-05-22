using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// @private
	[Obsolete( "use UITextInputAreaPanel", true )]
	public class UITextArea : UIThemedPanel {
		/// @private
		public delegate void TextChangeEvent( StringBuilder newText );


		////////////////

		/// @private
		public string Text { get; private set; }
		/// @private
		public string DisplayText { get; private set; }
		/// @private
		public string Hint { get; private set; }
		/// @private
		public int MaxLength { get; private set; }

		/// @private
		public event TextChangeEvent OnPreChange;

		/// @private
		public Color TextColor = Color.White;
		/// @private
		public Color HintColor = Color.Gray;

		/// @private
		public int CursorPos { get; private set; }
		/// @private
		public int CursorAnimation { get; private set; }

		/// @private
		public bool HasFocus { get; private set; }
		/// @private
		public bool IsEnabled { get; private set; }



		////////////////

		/// @private
		public UITextArea( UITheme theme, string hint, int maxLength=2024 ) : base( theme, true ) {
			this.Hint = hint;
			this.CursorPos = 0;
			this.CursorAnimation = 0;
			this.HasFocus = false;
			this.IsEnabled = false;
			this.MaxLength = maxLength;

			this.SetText( "" );

			this.RefreshTheme();
		}


		////////////////

		/// @private
		public void SetText( string text ) {
			var strBldr = new StringBuilder( text );
			if( this.OnPreChange != null ) {
				this.OnPreChange.Invoke( strBldr );
			}

			text = strBldr.ToString();

			if( text.Length > this.MaxLength ) {
				text = text.Substring( 0, this.MaxLength );
			}

			this.Text = text;
			this.CursorPos = text.Length;
			this.DisplayText = UITextInputAreaPanel.GetFittedText( text, this.CursorPos, this.GetInnerDimensions().Width );
		}


		////////////////

		/// @private
		public override void Update( GameTime gameTime ) {
			if( this.HasFocus ) {
				Main.blockInput = true;	// Force the point!

				this.CursorAnimation++;

				Terraria.GameInput.PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newText = Main.GetInputText( this.Text );

				if( !newText.Equals( this.Text ) ) {
					this.SetText( newText );
				}

				if( UILibraries.JustPressedKey(Keys.Escape) || UILibraries.JustPressedKey(Keys.Enter) ) {
					this.Unfocus();
				}
			}

			if( this.HasFocus ) {
				Vector2 mouse = new Vector2( Main.mouseX, Main.mouseY );
				if( !this.ContainsPoint(mouse) && Main.mouseLeft ) {
					this.Unfocus();
				}
			}

			base.Update( gameTime );
		}

		/// @private
		public override void Recalculate() {
			this.SetText( this.Text );
			base.Recalculate();
		}


		////////////////

		/// @private
		public override void Click( UIMouseEvent evt ) {
			this.Focus();
			base.Click( evt );
		}

		////////////////

		/// @private
		public bool Focus() {
			if( !this.IsEnabled ) { return false; }
			if( this.HasFocus ) { return false; }
			this.HasFocus = true;

			this.CursorAnimation = 0;

			Main.blockInput = true;
			Main.clrInput();

			return true;
		}

		/// @private
		public bool Unfocus() {
			if( !this.HasFocus ) { return false; }
			this.HasFocus = false;

			Main.blockInput = false;

			return true;
		}


		////////////////

		/// @private
		public override void Disable() {
			base.Disable();

			this.IsEnabled = false;

			if( this.HasFocus ) {
				this.Unfocus();
			}

			this.RefreshTheme();
		}

		/// @private
		public override void Enable() {
			base.Enable();

			this.IsEnabled = true;

			this.RefreshTheme();
		}


		////////////////

		/// @private
		public override void RefreshTheme() {
			if( this.IsEnabled ) {
				this.Theme.ApplyInput( this );
			} else {
				this.Theme.ApplyInputDisable( this );
			}
		}


		////////////////

		/// @private
		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			try {
				CalculatedStyle innerDim = this.GetInnerDimensions();
				Vector2 pos = innerDim.Position();

				if( this.DisplayText != "" ) {
					Utils.DrawBorderString( sb, this.DisplayText, pos, this.TextColor, 1f, 0.0f, 0.0f, -1 );
				}

				if( this.HasFocus ) {
					var imePos = new Vector2( (float)(Main.screenWidth / 2), (float)(this.GetDimensions().ToRectangle().Bottom + 32) );
					Main.instance.DrawWindowsIMEPanel( imePos, 0.5f );

					if( (this.CursorAnimation %= 40) <= 20 ) {
						float cursorOffsetX = this.DisplayText.Length == 0 ? 0f : Main.fontMouseText.MeasureString( this.DisplayText ).X;
						pos.X += cursorOffsetX + 2.0f;    //((innerDim.Width - this.TextSize.X) * 0.5f)

						Utils.DrawBorderString( sb, "|", pos, Color.White );
					}
				} else {
					if( this.DisplayText == "" && this.IsEnabled ) {
						Utils.DrawBorderString( sb, this.Hint, pos, this.HintColor );
					}
				}
			} catch( Exception e ) {
				LogLibraries.Log( e.ToString() );
			}
		}


		////////////////

		/// @private
		public static string GetFittedText( string text, int cursorPos, float width ) {
			int start = 0;
			int end = text.Length;
			string substr = text;

			while( Main.fontMouseText.MeasureString( substr ).X > width ) {
				if( cursorPos >= end ) {
					substr = substr.Substring( 1 );
					start++;
				} else if( cursorPos <= start ) {
					end--;
					substr = substr.Substring( 0, end - start );
				} else {
					start++;
					end--;
					substr = substr.Substring( 1, end - start );
				}
			}

			return substr;
		}
	}
}
