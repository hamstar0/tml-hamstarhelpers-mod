using System;
using System.Collections;
using System.Collections.Generic;


namespace HamstarHelpers.DotNetHelpers.DataStructures {
	public class ReadOnlySet<T> : ISet<T> {
		private ISet<T> MySet;


		public ReadOnlySet( ISet<T> myset ) {
			if( myset == null ) { throw new Exception("Base set not defined."); }
			this.MySet = myset;
		}

		public int Count { get { return this.MySet.Count; } }

		public bool IsReadOnly { get { return true; } }

		void ICollection<T>.Add( T item ) { throw new NotImplementedException(); }

		public void Clear() { throw new NotImplementedException(); }

		public bool Contains( T item ) { return this.MySet.Contains( item ); }

		public void CopyTo( T[] array, int array_idx ) { this.MySet.CopyTo( array, array_idx ); }

		public bool Remove( T item ) { throw new NotImplementedException(); }


		public bool Add( T item ) { throw new NotImplementedException(); }

		public void ExceptWith( IEnumerable<T> other ) { throw new NotImplementedException(); }

		public void IntersectWith( IEnumerable<T> other ) { throw new NotImplementedException(); }

		public bool IsProperSubsetOf( IEnumerable<T> other ) { return this.MySet.IsProperSubsetOf( other ); }

		public bool IsProperSupersetOf( IEnumerable<T> other ) { return this.MySet.IsProperSupersetOf( other ); }

		public bool IsSubsetOf( IEnumerable<T> other ) { return this.MySet.IsSubsetOf( other ); }

		public bool IsSupersetOf( IEnumerable<T> other ) { return this.MySet.IsSupersetOf( other ); }

		public bool Overlaps( IEnumerable<T> other ) { return this.MySet.Overlaps( other ); }

		public bool SetEquals( IEnumerable<T> other ) { return this.MySet.SetEquals( other ); }

		public void SymmetricExceptWith( IEnumerable<T> other ) { throw new NotImplementedException(); }

		public void UnionWith( IEnumerable<T> other ) { throw new NotImplementedException(); }

		IEnumerator<T> IEnumerable<T>.GetEnumerator() { return this.MySet.GetEnumerator(); }

		public IEnumerator GetEnumerator() { return this.MySet.GetEnumerator(); }
	}
}
