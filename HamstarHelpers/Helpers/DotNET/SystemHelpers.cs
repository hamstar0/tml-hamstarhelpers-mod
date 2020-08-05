using HamstarHelpers.Classes.Errors;
using System;
using System.Diagnostics;


namespace HamstarHelpers.Helpers.DotNET {
	/// <summary>
	/// Assorted static "helper" functions pertaining to system-level functions.
	/// </summary>
	public class SystemHelpers {
		/// <summary>
		/// Returns a timestamp spanning between 1970 and the present.
		/// </summary>
		/// <returns>The time span.</returns>
		public static TimeSpan TimeStamp() {
			return ( DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0 ) );
		}

		/// <summary>
		/// Returns a timestamp measured in seconds spanning between 1970 and the present.
		/// </summary>
		/// <returns>Seconds of the time span.</returns>
		public static long TimeStampInSeconds() {
			//return DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
			TimeSpan span = ( DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0 ) );
			return (long)span.TotalSeconds;
		}
		

		/// <summary>
		/// Opens a given URL with the OS default program (typically your default web browser).
		/// </summary>
		/// <param name="url">URL to open.</param>
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
