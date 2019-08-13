using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	public static class KeyValuePairExtensions {
		public static void Deconstruct<TKey, TValue>( this KeyValuePair<TKey, TValue> @this, out TKey key, out TValue value ) {
			key = @this.Key;
			value = @this.Value;
		}
	}
}
