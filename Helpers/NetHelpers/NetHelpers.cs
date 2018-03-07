using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using Terraria.ModLoader;


namespace HamstarHelpers.NetHelpers {
	public partial class NetHelpers {
		public static void MakePostRequestAsync( string url, byte[] bytes, Action<string> on_response, Action<Exception> on_error=null, Action on_completion=null ) {
			var worker = new BackgroundWorker();
			string output = "";
			Exception err = null;

			worker.DoWork += delegate ( object sender, DoWorkEventArgs args ) {
				try {
					output = NetHelpers.MakePostRequest( url, bytes );
				} catch( Exception e ) {
					err = e;
				}
			};
			worker.RunWorkerCompleted += delegate( object sender, RunWorkerCompletedEventArgs args ) {
				if( err == null ) {
					on_response( output );
				} else {
					if( on_error != null ) {
						on_error( err );
					}
				}

				if( on_completion != null ) {
					on_completion();
				}
			};

			worker.RunWorkerAsync();
		}


		public static string MakePostRequest( string url, byte[] bytes ) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "POST";
			request.ContentType = "application/json";   //"application/vnd.github.v3+json";
			request.ContentLength = bytes.Length;
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			using( Stream data_stream = request.GetRequestStream() ) {
				data_stream.Write( bytes, 0, bytes.Length );
				data_stream.Close();
			}

			WebResponse resp = request.GetResponse();
			string resp_data;

			using( Stream resp_data_stream = resp.GetResponseStream() ) {
				var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
				resp_data = stream_read.ReadToEnd();
				resp_data_stream.Close();
			}

			return resp_data;
		}


		////////////////

		public static void MakeGetRequestAsync( string url, Action<string> on_response, Action<Exception> on_error = null, Action on_completion = null ) {
			var worker = new BackgroundWorker();
			string output = "";
			Exception err = null;

			worker.DoWork += delegate ( object sender, DoWorkEventArgs args ) {
				try {
					output = NetHelpers.MakeGetRequest( url );
				} catch( Exception e ) {
					err = e;
				}
			};
			worker.RunWorkerCompleted += delegate ( object sender, RunWorkerCompletedEventArgs args ) {
				if( err == null ) {
					on_response( output );
				} else {
					if( on_error != null ) {
						on_error( err );
					}
				}

				if( on_completion != null ) {
					on_completion();
				}
			};

			worker.RunWorkerAsync();
		}


		public static string MakeGetRequest( string url ) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "GET";
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			WebResponse resp = request.GetResponse();
			string resp_data;

			using( Stream resp_data_stream = resp.GetResponseStream() ) {
				var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
				resp_data = stream_read.ReadToEnd();
				resp_data_stream.Close();
			}

			return resp_data;
		}
	}
}
