using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Helpers.TmlHelpers.CommandsHelpers {
	public static class CommandsHelpers {
		public static string GetQuotedStringFromArgsAt( string[] args, int startPos, out int nextArgIdx ) {
			nextArgIdx = -1;

			if( startPos >= args.Length || args[startPos].Length == 0 || args[startPos][0] != '"' ) {
				return "";
			}

			string startSeg = args[startPos].Substring( 1 );
			if( args[startPos][ startSeg.Length - 1 ] == '"' ) {
				nextArgIdx = startPos + 1;
				return startSeg;
			}

			IList<string> segs = new List<string> { startSeg };

			for( int i=startPos+1; i<args.Length; i++ ) {
				string seg = args[i];
				
				if( seg.Length > 0 && seg[ seg.Length-1 ] == '"' ) {
					string subSeg = seg.Substring( 0, seg.Length - 1 );
					segs.Add( subSeg );

					nextArgIdx = i + 1;

					return string.Join( " ", segs.ToArray() );
				}
				segs.Add( args[i] );
			}

			return "";
		}
	}
}
