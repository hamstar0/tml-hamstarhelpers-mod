using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static class DotNetHelpers {
		public const double RadDeg = Math.PI / 180d;
		public const double DegRed = 180d / Math.PI;


		////////////////

		public static string StringifyDict<TKey, TValue>( IDictionary<TKey, TValue> dict ) {
			return string.Join( ";", dict.SafeSelect( x => x.Key + "=" + x.Value ) );
		}

		public static string Stringify( object obj, int charLimit=-1 ) {
			string output;

			if( obj == null ) {
				output = "null";
			} else if( obj.GetType().IsClass ) {
				output = JsonConvert.SerializeObject( obj ).ToString();
			} else {
				output = obj.ToString();
			}

			if( charLimit > 0 ) {
				if( output.Length > charLimit ) {
					output = output.Substring(0, charLimit) + "...";
				}
			}
			return output;
		}


		////////////////

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

			string lowerValue = value.ToLower();
			Boolean boolout;
			if( Boolean.TryParse( lowerValue, out boolout ) ) {
				if( lowerValue.Equals( boolout ) ) { return (object)boolout; }
			}

			return (object)value;
		}


		public static IEnumerable<T> FlagsToList<T>( int flags ) where T : struct, IConvertible {
			IEnumerable<T> values = Enum.GetValues( typeof( T ) ).Cast<T>();
			foreach( T val in values ) {
				if( ( flags & Convert.ToInt32( val ) ) != 0 ) {
					yield return val;
				}
			}
		}


		public static bool IsSubclassOfRawGeneric( Type genericType, Type isTypeOf ) {
			while( isTypeOf != null && isTypeOf != typeof( object ) ) {
				Type currType = isTypeOf.IsGenericType ?
						isTypeOf.GetGenericTypeDefinition() :
						isTypeOf;

				if( genericType == currType ) {
					return true;
				}
				isTypeOf = isTypeOf.BaseType;
			}

			return false;
		}
	}
}
