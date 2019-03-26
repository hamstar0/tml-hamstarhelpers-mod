using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.DataStructures {
	public static class DictionaryExtensions {
		[Obsolete( "use `GetOrDefault(...)`", true)]
		public static TValue HardGet<TKey, TValue>( this IDictionary<TKey, TValue> dict, TKey key ) {
			return DictionaryExtensions.GetOrDefault( dict, key );
		}

		public static TValue GetOrDefault<TKey, TValue>( this IDictionary<TKey, TValue> dict, TKey key ) {
			TValue val;
			if( dict.TryGetValue( key, out val ) ) {
				return val;
			}
			return default( TValue );
		}


		public static bool TryGetValue2D<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2, out TValue value ) {
			IDictionary<TKey2, TValue> dict2;
			if( !dict.TryGetValue(key1, out dict2) ) {
				value = default(TValue);
				return false;
			}

			return dict2.TryGetValue( key2, out value );
		}


		public static TValue Get2DOrDefault<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2 ) {
			if( !dict.ContainsKey( key1 ) ) {
				return default(TValue);
			}
			if( !dict[key1].ContainsKey( key2 ) ) {
				return default( TValue );
			}
			return dict[key1][key2];
		}

		public static void Set2D<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2, TValue value ) {
			if( !dict.ContainsKey(key1) ) {
				dict[key1] = new Dictionary<TKey2, TValue>();
			}
			dict[key1][key2] = value;
		}

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
