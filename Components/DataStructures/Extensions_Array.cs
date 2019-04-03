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


		////////////////

		public static T[] Copy<T>( this T[] source, int sourceIndex, int destinationIndex, int length ) {
			T[] dest = new T[ length - destinationIndex ];

			int srcLen = source.Length;
			for( int i=sourceIndex; i<srcLen; i++ ) {
				dest[i - sourceIndex] = source[i];
			}
			
			return dest;
		}

		public static T[] Copy<T>( this T[] source, int length ) {
			T[] dest = new T[ length ];

			Array.Copy( source, dest, length );
			return dest;
		}

		public static T[] Copy<T>( this T[] source ) {
			return source.Copy( source.Length );
		}
	}
}
