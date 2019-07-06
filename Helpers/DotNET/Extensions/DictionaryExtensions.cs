using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to dictionaries.
	/// </summary>
	public static partial class DictionaryExtensions {
		/// <summary>
		/// Gets a value from a dictionary by a given key, or else the value type's default, if none found.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static TValue GetOrDefault<TKey, TValue>( this IDictionary<TKey, TValue> dict, TKey key ) {
			TValue val;
			if( dict.TryGetValue( key, out val ) ) {
				return val;
			}
			return default( TValue );
		}


		////////////////

		/// <summary>
		/// Adds to a given value (or sets it, if not present) in a dictionary at a given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool AddOrSet<TKey>( this IDictionary<TKey, int> dict, TKey key, int value ) {
			if( dict.ContainsKey(key) ) {
				dict[key] += value;
				return true;
			} else {
				dict[key] = value;
				return false;
			}
		}
		/// <summary>
		/// Adds to a given value (or sets it, if not present) in a dictionary at a given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool AddOrSet<TKey>( this IDictionary<TKey, float> dict, TKey key, float value ) {
			if( dict.ContainsKey( key ) ) {
				dict[key] += value;
				return true;
			} else {
				dict[key] = value;
				return false;
			}
		}
		/// <summary>
		/// Adds to a given value (or sets it, if not present) in a dictionary at a given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool AddOrSet<TKey>( this IDictionary<TKey, double> dict, TKey key, double value ) {
			if( dict.ContainsKey( key ) ) {
				dict[key] += value;
				return true;
			} else {
				dict[key] = value;
				return false;
			}
		}
	}
}
