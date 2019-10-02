using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to dictionaries.
	/// </summary>
	public static partial class DictionaryExtensions {
		/// <summary>
		/// Removes a value in a nested collection at a given dictionary key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="idx"></param>
		/// <returns></returns>
		public static bool Remove2D<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, int idx ) {
			bool removed = false;

			if( dict.ContainsKey( key ) ) {
				dict[key].RemoveAt( idx );
				removed = true;

				if( dict[key].Count == 0 ) {
					dict.Remove( key );
				}
			}

			return removed;
		}

		/// <summary>
		/// Removes a value in a nested collection at a given dictionary key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool Remove2D<TKey, TValue>( this IDictionary<TKey, ISet<TValue>> dict, TKey key, TValue val ) {
			bool removed = false;

			if( dict.ContainsKey( key ) ) {
				dict[key].Remove( val );
				removed = true;

				if( dict[key].Count == 0 ) {
					dict.Remove( key );
				}
			}

			return removed;
		}

		/// <summary>
		/// Removes a value in a nested dictionary at a given dictionary key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <returns></returns>
		public static bool Remove2D<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2 ) {
			bool removed = false;

			if( dict.ContainsKey( key1 ) ) {
				removed = dict[key1].Remove( key2 );

				if( dict[key1].Count == 0 ) {
					dict.Remove( key1 );
				}
			}

			return removed;
		}
	}
}
