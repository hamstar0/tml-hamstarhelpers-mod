using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to dictionaries.
	/// </summary>
	public static partial class DictionaryExtensions {
		/// <summary>
		/// Safely attempts to get a value (akin to the usual TryGetValue) by index within a nested collection in a
		/// dictionary at a given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="idx"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool TryGetValue2D<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, int idx, out TValue value ) {
			IList<TValue> list2;
			if( !dict.TryGetValue( key, out list2 ) ) {
				value = default( TValue );
				return false;
			}
			value = list2[idx];
			return true;
		}
		/// <summary>
		/// Safely attempts to get a value (akin to the usual TryGetValue) by key within a nested dictionary in the parent
		/// dictionary at a given key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool TryGetValue2D<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2, out TValue value ) {
			IDictionary<TKey2, TValue> dict2;
			if( !dict.TryGetValue( key1, out dict2 ) ) {
				value = default( TValue );
				return false;
			}

			return dict2.TryGetValue( key2, out value );
		}

		////

		/// <summary>
		/// Safely attempts to get a value (returning a default on failure) by index within a nested collection in a dictionary
		/// at a given key.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="idx"></param>
		/// <returns></returns>
		public static TValue Get2DOrDefault<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, int idx ) {
			if( !dict.ContainsKey( key ) ) {
				return default( TValue );
			}
			return dict[key][idx];
		}
		/// <summary>
		/// Safely attempts to get a value (returning a default on failure) by key within a nested dictionary in the parent
		/// dictionary at a given key.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <returns></returns>
		public static TValue Get2DOrDefault<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2 ) {
			if( !dict.ContainsKey( key1 ) ) {
				return default( TValue );
			}
			if( !dict[key1].ContainsKey( key2 ) ) {
				return default( TValue );
			}
			return dict[key1][key2];
		}


		////////////////

		/// <summary>
		/// Safely attempts to determine if a given a value exists within the 2D collection.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool Contains2D<TKey, TValue>( this IDictionary<TKey, ISet<TValue>> dict, TKey key, TValue val ) {
			if( !dict.ContainsKey( key ) ) {
				return false;
			}
			return dict[key].Contains( val );
		}

		/// <summary>
		/// Safely attempts to determine if a given a value exists within the 2D collection.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool Contains2D<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, TValue val ) {
			if( !dict.ContainsKey( key ) ) {
				return false;
			}
			return dict[key].Contains( val );
		}

		/// <summary>
		/// Safely attempts to determine if a given a value exists within the 2D collection.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool Contains2D<TKey, TValue>( this IDictionary<TKey, IEnumerable<TValue>> dict, TKey key, TValue val ) {
			if( !dict.ContainsKey( key ) ) {
				return false;
			}
			return dict[key].Contains( val );
		}
	}
}
