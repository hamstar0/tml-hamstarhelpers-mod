using HamstarHelpers.Libraries.Debug;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Libraries.DotNET {
	/// <summary>
	/// Assorted static "helper" functions pertaining to LINQ.
	/// </summary>
	public static class LINQLibraries {
		/// <summary>
		/// Wraps a LINQ `Select(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SafeSelect<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, TResult> selector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.Select( selector );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}
		/// <summary>
		/// Wraps a LINQ `Select(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SafeSelect<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, int, TResult> selector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.Select( selector );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}


		/// <summary>
		/// Wraps a LINQ `SelectMany(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SafeSelectMany<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.SelectMany( selector );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}
		/// <summary>
		/// Wraps a LINQ `SelectMany(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TCollection"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="collectionSelector"></param>
		/// <param name="resultSelector"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SafeSelectMany<TSource, TCollection, TResult>( this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.SelectMany( collectionSelector, resultSelector );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}
		/// <summary>
		/// Wraps a LINQ `SelectMany(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TCollection"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="collectionSelector"></param>
		/// <param name="resultSelector"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SafeSelectMany<TSource, TCollection, TResult>( this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.SelectMany( collectionSelector, resultSelector );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}
		/// <summary>
		/// Wraps a LINQ `SelectMany(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SafeSelectMany<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.SelectMany( selector );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}


		/// <summary>
		/// Wraps a LINQ `Where(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static IEnumerable<TSource> SafeWhere<TSource>( this IEnumerable<TSource> source, Func<TSource, int, bool> predicate ) {
			IEnumerable<TSource> output = null;
			try {
				output = source.Where( predicate );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return new List<TSource>();
			}
			return output;
		}
		/// <summary>
		/// Wraps a LINQ `Where(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static IEnumerable<TSource> SafeWhere<TSource>( this IEnumerable<TSource> source, Func<TSource, bool> predicate ) {
			IEnumerable<TSource> output = null;
			try {
				output = source.Where( predicate );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return new List<TSource>();
			}
			return output;
		}


		/// <summary>
		/// Wraps a LINQ `OrderBy(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="source"></param>
		/// <param name="keySelector"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> SafeOrderBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer ) {
			IOrderedEnumerable<TSource> output = null;
			try {
				output = source.OrderBy( keySelector, comparer );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return (new List<TSource>()).OrderBy( t=>t );
			}
			return output;
		}
		/// <summary>
		/// Wraps a LINQ `OrderBy(...)` call with an exception catcher that both reports the exception as a log error, and
		/// returns an empty collection to not interrupt program flow.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="source"></param>
		/// <param name="keySelector"></param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> SafeOrderBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector ) {
			IOrderedEnumerable<TSource> output = null;
			try {
				output = source.OrderBy( keySelector );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return ( new List<TSource>() ).OrderBy( t => t );
			}
			return output;
		}
	}
}
