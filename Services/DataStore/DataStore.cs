using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Services.DataStore {
	public partial class DataStore {
		private static object MyLock = new object();



		////////////////

		public static bool Has( object key ) {
			lock( DataStore.MyLock ) {
				return ModHelpersMod.Instance.DataStore.Data.ContainsKey( key );
			}
		}

		public static bool Get<T>( object key, out T val ) {
			val = default(T);
			object rawVal = null;
			bool success = false;

			lock( DataStore.MyLock ) {
				success = ModHelpersMod.Instance.DataStore.Data.TryGetValue( key, out rawVal );

				if( !( rawVal is T ) ) {
					success = false;
				} else {
					val = (T)rawVal;
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
		
		internal static IDictionary<object, object> GetAll() {
			var ds = ModHelpersMod.Instance.DataStore;
			IDictionary<object, object> clone;

			lock( DataStore.MyLock ) {
				clone = ds.Data.ToDictionary( kv => kv.Key, kv => kv.Value );
				clone.Remove( DataDumper.DataDumper.MyDataStorekey );
			}
			return clone;
		}


		////////////////

		public static void Add( object key, double val ) {
			var ds = ModHelpersMod.Instance.DataStore;

			lock( DataStore.MyLock ) {
				if( !ds.Data.ContainsKey( key ) ) {
					ds.Data[key] = val;
				} else {
					Type dst = ds.Data[key].GetType();

					if( !dst.IsValueType || Type.GetTypeCode(dst) == TypeCode.Boolean ) {
						throw new HamstarException( "Cannot use Add with non-numeric values." );
					}
					ds.Data[ key ] = (double)ds.Data[key] + val;
				}
			}
		}
	}
}
