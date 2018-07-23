using System;
using System.Collections;
using System.Collections.Generic;


namespace HamstarHelpers.Services.GlobalDataStore {
	public class LocalDataStore : IEnumerable<KeyValuePair<object, object>>, IEnumerable, ICollection {
		public static LocalDataStore Instance {
			get {
				return HamstarHelpersMod.Instance.LocalData;
			}
		}



		////////////////

		private IDictionary<object, object> Data = new Dictionary<object, object>();

		internal LocalDataStore() { }


		////////////////

		public int Count {
			get {
				return this.Data.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return this.Data.IsReadOnly;
			}
		}

		public object SyncRoot {
			get {
				throw new NotImplementedException();
			}
		}

		public bool IsSynchronized {
			get {
				throw new NotImplementedException();
			}
		}


		////////////////

		public object this[ object key ] {
			get {
				object val;

				if( this.Data.TryGetValue(key, out val) ) {
					return val;
				}
				return null;
			}
			
			set {
				this.Data[ key ] = value;
			}
		}


		////////////////

		public void Add( KeyValuePair<object, object> item ) {
			this.Data.Add( item );
		}

		public bool Contains( KeyValuePair<object, object> item ) {
			return this.Data.Contains( item );
		}

		public bool ContainsKey( object key ) {
			return this.Data.ContainsKey( key );
		}

		public bool Remove( KeyValuePair<object, object> item ) {
			return this.Data.Remove( item.Key );
		}

		public bool Remove( object key ) {
			return this.Data.Remove( key );
		}

		public IEnumerator<KeyValuePair<object, object>> GetEnumerator() {
			return this.Data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.Data.GetEnumerator();
		}

		public void CopyTo( KeyValuePair<object, object>[] array, int arrayIndex ) {
			this.Data.CopyTo( array, arrayIndex );
		}

		public void CopyTo( Array array, int index ) {
			this.Data.CopyTo( (KeyValuePair<object, object>[])array, index );
		}
	}
}
