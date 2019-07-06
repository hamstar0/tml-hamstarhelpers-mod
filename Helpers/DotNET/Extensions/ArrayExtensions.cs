using System;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to arrays.
	/// </summary>
	public static class ArrayExtensions {
		/// <summary>
		/// Removes an item at a given point in a source array.
		/// </summary>
		/// <typeparam name="T">Array type.</typeparam>
		/// <param name="source">Source array.</param>
		/// <param name="index">Index of item to remove from the array.</param>
		/// <returns>New array with removed item.</returns>
		public static T[] RemoveAt<T>( this T[] source, int index ) {
			T[] dest = new T[source.Length - 1];
			if( index > 0 )
				Array.Copy( source, 0, dest, 0, index );

			if( index < source.Length - 1 )
				Array.Copy( source, index + 1, dest, index, source.Length - index - 1 );

			return dest;
		}


		////////////////

		/// <summary>
		/// Makes a copy of a portion of an array.
		/// </summary>
		/// <typeparam name="T">Array type.</typeparam>
		/// <param name="source">Source array.</param>
		/// <param name="sourceIndex">Start index to begin copying.</param>
		/// <param name="destinationIndex">End position in source array to copy up to.</param>
		/// <returns>Resulting copied slice of the source array.</returns>
		public static T[] Copy<T>( this T[] source, int sourceIndex, int destinationIndex ) {
			//T[] dest = new T[ length - destinationIndex ];
			T[] dest = new T[ destinationIndex - sourceIndex ];

			int srcLen = source.Length;
			for( int i=sourceIndex; i<srcLen; i++ ) {
				dest[i - sourceIndex] = source[i];
			}
			
			return dest;
		}

		/// <summary>
		/// Makes a copy of a slice of a given array.
		/// </summary>
		/// <typeparam name="T">Array type.</typeparam>
		/// <param name="source">Source array.</param>
		/// <param name="length">Amount of array to copy.</param>
		/// <returns>Resulting copied slice of the source array.</returns>
		public static T[] Copy<T>( this T[] source, int length ) {
			T[] dest = new T[ length ];

			Array.Copy( source, dest, length );
			return dest;
		}

		/// <summary>
		/// Makes a full copy of a given array.
		/// </summary>
		/// <typeparam name="T">Array type.</typeparam>
		/// <param name="source">Source array.</param>
		/// <returns>Resulting copy of the source array.</returns>
		public static T[] Copy<T>( this T[] source ) {
			return source.Copy( source.Length );
		}
	}
}
