using HamstarHelpers.Components.Errors;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace HamstarHelpers.Helpers.DotNET {
	/** <summary>Assorted static "helper" functions pertaining to system-level functions.</summary> */
	public static class SystemHelpers {
		public static TimeSpan TimeStamp() {
			return ( DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0 ) );
		}

		public static long TimeStampInSeconds() {
			//return DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
			TimeSpan span = ( DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0 ) );
			return (long)span.TotalSeconds;
		}
		

		public static void OpenUrl( string url ) {
			try {
				Process.Start( url );
			} catch {
				try {
					//if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					//else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
					//else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					url = url.Replace( "&", "^&" );
					Process.Start( new ProcessStartInfo( "cmd", "/c start "+url ) { CreateNoWindow = true } );
				} catch( Exception ) {
					try {
						Process.Start( "xdg-open", url );
					} catch( Exception ) {
						Process.Start( "open", url );
					}
				}
			}
		}
	}
}
