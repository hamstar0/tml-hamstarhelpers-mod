using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.MiscHelpers {
	public static class DotNetHelpers {
		public static string DictToString( IDictionary<object, object> dict ) {
			return string.Join( ";", dict.Select( x => x.Key + "=" + x.Value ).ToArray() );
		}
	}
}
