using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.GlobalDataStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;

namespace HamstarHelpers.Helpers.DebugHelpers {
	public static class DataDumpHelpers {
		private static object MyLock = new object();
		private static object MyDataStorekey = new object();



		////////////////

		internal static IDictionary<string, Func<string>> GetDumpables() {
			bool success;
			var dumpables = (IDictionary<string, Func<string>>)DataStore.Get( DataDumpHelpers.MyDataStorekey, out success );

			lock( DataDumpHelpers.MyLock ) {
				if( !success ) {
					dumpables = new Dictionary<string, Func<string>>();
					DataStore.Set( DataDumpHelpers.MyDataStorekey, dumpables );
				}
			}

			return dumpables;
		}


		////////////////

		internal static string GetFileName() {
			string netmode;
			switch( Main.netMode ) {
			case 0:
				netmode = "single";
				break;
			case 1:
				netmode = "";
				break;
			case 2:
				netmode = "";
				break;
			default:
				netmode = "!";
				break;
			}

			double now_seconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;

			string now_seconds_whole = ( (int)now_seconds ).ToString( "D6" );
			string now_seconds_decimal = ( now_seconds - (int)now_seconds ).ToString( "N2" );
			string now = now_seconds_whole + "." + ( now_seconds_decimal.Length > 2 ? now_seconds_decimal.Substring( 2 ) : now_seconds_decimal );

			return netmode+"_"+now + "_dump.json";
		}

		public static void SetDumpSource( string name, Func<string> dump ) {
			var dumpables = DataDumpHelpers.GetDumpables();

			lock( DataDumpHelpers.MyLock ) {
				dumpables[ name ] = dump;
			}
		}


		////////////////

		public static string DumpToFile() {
			IDictionary<string, string> data;
			var dumpables = DataDumpHelpers.GetDumpables();

			Func<KeyValuePair<string, Func<string>>, string> getKey = ( kv ) => {
				try {
					string val = kv.Value();
					return val;
				} catch {
					return "ERROR";
				}
			};

			lock( DataDumpHelpers.MyLock ) {
				data = dumpables.ToDictionary( kv => kv.Key, getKey );
			}
			
			string file_name = DataDumpHelpers.GetFileName();
			string rel_path = "Logs" + Path.DirectorySeparatorChar + "Dumps";
			var json_file = new JsonConfig<IDictionary<string, string>>( file_name, rel_path, data );

			json_file.SaveFile();

			// Allow admins to dump on behalf of server, also
			if( Main.netMode == 1 ) {
				if( UserHelpers.)
			}

			return file_name;
		}
	}
}
