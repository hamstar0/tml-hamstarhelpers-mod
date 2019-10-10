using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Net {
	/// <summary>
	/// Assorted static "helper" functions pertaining to connecting to the web.
	/// </summary>
	public partial class WebConnectionHelpers {
		private static void HandleResponse( object _,
					UploadStringCompletedEventArgs e,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion = null ) {
			if( onCompletion != null ) {
				bool success = !e.Cancelled && e.Error == null;
				string result = success ? e.Result :
					(e.Cancelled ? "Cancelled" : e.Error?.Message ?? "Error");
				onCompletion( success, result );
			}

			if( e.Error != null ) {
				onError( e.Error, e.Result );
			}
		}

		private static void HandleResponse( object _,
					DownloadStringCompletedEventArgs e,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion = null ) {
			if( onCompletion != null ) {
				bool success = !e.Cancelled && e.Error == null;
				string result = success ? e.Result :
					(e.Cancelled ? "Cancelled" : e.Error?.Message ?? "Error");
				onCompletion( success, result );
			}

			if( e.Error != null ) {
				onError( e.Error, e.Result );
			}
		}


		////////////////

		/// <summary>
		/// Sends a POST request to a website asynchronously. 
		/// </summary>
		/// <param name="url">Website URL.</param>
		/// <param name="jsonData">JSON-formatted string data.</param>
		/// <param name="onError">Called on error. Receives an `Exception` and the site's output (if any).</param>
		/// <param name="onCompletion">Called regardless of success. Receives a boolean indicating if the site request succeeded,
		/// and the output (if any).</param>
		public static void MakePostRequestAsync( string url, string jsonData,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion=null ) {
			var cts = new CancellationTokenSource();

			try {
				Task.Factory.StartNew( () => {
					ServicePointManager.Expect100Continue = false;
					//var values = new NameValueCollection {
					//	{ "modloaderversion", ModLoader.versionedName },
					//	{ "platform", ModLoader.CompressedPlatformRepresentation },
					//	{ "netversion", FrameworkVersion.Version.ToString() }
					//};

					using( var client = new WebClient() ) {
						ServicePointManager.ServerCertificateValidationCallback = ( sender, certificate, chain, policyErrors ) => { return true; };

						client.Headers.Add( HttpRequestHeader.ContentType, "application/json" );
						client.Headers.Add( HttpRequestHeader.UserAgent, "tModLoader "+ModLoader.version.ToString() );
						//client.Headers["UserAgent"] = "tModLoader " + ModLoader.version.ToString();
						client.UploadStringAsync( new Uri( url ), "POST", jsonData );//UploadValuesAsync( new Uri( url ), "POST", values );
						client.UploadStringCompleted += ( sender, e ) => {
							WebConnectionHelpers.HandleResponse( sender, e, onError, onCompletion );
						};
					}
				}, cts.Token );
			} catch( WebException e ) {
				if( e.Status == WebExceptionStatus.Timeout ) {
					onError?.Invoke( e, "Timeout." );
					return;
				}

				if( e.Status == WebExceptionStatus.ProtocolError ) {
					var resp = (HttpWebResponse)e.Response;
					if( resp.StatusCode == HttpStatusCode.NotFound ) {
						onError?.Invoke( e, "Not found." );
						return;
					}

					onError?.Invoke( e, "Bad protocol." );
				}
			} catch( Exception e ) {
				onError?.Invoke( e, "Unknown." );
				//LogHelpers.Warn( e.ToString() );
			}
		}


		////////////////

		/// <summary>
		/// Makes a GET request to a website.
		/// </summary>
		/// <param name="url">Website URL.</param>
		/// <param name="onError">Called on error. Receives an `Exception` and the site's output (if any).</param>
		/// <param name="onCompletion">Called regardless of success. Receives a boolean indicating if the site request succeeded,
		/// and the output (if any).</param>
		public static void MakeGetRequestAsync( string url,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion = null ) {
			var cts = new CancellationTokenSource();

			try {
				Task.Factory.StartNew( () => {
					ServicePointManager.Expect100Continue = false;

					using( var client = new WebClient() ) {
						ServicePointManager.ServerCertificateValidationCallback =
							( sender, certificate, chain, policyErrors ) => { return true; };

						client.Headers.Add( HttpRequestHeader.UserAgent, "tModLoader "+ModLoader.version.ToString() );
						client.DownloadStringAsync( new Uri(url) );
						client.DownloadStringCompleted += ( sender, e ) => {
							WebConnectionHelpers.HandleResponse( sender, e, onError, onCompletion );
						};
						//client.UploadStringAsync( new Uri(url), "GET", "" );//UploadValuesAsync( new Uri( url ), "POST", values );
					}
				}, cts.Token );
			} catch( WebException e ) {
				if( e.Status == WebExceptionStatus.Timeout ) {
					onError?.Invoke( e, "Timeout." );
					return;
				}

				if( e.Status == WebExceptionStatus.ProtocolError ) {
					var resp = (HttpWebResponse)e.Response;
					if( resp.StatusCode == HttpStatusCode.NotFound ) {
						onError?.Invoke( e, "Not found." );
						return;
					}

					onError?.Invoke( e, "Bad protocol." );
				}
			} catch( Exception e ) {
				onError?.Invoke( e, "Unknown." );
				//LogHelpers.Warn( e.ToString() );
			}
		}
	}
}
