using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.DotNetHelpers.Extensions {
	public static partial class DictionaryExtensions {
		public static bool TryGetValue2D<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, int idx, out TValue value ) {
			IList<TValue> list2;
			if( !dict.TryGetValue( key, out list2 ) ) {
				value = default( TValue );
				return false;
			}
			value = list2[idx];
			return true;
		}
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

		public static TValue Get2DOrDefault<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, int idx ) {
			if( !dict.ContainsKey( key ) ) {
				return default( TValue );
			}
			return dict[key][idx];
		}
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

		public static void Set2D<TKey, TValue>( this IDictionary<TKey, List<TValue>> dict, TKey key, int idx, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<TValue>();
			}
			dict[key][idx] = value;
		}
		public static void Set2D<TKey1, TKey2, TValue>( this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict,
				TKey1 key1, TKey2 key2, TValue value ) {
			if( !dict.ContainsKey( key1 ) ) {
				dict[key1] = new Dictionary<TKey2, TValue>();
			}
			dict[key1][key2] = value;
		}

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

		public static void Append2D<TKey, TValue>( this IDictionary<TKey, TValue[]> dict, TKey key, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new TValue[] { value };
			} else {
				int len = dict[key].Length;
				dict[key] = new TValue[ len + 1 ];
				dict[key][len] = value;
			}
		}
		public static void Append2D<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<TValue>();
			}
			dict[key].Add( value );
		}
		public static void Append2D<TKey, TValue>( this IDictionary<TKey, ISet<TValue>> dict, TKey key, TValue value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new HashSet<TValue>();
			}
			dict[key].Add( value );
		}


		////////////////

		public static void Add2D<TKey>( this IDictionary<TKey, List<short>> dict, TKey key, int idx, short value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<short>( idx + 1 );
			}
			dict[key][idx] += value;
		}
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

		public static void Add2D<TKey>( this IDictionary<TKey, List<int>> dict, TKey key, int idx, int value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<int>( idx + 1 );
			}
			dict[key][idx] += value;
		}
		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, int>> dict,
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

		public static void Add2D<TKey>( this IDictionary<TKey, List<float>> dict, TKey key, int idx, float value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<float>( idx + 1 );
			}
			dict[key][idx] += value;
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

		public static void Add2D<TKey>( this IDictionary<TKey, List<double>> dict, TKey key, int idx, double value ) {
			if( !dict.ContainsKey( key ) ) {
				dict[key] = new List<double>( idx + 1 );
			}
			dict[key][idx] += value;
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
