using HamstarHelpers.Components.Errors;
using System;
using System.Collections;
using System.Collections.Generic;


namespace HamstarHelpers.Components.DataStructures {
	/// <summary></summary>
	/// <typeparam name="T"></typeparam>
	public interface IReadOnlySet<T> : ISet<T> { }




	/// <summary>
	/// Wraps a set to ensure a read-only interface. Some ISet interface members disabled.
	/// </summary>
	/// <typeparam name="T">Type stored in wrapped set.</typeparam>
	public class ReadOnlySet<T> : IReadOnlySet<T> {
		private ISet<T> MySet;


		/// <summary>
		/// Constructs a read-only ISet wrapper.
		/// </summary>
		/// <param name="myset">Set to wrap.</param>
		public ReadOnlySet( ISet<T> myset ) {
			if( myset == null ) {
				throw new ModHelpersException( "Base set not defined." );
			}
			this.MySet = myset;
		}

		/// <summary>
		/// Quantity of values in the set.
		/// </summary>
		public int Count => this.MySet.Count;

		/// <summary>
		/// Indicates the read-only state of the set.
		/// </summary>
		public bool IsReadOnly => true;

		/// @private
		void ICollection<T>.Add( T item ) { throw new NotImplementedException(); }

		/// @private
		public void Clear() { throw new NotImplementedException(); }

		/// <summary>
		/// Indicates if the given value exists inside the set.
		/// </summary>
		/// <param name="item">Value to check for existence of.</param>
		/// <returns>`true` if value found.</returns>
		public bool Contains( T item ) => this.MySet.Contains( item );

		/// <summary>
		/// Copies the set to a given array.
		/// </summary>
		/// <param name="array">Array to copy to</param>
		/// <param name="arrayIdx">Offset to begin copying at.</param>
		public void CopyTo( T[] array, int arrayIdx ) => this.MySet.CopyTo( array, arrayIdx );

		/// @private
		public bool Remove( T item ) { throw new NotImplementedException(); }


		/// @private
		public bool Add( T item ) { throw new NotImplementedException(); }

		/// @private
		public void ExceptWith( IEnumerable<T> other ) { throw new NotImplementedException(); }

		/// @private
		public void IntersectWith( IEnumerable<T> other ) { throw new NotImplementedException(); }

		/// <summary>
		/// Determines whether the current set is a proper (strict) subset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>true if the current set is a proper subset of other; otherwise, false.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		public bool IsProperSubsetOf( IEnumerable<T> other ) => this.MySet.IsProperSubsetOf( other );

		/// <summary>
		/// Determines whether the current set is a proper (strict) superset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>true if the current set is a proper superset of other; otherwise, false.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		public bool IsProperSupersetOf( IEnumerable<T> other ) => this.MySet.IsProperSupersetOf( other );

		/// <summary>
		/// Determines whether a set is a subset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>true if the current set is a subset of other; otherwise, false.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		public bool IsSubsetOf( IEnumerable<T> other ) => this.MySet.IsSubsetOf( other );

		/// <summary>
		/// Determines whether the current set is a superset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>true if the current set is a superset of other; otherwise, false.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		public bool IsSupersetOf( IEnumerable<T> other ) => this.MySet.IsSupersetOf( other );

		/// <summary>
		/// Determines whether the current set overlaps with the specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>true if the current set and other share at least one common element; otherwise, false.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		public bool Overlaps( IEnumerable<T> other ) => this.MySet.Overlaps( other );

		/// <summary>
		/// Determines whether the current set and the specified collection contain the same elements.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>true if the current set is equal to other; otherwise, false.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		public bool SetEquals( IEnumerable<T> other ) => this.MySet.SetEquals( other );

		/// @private
		public void SymmetricExceptWith( IEnumerable<T> other ) { throw new NotImplementedException(); }

		/// @private
		public void UnionWith( IEnumerable<T> other ) { throw new NotImplementedException(); }

		/// @private
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.MySet.GetEnumerator();

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		public IEnumerator GetEnumerator() => this.MySet.GetEnumerator();
	}
}
