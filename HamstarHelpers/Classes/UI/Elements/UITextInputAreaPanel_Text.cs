using System;
using System.Text;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a text area UI panel element with crop-to-fit text input. Captures focus while in use. Does not currently implement
	/// multi-line support (yet).
	/// </summary>
	public partial class UITextInputAreaPanel : UIThemedPanel, IToggleable {
		/// <summary>
		/// Applies a cursor to a given string. Cuts down the string to fit within the specified width.
		/// </summary>
		/// <param name="text">Input string.</param>
		/// <param name="cursorPos">Cursor's position.</param>
		/// <param name="maxAllowedWidth">Max allowed (pixel) width of string.</param>
		/// <returns>Cursor-added input string.</returns>
		public static string GetFittedText( string text, int cursorPos, float maxAllowedWidth ) {
			int start = 0;
			int end = text.Length;
			string substr = text;

			while( Main.fontMouseText.MeasureString( substr ).X > maxAllowedWidth ) {
				if( cursorPos >= end ) {
					start++;
					substr = ( end - start ) >= 1
						? substr.Substring( 1 )
						: "";
				} else if( cursorPos <= start ) {
					end--;
					substr = substr.Substring( 0, end - start );
				} else {
					start++;
					end--;
					substr = ( end - start ) >= 1
						? substr.Substring( 1, end - start )
						: "";
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



		////////////////

		/// <summary>
		/// Manually sets the input text, accommodating cursor position. Calls OnPreTextChange to validate.
		/// </summary>
		/// <param name="text">New text.</param>
		/// <returns></returns>
		public (bool isValid, string croppedString) SetTextWithValidation( string text ) {
			var strBldr = new StringBuilder( text );
			if( !this.OnPreTextChange?.Invoke(strBldr) ?? false ) {
				return (false, text);
			}

			string newStr = strBldr.ToString();
			this.SetTextDirect( newStr );

			return (true, newStr);
		}

		/// <summary>
		/// Manually sets the input text, accommodating cursor position.
		/// </summary>
		/// <param name="text">New text.</param>
		/// <returns>Input string cropped to MaxLength.</returns>
		public string SetTextDirect( string text ) {
			if( text.Length > this.MaxLength ) {
				text = text.Substring( 0, this.MaxLength );
			}

			this.Text = text;
			this.CursorPos = text.Length; // TODO: Allow cursor moving
			this.DisplayText = UITextInputAreaPanel.GetFittedText( text, this.CursorPos, this.GetInnerDimensions().Width );

			return text;
		}
	}
}
