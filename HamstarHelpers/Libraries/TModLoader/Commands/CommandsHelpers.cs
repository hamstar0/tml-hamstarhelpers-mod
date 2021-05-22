using System.Collections.Generic;


namespace HamstarHelpers.Libraries.TModLoader.Commands {
	/// <summary>
	/// Assorted static "helper" functions pertaining to (chat or console) commands.
	/// </summary>
	public static class CommandsLibraries {
		/// <summary>
		/// Parses the next quote-wrapped input string from a sequence of input argument strings.
		/// </summary>
		/// <param name="args">Input arguments. Typically an array split by spaces.</param>
		/// <param name="argStartIdx">Position in the `args` array to begin parsing.</param>
		/// <param name="argNextIdx">Position after last checked argument.</param>
		/// <param name="output">Parsed argument (minus quotes).</param>
		/// <returns>`true` if a quote wrapped string was found and properly formed.</returns>
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
