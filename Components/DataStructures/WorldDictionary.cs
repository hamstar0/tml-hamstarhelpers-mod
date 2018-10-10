using System.Collections;
using System.Collections.Generic;
using Terraria.DataStructures;


namespace HamstarHelpers.Components.DataStructures {
	/// <summary>
	/// Implements a dictionary for mapping data to world coordinates. Credit: Rartrin (via. DataCore)
	/// </summary>
	public class WorldDictionary<T> : IDictionary<Point16, T> {
		private readonly IDictionary<Point16, T> data = new Dictionary<Point16, T>();


		public T this[ushort x, ushort y] {
			get { return data[new Point16( x, y )]; }
			set { data[new Point16( x, y )] = value; }
		}
		T IDictionary<Point16, T>.this[Point16 key] {
			get { return data[key]; }
			set { data[key] = value; }
		}
		
		public ICollection<Point16> Keys => data.Keys;
		public ICollection<T> Values => data.Values;
		public int Count => data.Count;


		public void Add( ushort x, ushort y, T value ) => data.Add( new Point16( x, y ), value );
		public bool ContainsPoint( ushort x, ushort y ) => data.ContainsKey( new Point16( x, y ) );
		public bool Remove( ushort x, ushort y ) => data.Remove( new Point16( x, y ) );
		public bool TryGetValue( ushort x, ushort y, out T value ) => data.TryGetValue( new Point16( x, y ), out value );
		public void Clear() => data.Clear();
		public IEnumerator<KeyValuePair<Point16, T>> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
		bool ICollection<KeyValuePair<Point16, T>>.IsReadOnly => data.IsReadOnly;
		void ICollection<KeyValuePair<Point16, T>>.Add( KeyValuePair<Point16, T> item ) => data.Add( item );
		bool ICollection<KeyValuePair<Point16, T>>.Contains( KeyValuePair<Point16, T> item ) => data.Contains( item );
		void ICollection<KeyValuePair<Point16, T>>.CopyTo( KeyValuePair<Point16, T>[] array, int arrayIndex ) => data.CopyTo( array, arrayIndex );
		bool ICollection<KeyValuePair<Point16, T>>.Remove( KeyValuePair<Point16, T> item ) => data.Remove( item );
		void IDictionary<Point16, T>.Add( Point16 key, T value ) => data.Add( key, value );
		bool IDictionary<Point16, T>.ContainsKey( Point16 key ) => data.ContainsKey( key );
		bool IDictionary<Point16, T>.Remove( Point16 key ) => data.Remove( key );
		bool IDictionary<Point16, T>.TryGetValue( Point16 key, out T value ) => data.TryGetValue( key, out value );
	}
}
