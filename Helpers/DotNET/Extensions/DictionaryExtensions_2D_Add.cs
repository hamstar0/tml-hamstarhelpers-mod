using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to dictionaries.
	/// </summary>
	public static partial class DictionaryExtensions {
		/// <summary>
		/// Adds to a value to a nested collection in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="idx"></param>
		/// <param name="value"></param>
		public static void AddToValue2D<TKey>( this IDictionary<TKey, List<short>> dict, TKey key, int idx, short value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<short>( idx + 1 );
			}
			dict[key][idx] += value;
		}
		/// <summary>
		/// Adds to a value to a nested dictionary in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <param name="value"></param>
		public static void AddToValue2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, short>> dict,
				TKey1 key1, TKey2 key2, short value ) {
			if( !dict.ContainsKey( key1 ) ) {
				dict[key1] = new Dictionary<TKey2, short>();
			}
			if( dict[key1].ContainsKey( key2 ) ) {
				dict[key1][key2] += value;
			} else {
				dict[key1][key2] = value;
			}
		}

		/// <summary>
		/// Adds to a value to a nested collection in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="idx"></param>
		/// <param name="value"></param>
		public static void AddToValue2D<TKey>( this IDictionary<TKey, List<int>> dict, TKey key, int idx, int value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<int>( idx + 1 );
			}
			dict[key][idx] += value;
		}
		/// <summary>
		/// Adds to a value to a nested dictionary in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <param name="value"></param>
		public static void AddToValue2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, int>> dict,
				TKey1 key1, TKey2 key2, int value ) {
			if( !dict.ContainsKey( key1 ) ) {
				dict[key1] = new Dictionary<TKey2, int>();
			}
			if( dict[key1].ContainsKey( key2 ) ) {
				dict[key1][key2] += value;
			} else {
				dict[key1][key2] = value;
			}
		}

		/// <summary>
		/// Adds to a value to a nested collection in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="idx"></param>
		/// <param name="value"></param>
		public static void AddToValue2D<TKey>( this IDictionary<TKey, List<float>> dict, TKey key, int idx, float value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<float>( idx + 1 );
			}
			dict[key][idx] += value;
		}
		/// <summary>
		/// Adds to a value to a nested dictionary in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <param name="value"></param>
		public static void AddToValue2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, float>> dict,
				TKey1 key1, TKey2 key2, float value ) {
			if( !dict.ContainsKey( key1 ) ) {
				dict[key1] = new Dictionary<TKey2, float>();
			}
			if( dict[key1].ContainsKey( key2 ) ) {
				dict[key1][key2] += value;
			} else {
				dict[key1][key2] = value;
			}
		}

		/// <summary>
		/// Adds to a value to a nested collection in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="idx"></param>
		/// <param name="value"></param>
		public static void AddToValue2D<TKey>( this IDictionary<TKey, List<double>> dict, TKey key, int idx, double value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<double>( idx + 1 );
			}
			dict[key][idx] += value;
		}
		/// <summary>
		/// Adds to a value to a nested dictionary in the given dictionary at the given key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <param name="value"></param>
		public static void AddToValue2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, double>> dict,
				TKey1 key1, TKey2 key2, double value ) {
			if( !dict.ContainsKey( key1 ) ) {
				dict[key1] = new Dictionary<TKey2, double>();
			}
			if( dict[key1].ContainsKey( key2 ) ) {
				dict[key1][key2] += value;
			} else {
				dict[key1][key2] = value;
			}
		}
	}
}
