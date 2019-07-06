using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogHelpers {
		private static bool CanOutputOnceMessage( string msg, out string formattedMsg ) {
			var logHelpers = ModHelpersMod.Instance.LogHelpers;
			bool isShown = false;

			lock( LogHelpers.MyLock ) {
				if( !logHelpers.UniqueMessages.ContainsKey( msg ) ) {
					logHelpers.UniqueMessages[msg] = 1;
					formattedMsg = msg;
					isShown = true;
				} else {
					logHelpers.UniqueMessages[msg]++;

					if( ( Math.Log10( logHelpers.UniqueMessages[msg] ) % 1d ) == 0 ) {
						formattedMsg = "(" + logHelpers.UniqueMessages[msg] + "th) " + msg;
						isShown = true;
					} else {
						formattedMsg = msg;
					}
				}
			}

			return isShown;
		}


		////

		/// <summary>
		/// Outputs a plain log message "once" (or rather, once every log10 = 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void LogOnce( string msg ) {
			if( LogHelpers.CanOutputOnceMessage(msg, out msg) ) {
				LogHelpers.Log( "~" + msg );
			}
		}

		/// <summary>
		/// Outputs an "alert" log message "once" (or rather, once every log10 = 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void AlertOnce( string msg = "" ) {
			if( LogHelpers.CanOutputOnceMessage( msg, out msg ) ) {
				LogHelpers.Alert( "~" + msg );
			}
		}

		/// <summary>
		/// Outputs a "warning" log message "once" (or rather, once every log10 = 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void WarnOnce( string msg = "" ) {
			if( LogHelpers.CanOutputOnceMessage( msg, out msg ) ) {
				LogHelpers.Warn( "~" + msg );
			}
		}

		////

		/// <summary>
		/// Resets a given "once" log message.
		/// </summary>
		/// <param name="msg"></param>
		public static bool ResetOnceLogMessage( string msg ) {
			lock( LogHelpers.MyLock ) {
				return ModHelpersMod.Instance.LogHelpers.UniqueMessages.Remove( "~" + msg );
			}
		}

		/// <summary>
		/// Resets a given "once" warn message.
		/// </summary>
		/// <param name="msg"></param>
		public static bool ResetOnceAlertOrWarnMessage( string msg ) {
			lock( LogHelpers.MyLock ) {
				string fmtMsg = LogHelpers.FormatMessage( msg, true );

				return ModHelpersMod.Instance.LogHelpers.UniqueMessages.Remove( "~" + fmtMsg );
			}
		}
	}
}
