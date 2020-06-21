using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a simpler append-only text field input element (no panel). Suited for main menu use.
	/// </summary>
	public partial class UITextInputElement : UIElement, IToggleable {
		/// <summary>
		/// Draws element. Also handles text input changes.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically given `Main.spriteBatch`.</param>
		protected override void DrawSelf( SpriteBatch sb ) {
			if( !this.IsHidden ) {
				base.DrawSelf( sb );
			}

			////

			if( this.IsInteractive ) {
				this.UpdateInteractivity();
			}

			if( !this.IsHidden ) {
				this.DrawSelfText( sb );
			}
		}

		private void DrawSelfText( SpriteBatch sb ) {
			CalculatedStyle dim = this.GetDimensions();

			var pos = new Vector2( dim.X + this.PaddingLeft, dim.Y + this.PaddingTop );

			// Draw text
			if( this.Text.Length == 0 ) {
				Utils.DrawBorderString( sb, this.HintText, pos, Color.Gray, 1f );
			} else {
				string displayStr = this.Text;

				// Draw cursor
				if( this.HasFocus ) {
					if( ++this.CursorAnimation % 40 < 20 ) {
						displayStr = displayStr + "|";
					}
				}

				Utils.DrawBorderString( sb, displayStr, pos, this.TextColor, 1f );
			}
		}
	}
}
