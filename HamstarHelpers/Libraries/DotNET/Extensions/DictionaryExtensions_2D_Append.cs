using System;
using System.Collections.Generic;


namespace HamstarHelpers.Libraries.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to dictionaries.
	/// </summary>
	public static partial class DictionaryExtensions {
		/// <summary>
		/// Appends a value to a nested collection in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Append2D<TKey, TValue>( this IDictionary<TKey, TValue[]> dict, TKey key, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new TValue[] { value };
			} else {
				int len = dict[key].Length;
				dict[key] = new TValue[ len + 1 ];
				dict[key][len] = value;
			}
		}
		/// <summary>
		/// Appends a value to a nested collection in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Append2D<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<TValue>();
			}
			dict[key].Add( value );
		}
		/// <summary>
		/// Appends a value to a nested collection in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Append2D<TKey, TValue>( this IDictionary<TKey, ISet<TValue>> dict, TKey key, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new HashSet<TValue>();
			}
			dict[key].Add( value );
		}
	}
}
