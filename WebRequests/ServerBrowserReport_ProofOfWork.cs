using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Newtonsoft.Json;
using System;
using System.Text;


namespace HamstarHelpers.WebRequests {
	partial class ServerBrowserReporter {
		private static void DoWorkToValidateServer( ServerBrowserEntry server_data, string hash ) {
			const int Radix = 36;
			const int DigitOne = Radix;
			const int DigitTwo = DigitOne * Radix;
			const int DigitThree = DigitTwo * Radix;
			const int DigitFour = DigitTwo * Radix;

			string hash_base = "";

			for( int i = 0; i < DigitFour; i += DigitThree ) {
				for( int j = 0; j < DigitThree; j += DigitTwo ) {
					for( int k = 0; k < DigitTwo; k += DigitOne ) {
						for( int l = 0; l < DigitOne; l += 1 ) {
							hash_base = SystemHelpers.ConvertDecimalToRadix( i + j + k + l, 36 );
							string test_hash = SystemHelpers.ComputeSHA256Hash( hash_base );

							if( test_hash == hash_base ) {
								break;
							}
						}
					}
				}
			}

			var output_obj = new ServerBrowserWorkProof {
				ServerIP = server_data.ServerIP,
				Port = server_data.Port,
				WoldName = server_data.WorldName,
				HashBase = hash_base
			};

			string json_str = JsonConvert.SerializeObject( output_obj, Formatting.None );
			byte[] json_bytes = Encoding.ASCII.GetBytes( json_str );

			NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReporter.URL, json_bytes, delegate ( string output ) {
				LogHelpers.Log( "Server browser processing complete." );
			}, delegate ( Exception e, string output ) {
				LogHelpers.Log( "Server browser reply returned error: " + e.ToString() );
			} );
		}
	}
}
