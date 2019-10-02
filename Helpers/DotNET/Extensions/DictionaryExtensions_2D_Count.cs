using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to dictionaries.
	/// </summary>
	public static partial class DictionaryExtensions {
		/// <summary>
		/// Counts all entries in a 2D dictionary.
		/// </summary>
		/// <typeparam name="TKey1"></typeparam>
		/// <typeparam name="TKey2"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <returns></returns>
		public static int Count2D<TKey1, TKey2, TValue>(
				this IDictionary<TKey1, IDictionary<TKey2, TValue>> dict ) {
			int count = 0;

			foreach( (TKey1 k, IDictionary<TKey2, TValue> v) in dict ) {
				count += v.Count;
			}

			return count;
		}

		/// <summary>
		/// Counts all entries in a 2D dictionary.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <returns></returns>
		public static int Count2D<TKey, TValue>(
				this IDictionary<TKey, IEnumerable<TValue>> dict ) {
			int count = 0;

			foreach( (TKey k, IEnumerable<TValue> v) in dict ) {
				count += v.Count();
			}

			return count;
		}

		/// <summary>
		/// Counts all entries in a 2D dictionary.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <returns></returns>
		public static int Count2D<TKey, TValue>(
				this IDictionary<TKey, IList<TValue>> dict ) {
			int count = 0;

			foreach( (TKey k, IList<TValue> v) in dict ) {
				count += v.Count;
			}

			return count;
		}

		/// <summary>
		/// Counts all entries in a 2D dictionary.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict"></param>
		/// <returns></returns>
		public static int Count2D<TKey, TValue>(
				this IDictionary<TKey, ISet<TValue>> dict ) {
			int count = 0;

			foreach( (TKey k, ISet<TValue> v) in dict ) {
				count += v.Count;
			}

			return count;
		}
	}
}
