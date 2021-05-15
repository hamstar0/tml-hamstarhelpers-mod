using System;
using System.Net;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Helpers.Net {
	/// @private
	public partial class NetPlayHelpers {
		private string PublicIP = null;
		
		private int CurrentPing = -1;



		////////////////

		internal NetPlayHelpers() {
			if( ModHelpersConfig.Instance.DisableOwnIPCheck ) {
				return;
			}

			this.LoadIPAsync();

			int attempts = 3;

			Timers.SetTimer( "ModHelpersRetryIP", 60 * 20, true, delegate () {
				if( this.PublicIP != null ) { return false; }

				this.LoadIPAsync();
				return attempts-- > 0;
			} );
		}
		

		private void LoadIPAsync() {
			Action<bool, string> onCompletion = null;
			Action<Exception, string> onFail = null;

			onCompletion = ( success, output ) => {
				if( !success ) {
					onFail( new Exception( "Could not reach site." ), output );
					return;
				}
				if( this.PublicIP != null ) {
					return;
				}

				string[] a = output.Split( ':' );
				if( a.Length < 2 ) {
					onFail( new Exception( "Malformed IP output (1)." ), output );
					return;
				}

				string a2 = a[1].Substring( 1 );

				string[] a3 = a2.Split( '<' );
				if( a3.Length == 0 ) {
					onFail( new Exception( "Malformed IP output (2)." ), output );
					return;
				}

				string a4 = a3[0];

				this.PublicIP = a4;
			};

			onFail = ( Exception e, string output ) => {
				if( e is WebException ) {
					LogHelpers.Log( "Could not acquire IP: " + e.Message );
				} else {
					LogHelpers.Alert( "Could not acquire IP: " + e.ToString() );
				}
			};

			WebConnectionHelpers.MakeGetRequestAsync( "http://checkip.dyndns.org/", e => onFail(e, ""), onCompletion );
			//NetHelpers.MakeGetRequestAsync( "https://api.ipify.org/", onSuccess, onFail );
			//using( WebClient webClient = new WebClient() ) {
			//	this.PublicIP = webClient.DownloadString( "http://ifconfig.me/ip" );
			//}
		}

		////////////////

		internal void UpdatePing( int upTimeSpan, int downTimeSpan ) {
			//this.CurrentPing = ( this.CurrentPing + this.CurrentPing + ping ) / 3;
			this.CurrentPing = (this.CurrentPing + upTimeSpan) / 2;
		}
	}
}
