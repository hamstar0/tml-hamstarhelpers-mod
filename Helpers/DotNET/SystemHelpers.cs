using HamstarHelpers.Components.Errors;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace HamstarHelpers.Helpers.DotNET {
	public static class SystemHelpers {
		public static TimeSpan TimeStamp() {
			return ( DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0 ) );
		}

		public static long TimeStampInSeconds() {
			//return DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
			TimeSpan span = ( DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0 ) );
			return (long)span.TotalSeconds;
		}


		/// <summary>
		/// Converts the given decimal number to the numeral system with the
		/// specified radix (in the range [2, 36]).
		/// </summary>
		/// <param name="number">The number to convert.</param>
		/// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
		/// <returns></returns>
		public static string ConvertDecimalToRadix( long number, int radix ) {
			const int BitsInLong = 64;
			const string Digits = "0123456789abcdefghijklmnopqrstuvwxyz";

			if( radix < 2 || radix > Digits.Length ) {
				throw new HamstarException( "The radix must be >= 2 and <= " + Digits.Length.ToString() );
			}

			if( number == 0 ) {
				return "0";
			}

			int index = BitsInLong - 1;
			long currNum = Math.Abs( number );
			char[] chars = new char[ BitsInLong ];

			while( currNum != 0 ) {
				int remainder = (int)( currNum % radix );
				chars[index--] = Digits[remainder];
				currNum = currNum / radix;
			}

			string result = new String( chars, index + 1, BitsInLong - index - 1 );
			if( number < 0 ) {
				result = "-" + result;
			}

			return result;
		}


		public static string ComputeSHA256Hash( string str ) {
			var crypt = new SHA256Managed();
			byte[] hashBytes = crypt.ComputeHash( Encoding.UTF8.GetBytes( str ) );
			string hash = Convert.ToBase64String( hashBytes );
			
			return hash;
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
