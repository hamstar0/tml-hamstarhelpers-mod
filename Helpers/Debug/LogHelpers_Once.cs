using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogHelpers {
		internal static bool CanOutputOnceMessage( string msg, out string formattedMsg ) {
			var logHelpers = ModHelpersMod.Instance?.LogHelpers;
			if( logHelpers == null ) {
				formattedMsg = msg;
				return false;
			}

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
		/// Outputs a plain log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void LogOnce( string msg ) {
			if( LogHelpers.CanOutputOnceMessage(msg, out msg) ) {
				LogHelpers.Log( "~" + msg );
			}
		}

		/// <summary>
		/// Outputs an "alert" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void AlertOnce( string msg = "" ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogHelpers.FormatMessageFull( msg, 3 );

			string outMsg;
			LogHelpers.CanOutputOnceMessage( fmtMsg.Full, out outMsg );

			if( !LogHelpers.CanOutputOnceMessage( fmtMsg.Context+" "+msg, out _ ) ) {
				return;
			}

			mymod.Logger.Warn( "~" + outMsg );	//was Error(...)
		}

		/// <summary>
		/// Outputs a "warning" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void WarnOnce( string msg = "" ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogHelpers.FormatMessageFull( msg, 3 );

			string outMsg;
			LogHelpers.CanOutputOnceMessage( fmtMsg.Full, out outMsg );

			if( !LogHelpers.CanOutputOnceMessage( fmtMsg.Context + " " + msg, out _ ) ) {
				return;
			}

			mymod.Logger.Error( "~" + outMsg );	//was Fatal(...)
		}

		////

		/// <summary>
		/// Outputs a plain log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="color"></param>
		public static void LogAndPrintOnce( string msg, Color? color=null ) {
			if( LogHelpers.CanOutputOnceMessage( msg, out msg ) ) {
				LogHelpers.Log( "~" + msg );
				Main.NewText( "~" + msg, (color.HasValue ? color.Value : Color.White) );
			}
		}

		/// <summary>
		/// Outputs an "alert" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="color"></param>
		public static void AlertAndPrintOnce( string msg = "", Color? color = null ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogHelpers.FormatMessageFull( msg, 3 );

			string outMsg;
			LogHelpers.CanOutputOnceMessage( fmtMsg.Full, out outMsg );

			if( !LogHelpers.CanOutputOnceMessage( fmtMsg.Context + " " + msg, out _ ) ) {
				return;
			}

			mymod.Logger.Warn( "~" + outMsg ); //was Error(...)
			Main.NewText( "~" + fmtMsg.Context + " - " + msg, ( color.HasValue ? color.Value : Color.White ) );
		}

		/// <summary>
		/// Outputs a "warning" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="color"></param>
		public static void WarnAndPrintOnce( string msg = "", Color? color = null ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogHelpers.FormatMessageFull( msg, 3 );

			string outMsg;
			LogHelpers.CanOutputOnceMessage( fmtMsg.Full, out outMsg );

			if( !LogHelpers.CanOutputOnceMessage( fmtMsg.Context + " " + msg, out _ ) ) {
				return;
			}

			mymod.Logger.Error( "~" + outMsg );	//was Fatal(...)
			Main.NewText( "~!" + fmtMsg.Context + " - " + msg, ( color.HasValue ? color.Value : Color.White ) );
		}


		////////////////

		/// <summary>
		/// Resets a given "once" log, alert, or warn messages.
		/// </summary>
		/// <param name="msg"></param>
		public static void ResetOnceMessage( string msg ) {
			lock( LogHelpers.MyLock ) {
				string fmtMsg = LogHelpers.FormatMessage( msg, 3 );

				ModHelpersMod.Instance.LogHelpers.UniqueMessages.Remove( "~" + msg );
				ModHelpersMod.Instance.LogHelpers.UniqueMessages.Remove( "~" + fmtMsg );
			}
		}
	}
}
