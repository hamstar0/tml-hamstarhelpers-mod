using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Timers;
using System;
using System.Net;
using Terraria;


namespace HamstarHelpers.Helpers.NetHelpers {
	public partial class NetHelpers {
		private string PublicIP = null;
		
		private int CurrentPing = -1;



		////////////////

		internal NetHelpers() {
			this.LoadIPAsync();

			int attempts = 3;

			Timers.SetTimer( "ModHelpersRetryIP", 60 * 20, delegate () {
				if( this.PublicIP != null ) { return false; }

				this.LoadIPAsync();
				return attempts-- > 0;
			} );
		}
		

		private void LoadIPAsync() {
			Func<string, Tuple<object, bool>> onResponse = ( string output ) => {
				if( this.PublicIP != null ) {
					return null;
				}

				string[] a = output.Split( ':' );
				string a2 = a[1].Substring( 1 );
				string[] a3 = a2.Split( '<' );
				string a4 = a3[0];

				this.PublicIP = a4;

				return Tuple.Create( (object)null, true );
			};

			Action<Exception, string> onFail = delegate ( Exception e, string output ) {
				if( e is WebException ) {
					LogHelpers.Log( "Could not acquire IP: " + e.Message );
				} else {
					LogHelpers.Log( "Could not acquire IP: " + e.ToString() );
				}
			};

			NetHelpers.MakeGetRequestAsync<object>( "http://checkip.dyndns.org/", onResponse, onFail );
			//NetHelpers.MakeGetRequestAsync( "https://api.ipify.org/", onSuccess, onFail );
			//using( WebClient webClient = new WebClient() ) {
			//	this.PublicIP = webClient.DownloadString( "http://ifconfig.me/ip" );
			//}
		}

		////////////////

		internal void UpdatePing( int ping ) {
			//this.CurrentPing = ( this.CurrentPing + this.CurrentPing + ping ) / 3;
			this.CurrentPing = ( this.CurrentPing + ping ) / 2;
		}
	}
}
