using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to dictionaries.
	/// </summary>
	public static partial class DictionaryExtensions {
		/// <summary>
		/// Sets a value in a nested collection at a given dictionary key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="idx"></param>
		/// <param name="value"></param>
		public static void Set2D<TKey, TValue>( this IDictionary<TKey, List<TValue>> dict,
				TKey key, int idx, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<TValue>();
			}
			dict[key][idx] = value;
		}

		/// <summary>
		/// Sets a value in a nested collection at a given dictionary key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set2D<TKey, TValue>( this IDictionary<TKey, ISet<TValue>> dict, TKey key, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new HashSet<TValue>();
			}
			dict[key].Add( value );
		}

		/// <summary>
		/// Sets a value in a nested dictionary at a given dictionary key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <param name="value"></param>
		public static void Set2D<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2, TValue value ) {
			if( !dict.ContainsKey( key1 ) ) {
				dict[key1] = new Dictionary<TKey2, TValue>();
			}
			dict[key1][key2] = value;
		}

		/// <summary>
		/// Sets a value in a nested (sorted) dictionary at a given dictionary key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <param name="value"></param>
		public static void Set2DSorted<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2, TValue value ) {
			if( !dict.ContainsKey( key1 ) ) {
				dict[key1] = new SortedDictionary<TKey2, TValue>();
			}
			dict[key1][key2] = value;
		}
	}
}
