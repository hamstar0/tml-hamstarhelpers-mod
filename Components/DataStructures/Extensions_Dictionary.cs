using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.DataStructures {
	public static partial class DictionaryExtensions {
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


		////////////////

		public static bool AddOrSet<TKey>( this IDictionary<TKey, int> dict, TKey key, int value ) {
			if( dict.ContainsKey(key) ) {
				dict[key] += value;
				return true;
			} else {
				dict[key] = value;
				return false;
			}
		}
		public static bool AddOrSet<TKey>( this IDictionary<TKey, float> dict, TKey key, float value ) {
			if( dict.ContainsKey( key ) ) {
				dict[key] += value;
				return true;
			} else {
				dict[key] = value;
				return false;
			}
		}
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
