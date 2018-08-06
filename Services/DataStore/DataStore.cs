using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;


namespace HamstarHelpers.Services.DataStore {
	public class DataStore {
		private static object MyLock = new object();


		////////////////

		public static bool Has( object key ) {
			lock( DataStore.MyLock ) {
				return HamstarHelpersMod.Instance.DataStore.Data.ContainsKey( key );
			}
		}

		public static object Get( object key, out bool success ) {
			object val = null;

			lock( DataStore.MyLock ) {
				success = HamstarHelpersMod.Instance.DataStore.Data.TryGetValue( key, out val );
			}
			
			return val;
		}

		public static void Set( object key, object val ) {
			lock( DataStore.MyLock ) {
				HamstarHelpersMod.Instance.DataStore.Data[ key ] = val;
			}
		}

		public static bool Remove( object key ) {
			return HamstarHelpersMod.Instance.DataStore.Data.Remove( key );
		}



		////////////////

		private IDictionary<object, object> Data = new Dictionary<object, object>();

		internal DataStore() { }
	}
}
