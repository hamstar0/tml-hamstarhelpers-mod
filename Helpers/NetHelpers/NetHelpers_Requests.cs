using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.NetHelpers {
	public partial class NetHelpers {
		public static void MakePostRequestAsync( string url, byte[] bytes, Action<string> on_response, Action<Exception> on_error=null, Action on_completion=null ) {
			ThreadPool.QueueUserWorkItem( _ => {
				try {
					bool success;
					string output = NetHelpers.MakePostRequest( url, bytes, out success );

					if( success ) {
						on_response( output );
					} else {
						throw new Exception( "POST request unsuccessful (url: "+url+")" );
					}
				} catch( Exception e ) {
					if( on_error != null ) {
						on_error( e );
					}
				}

				if( on_completion != null ) {
					on_completion();
				}
			} );
		}


		public static string MakePostRequest( string url, byte[] bytes ) {
			bool _;
			return NetHelpers.MakePostRequest( url, bytes, out _ );
		}

		public static string MakePostRequest( string url, byte[] bytes, out bool success ) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "POST";
			request.ContentType = "application/json";   //"application/vnd.github.v3+json";
			request.ContentLength = bytes.Length;
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			using( Stream data_stream = request.GetRequestStream() ) {
				data_stream.Write( bytes, 0, bytes.Length );
				data_stream.Close();
			}

			HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
			string resp_data;

			success = ( (int)resp.StatusCode / 200 ) == 1;

			using( Stream resp_data_stream = resp.GetResponseStream() ) {
				var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
				resp_data = stream_read.ReadToEnd();
				resp_data_stream.Close();
			}

			return resp_data;
		}


		////////////////

		public static void MakeGetRequestAsync( string url, Action<string> on_response, Action<Exception> on_error = null, Action on_completion = null ) {
			ThreadPool.QueueUserWorkItem( _ => {
				try {
					bool success;
					string output = NetHelpers.MakeGetRequest( url, out success );
					
					if( success ) {
						on_response( output );
					} else {
						throw new Exception( "GET request unsuccessful (url: " + url + ")" );
					}
				} catch( Exception e ) {
					if( on_error != null ) {
						on_error( e );
					}
				}

				if( on_completion != null ) {
					on_completion();
				}
			} );
		}


		public static string MakeGetRequest( string url, out bool success ) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "GET";
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
			string resp_data;

			success = ((int)resp.StatusCode / 200) == 1;

			using( Stream resp_data_stream = resp.GetResponseStream() ) {
				var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
				resp_data = stream_read.ReadToEnd();
				resp_data_stream.Close();
			}

			return resp_data;
		}

		public static string MakeGetRequest( string url ) {
			bool _;
			return NetHelpers.MakeGetRequest( url, out _ );
		}
	}
}
