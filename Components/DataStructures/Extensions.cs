using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.DataStructures {
	public static class ArrayExtensions {
		public static T[] RemoveAt<T>( this T[] source, int index ) {
			T[] dest = new T[source.Length - 1];
			if( index > 0 )
				Array.Copy( source, 0, dest, 0, index );

			if( index < source.Length - 1 )
				Array.Copy( source, index + 1, dest, index, source.Length - index - 1 );

			return dest;
		}
	}




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


		////////////////

		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, short>> dict,
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

		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, int>> dict,
				TKey1 key1, TKey2 key2, int value ) {
			if( !dict.ContainsKey( key1 ) ) {
				dict[key1] = new Dictionary<TKey2, int>();
			}
			if( dict[key1].ContainsKey(key2) ) {
				dict[key1][key2] += value;
			} else {
				dict[key1][key2] = value;
			}
		}

		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, float>> dict,
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

		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, double>> dict,
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
