using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Threading;
using System;
using System.Net;
using Terraria.ModLoader;


namespace HamstarHelpers.Libraries.Net {
	/// <summary>
	/// Assorted static "helper" functions pertaining to connecting to the web.
	/// </summary>
	public partial class WebConnectionLibraries {
		private static void HandleResponse(
					object _,
					UploadStringCompletedEventArgs args,
					Action<Exception> onError,
					Action<bool, string> onCompletion = null ) {
			try {
				if( onCompletion != null ) {
					bool success = !args.Cancelled && args.Error == null;
					string result = success ? args.Result :
						(args.Cancelled ? "Cancelled" : args.Error?.Message ?? "Error");
					onCompletion( success, result );
				}

				if( args.Error != null ) {
					onError( args.Error );
				}
			} catch( Exception e ) {
				LogLibraries.Warn( e.GetType().Name + " - " + e.Message );
			}
		}

		private static void HandleResponse(
					object _,
					DownloadStringCompletedEventArgs args,
					Action<Exception> onError,
					Action<bool, string> onCompletion = null ) {
			try {
				if( onCompletion != null ) {
					bool success = !args.Cancelled && args.Error == null;
					string result = success ? args.Result :
						( args.Cancelled ? "Cancelled" : args.Error?.Message ?? "Error" );
					onCompletion( success, result );
				}

				if( args.Error != null ) {
					onError( args.Error );
				}
			} catch( Exception e ) {
				LogLibraries.Warn( e.GetType().Name + " - " + e?.Message );
			}
		}


		////////////////

		/// <summary>
		/// Sends a POST request to a website asynchronously. 
		/// </summary>
		/// <param name="url">Website URL.</param>
		/// <param name="jsonData">JSON-formatted string data.</param>
		/// <param name="onError">Called on error. Receives an `Exception`.</param>
		/// <param name="onCompletion">Called regardless of success. Receives a boolean indicating if the site request succeeded,
		/// and the output (if any).</param>
		public static void MakePostRequestAsync(
					string url,
					string jsonData,
					Action<Exception> onError,
					Action<bool, string> onCompletion=null ) {
			TaskLauncher.Run( (token) => {
				try {
					ServicePointManager.Expect100Continue = false;
					//var values = new NameValueCollection {
					//	{ "modloaderversion", ModLoader.versionedName },
					//	{ "platform", ModLoader.CompressedPlatformRepresentation },
					//	{ "netversion", FrameworkVersion.Version.ToString() }
					//};

					using( var client = new WebClient() ) {
						ServicePointManager.ServerCertificateValidationCallback = ( sender, certificate, chain, policyErrors ) => {
							return true;
						};

						client.Headers.Add( HttpRequestHeader.ContentType, "application/json" );
						client.Headers.Add( HttpRequestHeader.UserAgent, "tModLoader " + ModLoader.version.ToString() );
						//client.Headers["UserAgent"] = "tModLoader " + ModLoader.version.ToString();
						client.UploadStringAsync( new Uri(url), "POST", jsonData );//UploadValuesAsync( new Uri( url ), "POST", values );
						client.UploadStringCompleted += (sender, e) => {
							if( token.IsCancellationRequested ) { return; }
							WebConnectionLibraries.HandleResponse( sender, e, onError, onCompletion );
						};
					}
				} catch( WebException e ) {
					if( e.Status == WebExceptionStatus.Timeout ) {
						onCompletion?.Invoke( false, "Timeout." );
						return;
					}

					if( e.Status == WebExceptionStatus.ProtocolError ) {
						var resp = (HttpWebResponse)e.Response;
						if( resp.StatusCode == HttpStatusCode.NotFound ) {
							onCompletion?.Invoke( false, "Not found." );
							return;
						}

						onCompletion?.Invoke( false, "Bad protocol." );
					}
				} catch( Exception e ) {
					onError?.Invoke( e );
					//LogHelpers.Warn( e.ToString() );
				}
			} );
		}


		////////////////

		/// <summary>
		/// Makes a GET request to a website.
		/// </summary>
		/// <param name="url">Website URL.</param>
		/// <param name="onError">Called on error. Receives an `Exception`.</param>
		/// <param name="onCompletion">Called regardless of success. Receives a boolean indicating if the site request succeeded,
		/// and the output (if any).</param>
		public static void MakeGetRequestAsync(
					string url,
					Action<Exception> onError,
					Action<bool, string> onCompletion = null ) {
			TaskLauncher.Run( ( token ) => {
				try {
					ServicePointManager.Expect100Continue = false;

					using( var client = new WebClient() ) {
						ServicePointManager.ServerCertificateValidationCallback =
							( sender, certificate, chain, policyErrors ) => { return true; };

						client.Headers.Add( HttpRequestHeader.UserAgent, "tModLoader " + ModLoader.version.ToString() );
						client.DownloadStringAsync( new Uri( url ) );
						client.DownloadStringCompleted += ( sender, e ) => {
							if( token.IsCancellationRequested ) { return; }
							WebConnectionLibraries.HandleResponse( sender, e, onError, onCompletion );
						};
						//client.UploadStringAsync( new Uri(url), "GET", "" );//UploadValuesAsync( new Uri( url ), "POST", values );
					}
				} catch( WebException e ) {
					if( e.Status == WebExceptionStatus.Timeout ) {
						onCompletion?.Invoke( false, "Timeout." );
						return;
					}

					if( e.Status == WebExceptionStatus.ProtocolError ) {
						var resp = (HttpWebResponse)e.Response;
						if( resp.StatusCode == HttpStatusCode.NotFound ) {
							onCompletion?.Invoke( false, "Not found." );
							return;
						}

						onCompletion?.Invoke( false, "Bad protocol." );
					}
				} catch( Exception e ) {
					onError?.Invoke( e );
						//LogHelpers.Warn( e.ToString() );
				}
			} );//, cts.Token );
		}
	}
}
