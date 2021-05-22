using System;
using System.Collections.Generic;


namespace HamstarHelpers.Libraries.DotNET.Extensions {
	/// <summary>
	/// Extensions for collection types.
	/// </summary>
	public static class CollectionExtensions {
		/// <summary></summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">Collection item.</param>
		/// <param name="isBreak">If `true`, the loop breaks.</param>
		public delegate void IterationAction<T>( T item, ref bool isBreak );



		////////////////

		/// <summary>
		/// Shorthand for a `foreach` loop.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="action"></param>
		public static void ForEach<T>( this IEnumerable<T> source, Action<T> action ) {
			foreach( T t in source ) {
				action( t );
			}
		}

		/// <summary>
		/// Shorthand for a `foreach` loop. `action` can optionally indicate if the loop should break.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="action"></param>
		public static void ForEach<T>( this IEnumerable<T> source, IterationAction<T> action ) {
			foreach( T t in source ) {
				bool isBreak = false;

				action( t, ref isBreak );
				
				if( isBreak ) {
					break;
				}
			}
		}
	}
}
