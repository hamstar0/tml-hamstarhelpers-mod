using System;
using System.Diagnostics;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static class SystemHelpers {
		public static long TimeStampInSeconds() {
			return (DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond) / 1000L;
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
				} catch( Exception _ ) {
					try {
						Process.Start( "xdg-open", url );
					} catch( Exception __ ) {
						Process.Start( "open", url );
					}
				}
			}
		}
	}
}
