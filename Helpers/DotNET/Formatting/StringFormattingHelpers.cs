using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Helpers.DotNET.Formatting {
	/** <summary>Assorted static "helper" functions pertaining to formatting string data.</summary> */
	public class StringFormattingHelpers {
		public static string SanitizeMarkdown( string input ) {
			return input.Replace( "*", "\\*" )
				.Replace( "|", "\\|" )
				.Replace( "=", "\\=" )
				.Replace( "_", "\\_" )
				.Replace( ".", "\\." )
				.Replace( "[", "\\[" )
				.Replace( "]", "\\]" )
				.Replace( "!", "\\!" )
				.Replace( "<", "\\<" )
				.Replace( ">", "\\>" )
				.Replace( ":", "\\:" )
				.Replace( "`", "\\`" );
		}


		/** <summary>
		* Converts the given decimal number to the numeral system with the
		* specified radix (in the range [2, 36]).
		* </summary>
		* <param name="number">The number to convert.</param>
		* <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
		* <returns></returns>
		*/
		public static string ConvertDecimalToRadix( long number, int radix ) {
			const int bitsInLong = 64;
			const string digits = "0123456789abcdefghijklmnopqrstuvwxyz";

			if( radix < 2 || radix > digits.Length ) {
				throw new HamstarException( "The radix must be >= 2 and <= " + digits.Length.ToString() );
			}

			if( number == 0 ) {
				return "0";
			}

			int index = bitsInLong - 1;
			long currNum = Math.Abs( number );
			char[] chars = new char[bitsInLong];

			while( currNum != 0 ) {
				int remainder = (int)( currNum % radix );
				chars[index--] = digits[remainder];
				currNum = currNum / radix;
			}

			string result = new String( chars, index + 1, bitsInLong - index - 1 );
			if( number < 0 ) {
				result = "-" + result;
			}

			return result;
		}
	}
}
