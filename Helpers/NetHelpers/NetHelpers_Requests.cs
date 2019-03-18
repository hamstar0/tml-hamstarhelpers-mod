using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.NetHelpers {
	public partial class NetHelpers {
		public static void MakePostRequestAsync( string url, byte[] bytes, Action<string> onResponse, Action<Exception, string> onError, Action onCompletion=null ) {
			ThreadPool.QueueUserWorkItem( _ => {
				bool success;
				string output = null;

				try {
					//lock( NetHelpers.RequestMutex ) {
					output = NetHelpers.MakePostRequest( url, bytes, out success );
					//}

					if( success ) {
						onResponse( output );
					} else {
						output = null;
						throw new HamstarException( "POST request unsuccessful (url: "+url+")" );
					}
				} catch( Exception e ) {
					onError?.Invoke( e, output );
				}

				onCompletion?.Invoke();
			} );
		}

		
		public static string MakePostRequest( string url, byte[] bytes, out bool success ) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "POST";
			request.ContentType = "application/json";   //"application/vnd.github.v3+json";
			request.ContentLength = bytes.Length;
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			using( Stream dataStream = request.GetRequestStream() ) {
				dataStream.Write( bytes, 0, bytes.Length );
				dataStream.Close();
			}

			HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
			int statusCode = (int)resp.StatusCode;
			string respData;

			success = statusCode >= 200 && statusCode < 300;

			using( Stream respDataStream = resp.GetResponseStream() ) {
				var streamRead = new StreamReader( respDataStream, Encoding.UTF8 );
				respData = streamRead.ReadToEnd();
				respDataStream.Close();
			}

			return respData;
		}


		////////////////

		public static void MakeGetRequestAsync<T>( string url,
				Func<string, Tuple<T, bool>> onResponse,
				Action<Exception, string> onError,
				Action<T, bool> onCompletion = null ) where T : class {

			ThreadPool.QueueUserWorkItem( _ => {
				bool success = false;
				string output = null;

				var responseVal = Tuple.Create( (T)null, false );

				try {
					//lock( NetHelpers.RequestMutex ) {
					output = NetHelpers.MakeGetRequest( url, out success );
					//}

					if( success ) {
						responseVal = onResponse( output );
					} else {
						onError?.Invoke( new Exception( "GET request unsuccessful (url: " + url + ")" ), output ?? "" );
					}
				} catch( Exception e ) {
					onError?.Invoke( e, output ?? "" );
				}
				
				onCompletion?.Invoke( responseVal.Item1, (success && responseVal.Item2) );
			} );
		}

		////
		
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
 
			int statusCode = (int)resp.StatusCode;
			string respData = "";

			success = statusCode >= 200 && statusCode < 300;

			using( Stream respDataStream = resp.GetResponseStream() ) {
				var streamRead = new StreamReader( respDataStream, Encoding.UTF8 );
				respData = streamRead.ReadToEnd();
				respDataStream.Close();
			}

			return respData;
		}
	}
}
