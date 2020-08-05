using System;
using System.Text;


namespace HamstarHelpers.Helpers.DotNET.Encoding {
	/// <summary>
	/// Assorted static "helper" functions pertaining to ASCII strings.
	/// </summary>
	public class EncodingHelpers {
		/// <summary>
		/// Strips non-ASCII (unicode) characters from a given string.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string SanitizeForASCII( string input ) {
			return System.Text.Encoding.ASCII.GetString(
				System.Text.Encoding.Convert(
					System.Text.Encoding.UTF8,
					System.Text.Encoding.GetEncoding(
						System.Text.Encoding.ASCII.EncodingName,
						new EncoderReplacementFallback( string.Empty ),
						new DecoderExceptionFallback()
					),
					System.Text.Encoding.UTF8.GetBytes( input )
				)
			);
		}
	}
}
