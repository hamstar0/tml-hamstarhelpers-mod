using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.DataStructures {
	public static class DictionaryExtensions {
		public static TValue HardGet<TKey, TValue>( this IDictionary<TKey, TValue> dict, TKey key ) {
			TValue val;
			if( dict.TryGetValue( key, out val ) ) {
				return val;
			}
			return default( TValue );
		}
	}
}
