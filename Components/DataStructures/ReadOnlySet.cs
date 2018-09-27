using System;
using System.Collections;
using System.Collections.Generic;


namespace HamstarHelpers.Components.DataStructures {
	public class ReadOnlySet<T> : ISet<T> {
		private ISet<T> MySet;


		public ReadOnlySet( ISet<T> myset ) => this.MySet = myset ?? throw new Exception("Base set not defined.");

		public int Count => this.MySet.Count;

		public bool IsReadOnly => true;

		void ICollection<T>.Add( T item ) => throw new NotImplementedException();

		public void Clear() => throw new NotImplementedException();

		public bool Contains( T item ) => this.MySet.Contains( item );

		public void CopyTo( T[] array, int array_idx ) => this.MySet.CopyTo( array, array_idx );

		public bool Remove( T item ) => throw new NotImplementedException();


		public bool Add( T item ) => throw new NotImplementedException();

		public void ExceptWith( IEnumerable<T> other ) => throw new NotImplementedException();

		public void IntersectWith( IEnumerable<T> other ) => throw new NotImplementedException();

		public bool IsProperSubsetOf( IEnumerable<T> other ) => this.MySet.IsProperSubsetOf( other );

		public bool IsProperSupersetOf( IEnumerable<T> other ) => this.MySet.IsProperSupersetOf( other );

		public bool IsSubsetOf( IEnumerable<T> other ) => this.MySet.IsSubsetOf( other );

		public bool IsSupersetOf( IEnumerable<T> other ) => this.MySet.IsSupersetOf( other );

		public bool Overlaps( IEnumerable<T> other ) => this.MySet.Overlaps( other );

		public bool SetEquals( IEnumerable<T> other ) => this.MySet.SetEquals( other );

		public void SymmetricExceptWith( IEnumerable<T> other ) => throw new NotImplementedException();

		public void UnionWith( IEnumerable<T> other ) => throw new NotImplementedException();

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.MySet.GetEnumerator();

		public IEnumerator GetEnumerator() => this.MySet.GetEnumerator();
	}
}
