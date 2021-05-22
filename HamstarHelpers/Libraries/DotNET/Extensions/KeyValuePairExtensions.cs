using System;
using System.Collections.Generic;


namespace HamstarHelpers.Libraries.DotNET.Extensions {
	/// <summary>
	/// Extensions for `KeyValuePair`.
	/// </summary>
	public static class KeyValuePairExtensions {
		/// <summary>
		/// Enables value tuples `foreach` for dictionaries, rather than just `KeyValuePair`.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="this"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Deconstruct<TKey, TValue>( this KeyValuePair<TKey, TValue> @this, out TKey key, out TValue value ) {
			key = @this.Key;
			value = @this.Value;
		}
	}
}
