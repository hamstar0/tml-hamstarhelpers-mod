using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Net {
	/// <summary>
	/// Assorted static "helper" functions pertaining to connecting to the web.
	/// </summary>
	public partial class WebConnectionHelpers {
		/// <summary>
		/// Sends a POST request to a website asynchronously. 
		/// </summary>
		/// <param name="url">Website URL.</param>
		/// <param name="jsonData">JSON-formatted string data.</param>
		/// <param name="onError">Called on error. Receives an `Exception` and the site's output (if any).</param>
		/// <param name="onCompletion">Called regardless of success. Receives processed output (if any) from `onSuccess`, and
		/// a boolean indicating if the site request succeeded.</param>
		public static void MakePostRequestAsync( string url, string jsonData,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion=null ) {
			ThreadPool.QueueUserWorkItem( _ => {
				bool success = false;
				string output = null;

				try {
					success = WebConnectionHelpers.MakePostRequest( url, jsonData, out output );

					if( !success ) {
						output = "";
						throw new HamstarException( "POST request unsuccessful (url: "+url+")" );
					}
				} catch( Exception e ) {
					onError?.Invoke( e, output );
				}

				onCompletion?.Invoke( success, output );
			} );
		}


		/// <summary>
		/// Makes a POST request to a website. May tie up the current thread awaiting a response.
		/// </summary>
		/// <param name="url">Website URL.</param>
		/// <param name="jsonData">JSON-formatted string data.</param>
		/// <param name="output">Data returned from the website.</param>
		/// <returns>`true` if response code is 400; success.</returns>
		public static bool MakePostRequest( string url, string jsonData, out string output ) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "POST";
			request.ContentType = "application/json";   //"application/vnd.github.v3+json";
			request.ContentLength = jsonData.Length;
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			using( Stream dataStream = request.GetRequestStream() ) {
				byte[] bytes = Encoding.UTF8.GetBytes( jsonData );
				
				dataStream.Write( bytes, 0, bytes.Length );
				dataStream.Close();
			}

			HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
			int statusCode = (int)resp.StatusCode;

			using( Stream respDataStream = resp.GetResponseStream() ) {
				var streamRead = new StreamReader( respDataStream, Encoding.UTF8 );
				output = streamRead.ReadToEnd();
				respDataStream.Close();
			}

			return statusCode >= 200 && statusCode < 300;
		}


		////////////////

		/// <summary>
		/// Makes a GET request to a website.
		/// </summary>
		/// <param name="url">Website URL.</param>
		/// <param name="onError">Called on error. Receives an `Exception` and the site's output (if any).</param>
		/// <param name="onCompletion">Called regardless of success. Receives processed output (if any) from `onSuccess`, and
		/// a boolean indicating if the site request succeeded.</param>
		public static void MakeGetRequestAsync( string url,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion = null ) {
			ThreadPool.QueueUserWorkItem( _ => {
				bool success = false;
				string output = null;

				try {
					//lock( NetHelpers.RequestMutex ) {
					success = WebConnectionHelpers.MakeGetRequest( url, out output );
					//}

					if( !success ) {
						output = "";
						throw new HamstarException( "GET request unsuccessful (url: " + url + ")" );
					}
				} catch( Exception e ) {
					onError?.Invoke( e, output ?? "" );
				}
				
				onCompletion?.Invoke( success, output );
			} );
		}


		/// <summary>
		/// Makes a GET request to a website. May tie up the current thread awaiting a response.
		/// </summary>
		/// <param name="url">Website URL.</param>
		/// <param name="output">Data returned from the website.</param>
		/// <returns>`true` if response code is 400; success.</returns>
		public static bool MakeGetRequest( string url, out string output ) {
			var request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "GET";
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			HttpWebResponse resp = null;
			try {
				resp = (HttpWebResponse)request.GetResponse();
			} catch( WebException _ ) {
				output = "";
				return false;
			}

			using( Stream respDataStream = resp.GetResponseStream() ) {
				var streamRead = new StreamReader( respDataStream, Encoding.UTF8 );
				output = streamRead.ReadToEnd();
				respDataStream.Close();
			}

			int statusCode = (int)resp.StatusCode;
			return statusCode >= 200 && statusCode < 300;
		}
	}
}
