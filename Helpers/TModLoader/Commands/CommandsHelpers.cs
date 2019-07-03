using System.Collections.Generic;


namespace HamstarHelpers.Helpers.TModLoader.Commands {
	/** <summary>Assorted static "helper" functions pertaining to (chat or console) commands.</summary> */
	public static class CommandsHelpers {
		public static bool GetQuotedStringFromArgsAt( string[] args, int argStartIdx, out int argNextIdx, out string output ) {
			argNextIdx = 0;

			if( argStartIdx >= args.Length || args[argStartIdx].Length == 0 || args[argStartIdx][0] != '"' ) {
				output = "";
				return false;
			}

			string startSeg = args[argStartIdx].Substring( 1 );

			if( startSeg[startSeg.Length - 1] == '"' ) {
				argNextIdx = argStartIdx + 1;
				output = startSeg.Substring( 0, startSeg.Length - 1 );

				return true;
			}

			bool isTerminated = false;
			IList<string> segs = new List<string> { startSeg };
			int i;

			for( i = argStartIdx + 1; i < args.Length; i++ ) {
				string seg = args[i];

				if( seg.Length > 0 ) {
					if( seg[seg.Length - 1] == '"' ) {
						string subSeg = seg.Substring( 0, seg.Length - 1 );
						segs.Add( subSeg );
						isTerminated = true;
						break;
					}
				}

				segs.Add( seg );
			}

			argNextIdx = i + 1;
			output = string.Join( " ", segs );

			return isTerminated;
		}
	}
}
