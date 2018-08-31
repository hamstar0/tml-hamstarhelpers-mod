using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.NetHelpers {
	public partial class NetHelpers {
		private readonly static object RequestMutex = new object();


		
		public static void MakePostRequestAsync( string url, byte[] bytes, Action<string> on_response, Action<Exception, string> on_error, Action on_completion=null ) {
			ThreadPool.QueueUserWorkItem( _ => {
				bool success;
				string output = null;

				try {
					//lock( NetHelpers.RequestMutex ) {
					output = NetHelpers.MakePostRequest( url, bytes, out success );
					//}

					if( success ) {
						on_response( output );
					} else {
						output = null;
						throw new Exception( "POST request unsuccessful (url: "+url+")" );
					}
				} catch( Exception e ) {
					on_error?.Invoke( e, output );
				}

				on_completion?.Invoke();
			} );
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
			int status_code = (int)resp.StatusCode;
			string resp_data;

			success = status_code >= 200 && status_code < 300;

			using( Stream resp_data_stream = resp.GetResponseStream() ) {
				var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
				resp_data = stream_read.ReadToEnd();
				resp_data_stream.Close();
			}

			return resp_data;
		}


		////////////////
		
		public static void MakeGetRequestAsync( string url, Action<string> on_response, Action<Exception, string> on_error, Action on_completion = null ) {
			ThreadPool.QueueUserWorkItem( _ => {
				bool success;
				string output = null;

				try {
					//lock( NetHelpers.RequestMutex ) {
					output = NetHelpers.MakeGetRequest( url, out success );
					//}

					if( success ) {
						on_response( output );
					} else {
						on_error?.Invoke( new Exception( "GET request unsuccessful (url: " + url + ")" ), output??"" );
					}
				} catch( Exception e ) {
					on_error?.Invoke( e, output??"" );
				}

				on_completion?.Invoke();
			} );
		}

		
		public static string MakeGetRequest( string url, out bool success ) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "GET";
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			HttpWebResponse resp = null;
			try {
				resp = (HttpWebResponse)request.GetResponse();
			} catch( WebException e ) {
				success = false;
				return "";
			}
 
			int status_code = (int)resp.StatusCode;
			string resp_data = "";

			success = status_code >= 200 && status_code < 300;

			using( Stream resp_data_stream = resp.GetResponseStream() ) {
				var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
				resp_data = stream_read.ReadToEnd();
				resp_data_stream.Close();
			}

			return resp_data;
		}
	}
}
