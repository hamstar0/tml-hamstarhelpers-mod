using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.DotNetHelpers {
	public static class DotNetHelpers {
		public const double RadDeg = Math.PI / 180d;


		public static string DictToString( IDictionary<object, object> dict ) {
			return string.Join( ";", dict.Select( x => x.Key + "=" + x.Value ).ToArray() );
		}


		public static object ParseToInferredPrimitiveType( string value ) {
			Int32 int32out;
			if( Int32.TryParse( value, out int32out ) ) {
				if( value.Equals( int32out.ToString() ) ) { return (object)int32out; }
			}
			Int64 int64out;
			if( Int64.TryParse( value, out int64out ) ) {
				if( value.Equals( int64out.ToString() ) ) { return (object)int64out; }
			}
			Single floatout;
			if( Single.TryParse( value, out floatout ) ) {
				if( value.Equals( floatout.ToString() ) ) { return (object)floatout; }
			}
			Double doubleout;
			if( Double.TryParse( value, out doubleout ) ) { return (object)doubleout; }

			string lower_value = value.ToLower();
			Boolean boolout;
			if( Boolean.TryParse( lower_value, out boolout ) ) {
				if( lower_value.Equals( boolout ) ) { return (object)boolout; }
			}

			return (object)value;
		}
	}
}
