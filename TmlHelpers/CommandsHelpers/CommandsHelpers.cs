using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.TmlHelpers.CommandsHelpers {
	public static class CommandsHelpers {
		public static string GetQuotedStringFromArgsAt( string[] args, int start_pos, out int next_arg_idx ) {
			next_arg_idx = -1;

			if( args[start_pos].Length == 0 || args[start_pos][0] != '"' ) {
				return "";
			}

			string start_seg = args[start_pos].Substring( 1 );
			if( args[start_pos][ start_seg.Length - 1 ] == '"' ) {
				next_arg_idx = start_pos + 1;
				return start_seg;
			}

			IList<string> segs = new List<string> { start_seg };

			for( int i=start_pos+1; i<args.Length; i++ ) {
				string seg = args[i];
				
				if( seg.Length > 0 && seg[ seg.Length-1 ] == '"' ) {
					string sub_seg = seg.Substring( 0, seg.Length - 1 );
					segs.Add( sub_seg );

					next_arg_idx = i + 1;

					return string.Join( " ", segs.ToArray() );
				}
				segs.Add( args[i] );
			}

			return "";
		}
	}
}
