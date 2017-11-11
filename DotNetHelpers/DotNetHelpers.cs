using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.DotNetHelpers {
	public static class DotNetHelpers {
		public const double RadDeg = Math.PI / 180d;


		public static string DictToString( IDictionary<object, object> dict ) {
			return string.Join( ";", dict.Select( x => x.Key + "=" + x.Value ).ToArray() );
		}
	}
}
