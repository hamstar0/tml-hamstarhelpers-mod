using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Helpers.DotNET {
	/** <summary>Assorted static "helper" functions pertaining to LINQ.</summary> */
	public static class LINQHelpers {
		public static IEnumerable<TResult> SafeSelect<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, TResult> selector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.Select( selector );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}
		public static IEnumerable<TResult> SafeSelect<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, int, TResult> selector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.Select( selector );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}


		public static IEnumerable<TResult> SafeSelectMany<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.SelectMany( selector );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}
		public static IEnumerable<TResult> SafeSelectMany<TSource, TCollection, TResult>( this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.SelectMany( collectionSelector, resultSelector );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}
		public static IEnumerable<TResult> SafeSelectMany<TSource, TCollection, TResult>( this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.SelectMany( collectionSelector, resultSelector );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}
		public static IEnumerable<TResult> SafeSelectMany<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector ) {
			IEnumerable<TResult> output = null;
			try {
				output = source.SelectMany( selector );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<TResult>();
			}
			return output;
		}


		public static IEnumerable<TSource> SafeWhere<TSource>( this IEnumerable<TSource> source, Func<TSource, int, bool> predicate ) {
			IEnumerable<TSource> output = null;
			try {
				output = source.Where( predicate );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<TSource>();
			}
			return output;
		}
		public static IEnumerable<TSource> SafeWhere<TSource>( this IEnumerable<TSource> source, Func<TSource, bool> predicate ) {
			IEnumerable<TSource> output = null;
			try {
				output = source.Where( predicate );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<TSource>();
			}
			return output;
		}

		
		public static IOrderedEnumerable<TSource> SafeOrderBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer ) {
			IOrderedEnumerable<TSource> output = null;
			try {
				output = source.OrderBy( keySelector, comparer );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return (new List<TSource>()).OrderBy( t=>t );
			}
			return output;
		}
		public static IOrderedEnumerable<TSource> SafeOrderBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector ) {
			IOrderedEnumerable<TSource> output = null;
			try {
				output = source.OrderBy( keySelector );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return ( new List<TSource>() ).OrderBy( t => t );
			}
			return output;
		}
	}
}
