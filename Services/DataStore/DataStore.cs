using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;


namespace HamstarHelpers.Services.DataStore {
	public class DataStore {
		private static object MyLock = new object();


		////////////////

		public static bool Has( object key ) {
			lock( DataStore.MyLock ) {
				return ModHelpersMod.Instance.DataStore.Data.ContainsKey( key );
			}
		}

		public static bool Get<T>( object key, out T val ) {
			val = default(T);
			object raw_val = null;
			bool success = false;

			lock( DataStore.MyLock ) {
				success = ModHelpersMod.Instance.DataStore.Data.TryGetValue( key, out raw_val );

				if( !( raw_val is T ) ) {
					success = false;
				} else {
					val = (T)raw_val;
				}
			}
			
			return success;
		}

		public static void Set( object key, object val ) {
			lock( DataStore.MyLock ) {
				ModHelpersMod.Instance.DataStore.Data[ key ] = val;
			}
		}

		public static bool Remove( object key ) {
			return ModHelpersMod.Instance.DataStore.Data.Remove( key );
		}



		////////////////

		private IDictionary<object, object> Data = new Dictionary<object, object>();

		internal DataStore() { }
	}
}
