using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.UserHelpers;
using HamstarHelpers.Internals.NetProtocols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.DataDumper {
	public static class DataDumper {
		private static object MyLock = new object();
		private static object MyDataStorekey = new object();
		private static int Dumps = 0;



		////////////////

		internal static IDictionary<string, Func<string>> GetDumpables() {
			bool success;
			var dumpables = (IDictionary<string, Func<string>>)DataStore.DataStore.Get( DataDumper.MyDataStorekey, out success );

			lock( DataDumper.MyLock ) {
				if( !success ) {
					dumpables = new Dictionary<string, Func<string>>();
					DataStore.DataStore.Set( DataDumper.MyDataStorekey, dumpables );
				}
			}

			return dumpables;
		}


		////////////////

		internal static string GetFileName( string prefix ) {
			string netmode;
			switch( Main.netMode ) {
			case 0:
				netmode = "single";
				break;
			case 1:
				netmode = "client";
				break;
			case 2:
				netmode = "server";
				break;
			default:
				netmode = "!";
				break;
			}

			double now_seconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;

			string now_seconds_whole = ( (int)now_seconds ).ToString( "D6" );
			string now_seconds_decimal = ( now_seconds - (int)now_seconds ).ToString( "N2" );
			string now = now_seconds_whole + "." + ( now_seconds_decimal.Length > 2 ? now_seconds_decimal.Substring( 2 ) : now_seconds_decimal );

			return prefix + "_"+netmode +"_"+now + "_dump.txt";
		}

		public static string GetRelativePath() {
			return "Logs" + Path.DirectorySeparatorChar + "Dumps";
		}


		////////////////

		private static void PrepareDir() {
			string full_dir = Main.SavePath + Path.DirectorySeparatorChar + DataDumper.GetRelativePath();

			try {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + "Logs" );
				Directory.CreateDirectory( full_dir );
			} catch( IOException e ) {
				throw new IOException( "Failed to prepare directory: " + full_dir, e );
			}
		}


		////////////////

		public static void SetDumpSource( string name, Func<string> dump ) {
			var dumpables = DataDumper.GetDumpables();

			lock( DataDumper.MyLock ) {
				dumpables[ name ] = dump;
			}
		}


		////////////////

		[Obsolete("use DumpToFile(out string)", true)]
		public static string DumpToFile( out bool success ) {
			string file_name;
			success = DataDumper.DumpToFile( out file_name );
			return file_name;
		}

		public static bool DumpToFile( out string file_name ) {
			string data;
			IDictionary<string, Func<string>> dumpables = DataDumper.GetDumpables();

			Func<KeyValuePair<string, Func<string>>, string> getKey = ( kv ) => {
				try { return kv.Value(); }
				catch { return "ERROR"; }
			};

			lock( DataDumper.MyLock ) {
				data = string.Join( "\r\n", dumpables
					.ToDictionary( kv => kv.Key, getKey )
					.Select( kv=>kv.Key+":\r\n"+kv.Value )
				);
			}
			
			file_name = DataDumper.GetFileName( (DataDumper.Dumps++)+"" );
			string rel_path = DataDumper.GetRelativePath();
			string full_folder = Main.SavePath + Path.DirectorySeparatorChar + rel_path;
			string full_path = full_folder + Path.DirectorySeparatorChar + file_name;

			DataDumper.PrepareDir();
			bool success = FileHelpers.SaveTextFile( data, full_path, false, false );

			if( success ) {
				// Allow admins to dump on behalf of server, also
				if( Main.netMode == 1 ) {
					if( ModHelpersMod.Instance.Config.DebugModeDumpAlsoServer || UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer ) ) {
						PacketProtocolRequestToServer.QuickRequest<DataDumpProtocol>();
					}
				}
			}

			return success;
		}
	}
}
