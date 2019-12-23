using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to dictionaries.
	/// </summary>
	public static partial class DictionaryExtensions {
		/// @private
		[Obsolete( "use AddToValue2D", true )]
		public static void Add2D<TKey>( this IDictionary<TKey, List<short>> dict, TKey key, int idx, short value ) {
			dict.AddToValue2D( key, idx, value );
		}
		/// @private
		[Obsolete( "use AddToValue2D", true )]
		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, short>> dict,
				TKey1 key1, TKey2 key2, short value ) {
			dict.AddToValue2D( key1, key2, value );
		}
		/// @private
		[Obsolete( "use AddToValue2D", true )]
		public static void Add2D<TKey>( this IDictionary<TKey, List<int>> dict, TKey key, int idx, int value ) {
			dict.AddToValue2D( key, idx, value );
		}
		/// @private
		[Obsolete( "use AddToValue2D", true )]
		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, int>> dict,
				TKey1 key1, TKey2 key2, int value ) {
			dict.AddToValue2D( key1, key2, value );
		}
		/// @private
		[Obsolete( "use AddToValue2D", true )]
		public static void Add2D<TKey>( this IDictionary<TKey, List<float>> dict, TKey key, int idx, float value ) {
			dict.AddToValue2D( key, idx, value );
		}
		/// @private
		[Obsolete( "use AddToValue2D", true )]
		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, float>> dict,
				TKey1 key1, TKey2 key2, float value ) {
			dict.AddToValue2D( key1, key2, value );
		}
		/// @private
		[Obsolete( "use AddToValue2D", true )]
		public static void Add2D<TKey>( this IDictionary<TKey, List<double>> dict, TKey key, int idx, double value ) {
			dict.AddToValue2D( key, idx, value );
		}
		/// @private
		[Obsolete( "use AddToValue2D", true )]
		public static void Add2D<TKey1, TKey2>( this IDictionary<TKey1, IDictionary<TKey2, double>> dict,
				TKey1 key1, TKey2 key2, double value ) {
			dict.AddToValue2D( key1, key2, value );
		}
	}
}
