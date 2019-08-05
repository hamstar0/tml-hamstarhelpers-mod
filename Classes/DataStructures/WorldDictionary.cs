using System.Collections;
using System.Collections.Generic;
using Terraria.DataStructures;


namespace HamstarHelpers.Classes.DataStructures {
	/// <summary>
	/// Implements a dictionary for mapping data to world coordinates.
	/// 
	/// Credit: Rartrin (via. DataCore)
	/// </summary>
	public class WorldDictionary<T> : IDictionary<Point16, T> {
		private readonly IDictionary<Point16, T> data = new Dictionary<Point16, T>();


		/// <summary>
		/// Standard getter and setter (via. 2D, uint16 key).
		/// </summary>
		public T this[ushort x, ushort y] {
			get { return data[new Point16( x, y )]; }
			set { data[new Point16( x, y )] = value; }
		}
		/// <summary>
		/// Standard getter and setter (via. Point16 key).
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		T IDictionary<Point16, T>.this[Point16 key] {
			get { return data[key]; }
			set { data[key] = value; }
		}
		
		/// <summary>
		/// Returns collection of points as keys.
		/// </summary>
		public ICollection<Point16> Keys => data.Keys;
		/// <summary>
		/// Return collection of values.
		/// </summary>
		public ICollection<T> Values => data.Values;
		/// <summary>
		/// Return count of data mapped world points.
		/// </summary>
		public int Count => data.Count;


		/// <summary>
		/// Adds an element with the provided key and value to the collection
		/// </summary>
		/// <param name="x">Y world coordinate.</param>
		/// <param name="y">Y world coordinate.</param>
		/// <param name="value"></param>
		/// <exception cref="System.ArgumentNullException">key is null.</exception>
		/// <exception cref="System.ArgumentException">An element with the same key already exists in the collection.</exception>
		public void Add( ushort x, ushort y, T value ) => data.Add( new Point16( x, y ), value );
		/// <summary>
		/// Determines whether the System.Collections.Generic.IDictionary`2 contains an element with the specified key.
		/// </summary>
		/// <param name="x">Y world coordinate.</param>
		/// <param name="y">Y world coordinate.</param>
		/// <returns>true if the collection contains an element with the key; otherwise, false.</returns>
		/// <exception cref="System.ArgumentNullException">key is null.</exception>
		public bool ContainsPoint( ushort x, ushort y ) => data.ContainsKey( new Point16( x, y ) );
		/// <summary>
		/// Removes the element with the specified key from the collection.
		/// </summary>
		/// <param name="x">Y world coordinate.</param>
		/// <param name="y">Y world coordinate.</param>
		/// <returns>true if the element is successfully removed; otherwise, false. This method also  returns false if key was not found in the
		/// original collection.</returns>
		public bool Remove( ushort x, ushort y ) => data.Remove( new Point16( x, y ) );
		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="x">Y world coordinate.</param>
		/// <param name="y">Y world coordinate.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the
		/// default value for the type of the value parameter. This parameter is passed uninitialized</param>
		/// <returns>true if the object that implements System.Collections.Generic.IDictionary`2 contains an element with the specified key;
		/// otherwise, false.</returns>
		/// <exception cref="System.ArgumentNullException">key is null.</exception>
		public bool TryGetValue( ushort x, ushort y, out T value ) => data.TryGetValue( new Point16( x, y ), out value );
		/// <summary>
		/// Removes all items in the collection.
		/// </summary>
		public void Clear() => data.Clear();
		/// <summary>Returns an enumerator that iterates through the collection</summary>
		/// <returns>Returns an enumerator that iterates through the collection</returns>
		public IEnumerator<KeyValuePair<Point16, T>> GetEnumerator() => data.GetEnumerator();

		/// @private
		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
		/// @private
		bool ICollection<KeyValuePair<Point16, T>>.IsReadOnly => data.IsReadOnly;
		/// @private
		void ICollection<KeyValuePair<Point16, T>>.Add( KeyValuePair<Point16, T> item ) => data.Add( item );
		/// @private
		bool ICollection<KeyValuePair<Point16, T>>.Contains( KeyValuePair<Point16, T> item ) => data.Contains( item );
		/// @private
		void ICollection<KeyValuePair<Point16, T>>.CopyTo( KeyValuePair<Point16, T>[] array, int arrayIndex ) => data.CopyTo( array, arrayIndex );
		/// @private
		bool ICollection<KeyValuePair<Point16, T>>.Remove( KeyValuePair<Point16, T> item ) => data.Remove( item );
		/// @private
		void IDictionary<Point16, T>.Add( Point16 key, T value ) => data.Add( key, value );
		/// @private
		bool IDictionary<Point16, T>.ContainsKey( Point16 key ) => data.ContainsKey( key );
		/// @private
		bool IDictionary<Point16, T>.Remove( Point16 key ) => data.Remove( key );
		/// @private
		bool IDictionary<Point16, T>.TryGetValue( Point16 key, out T value ) => data.TryGetValue( key, out value );
	}
}
