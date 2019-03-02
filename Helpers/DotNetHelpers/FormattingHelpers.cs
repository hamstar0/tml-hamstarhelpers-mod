using System;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public class FormattingHelpers {
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
	}
}
