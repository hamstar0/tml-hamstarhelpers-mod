using HamstarHelpers.Components.PacketProtocol;
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
		internal static object MyDataStorekey = new object();
		private static int Dumps = 0;



		////////////////

		public static bool DumpToLocalFile( string data, out string fileName ) {
			fileName = DataDumper.GetFileName( (DataDumper.Dumps++) + "" );
			string relPath = DataDumper.GetRelativePath();
			string fullFolder = Main.SavePath + Path.DirectorySeparatorChar + relPath;
			string fullPath = fullFolder + Path.DirectorySeparatorChar + fileName;

			DataDumper.PrepareDir();
			return FileHelpers.SaveTextFile( data, fullPath, false, false );
		}

		////////////////

		internal static IDictionary<string, Func<string>> GetDumpables() {
			IDictionary<string, Func<string>> dumpables;
			bool success = DataStore.DataStore.Get( DataDumper.MyDataStorekey, out dumpables );

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
				netmode = "client("+Main.LocalPlayer.name+"_"+Main.myPlayer+")";
				break;
			case 2:
				netmode = "server";
				break;
			default:
				netmode = "!";
				break;
			}

			double nowSeconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;

			string nowSecondsWhole = ( (int)nowSeconds ).ToString( "D6" );
			string nowSecondsDecimal = ( nowSeconds - (int)nowSeconds ).ToString( "N2" );
			string now = nowSecondsWhole + "." + ( nowSecondsDecimal.Length > 2 ? nowSecondsDecimal.Substring( 2 ) : nowSecondsDecimal );

			return prefix + "_"+netmode +"_"+now + "_dump.txt";
		}

		public static string GetRelativePath() {
			return "Logs" + Path.DirectorySeparatorChar + "Dumps";
		}


		////////////////

		private static void PrepareDir() {
			string fullDir = Main.SavePath + Path.DirectorySeparatorChar + DataDumper.GetRelativePath();

			try {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + "Logs" );
				Directory.CreateDirectory( fullDir );
			} catch( IOException e ) {
				throw new IOException( "Failed to prepare directory: " + fullDir, e );
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

		public static bool DumpToFile( out string fileName ) {
			string data;
			IDictionary<string, Func<string>> dumpables = DataDumper.GetDumpables();

			Func<KeyValuePair<string, Func<string>>, string> getKey = kv => {
				try { return kv.Value(); }
				catch( Exception e ) { return "ERROR: "+e.Message; }
			};

			lock( DataDumper.MyLock ) {
				data = string.Join( "\r\n", dumpables
					.ToDictionary( kv => kv.Key, getKey )
					.SafeSelect( kv=>kv.Key+":\r\n"+kv.Value )
				);
			}
			
			bool success = DataDumper.DumpToLocalFile( data, out fileName );

			if( success ) {
				// Allow admins to dump on behalf of server, also
				if( Main.netMode == 1 ) {
					if( ModHelpersMod.Instance.Config.DebugModeDumpAlsoServer || UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer ) ) {
						PacketProtocolRequestToServer.QuickRequest<DataDumpProtocol>( -1 );
					}
				}
			}

			return success;
		}
	}
}
