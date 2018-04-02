using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;


namespace HamstarHelpers.WebRequests {
	partial class ServerBrowserReporter {
		private static void DoWorkToValidateServer( ServerBrowserEntry server_data, string hash ) {
			string hash_base = "";

			for( int i = 0; i < 1000000; i++ ) {
				hash_base = i + "";

				string test_hash = SystemHelpers.ComputeSHA256Hash( hash_base );
				if( hash == test_hash ) {
					break;
				}
			}

			var output_obj = new ServerBrowserWorkProof {
				ServerIP = server_data.ServerIP,
				Port = server_data.Port,
				WorldName = server_data.WorldName,
				HashBase = hash_base
			};

			string json_str = JsonConvert.SerializeObject( output_obj, Formatting.None );
			byte[] json_bytes = Encoding.ASCII.GetBytes( json_str );

			Thread.Sleep( 1500 );

			NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReporter.URL, json_bytes, delegate ( string output ) {
				LogHelpers.Log( "Server browser processing complete." );
			}, delegate ( Exception e, string output ) {
				LogHelpers.Log( "Server browser reply returned error: " + e.ToString() );
			} );

			ServerBrowserReporter.LastSendTimestamp = SystemHelpers.TimeStampInSeconds();
		}
	}
}
