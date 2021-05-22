using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Libraries.DotNET {
	/// <summary>
	/// Assorted static "helper" functions pertaining to .NET/C# functionality.
	/// </summary>
	public class DotNetLibraries {
		/// <summary>
		/// Radians-to-degrees.
		/// </summary>
		public const double RadDeg = Math.PI / 180d;
		/// <summary>
		/// Degrees-to-radians.
		/// </summary>
		public const double DegRed = 180d / Math.PI;



		////////////////

		/// <summary>
		/// Processes a dictionary into a string with some condensed formatting.
		/// </summary>
		/// <typeparam name="TKey">Dictionary key.</typeparam>
		/// <typeparam name="TValue">Dictionary value.</typeparam>
		/// <param name="dict">Dictionary.</param>
		/// <returns>Dictionary processed into a string.</returns>
		public static string StringifyDict<TKey, TValue>( IDictionary<TKey, TValue> dict ) {
			return string.Join( ";", dict.SafeSelect( x => x.Key + "=" + x.Value ) );
		}

		/// <summary>
		/// Processes an object as a (JSON) string. Applies a character limit, if specified.
		/// </summary>
		/// <param name="obj">Object to process.</param>
		/// <param name="charLimit">Size of string. Leave as -1 for no limit.</param>
		/// <returns>Object processed into a string.</returns>
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

		/// <summary>
		/// Parses a string into a primive type based on a best guess.
		/// </summary>
		/// <param name="value">Value to parse.</param>
		/// <returns>Parsed value object. Use `getType()` to know what it is.</returns>
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


		/// <summary>
		/// Separates each bit of a 32-bit int into a collection.
		/// </summary>
		/// <typeparam name="T">Enum type to represent flags.</typeparam>
		/// <param name="flags">Input bit flags as an int.</param>
		/// <returns>Collection of flags as enums.</returns>
		public static IEnumerable<T> FlagsToCollection<T>( int flags ) where T : struct, IConvertible {
			IEnumerable<T> values = Enum.GetValues( typeof( T ) ).Cast<T>();
			foreach( T val in values ) {
				if( ( flags & Convert.ToInt32(val) ) != 0 ) {
					yield return val;
				}
			}
		}


		/// <summary>
		/// Reports if a type is a subtype of another type using generics.
		/// </summary>
		/// <param name="parentTypeWithGenericParams">Parent class type (implicitly as a generic type) to check against.</param>
		/// <param name="givenType">Type to check with.</param>
		/// <returns>`true` if given type is a subclass of the parent type.</returns>
		public static bool IsSubclassOfRawGeneric( Type parentTypeWithGenericParams, Type givenType ) {
			while( givenType != null && givenType != typeof( object ) ) {
				Type currType = givenType.IsGenericType ?
						givenType.GetGenericTypeDefinition() :
						givenType;

				if( parentTypeWithGenericParams == currType ) {
					return true;
				}
				givenType = givenType.BaseType;
			}

			return false;
		}
	}
}
