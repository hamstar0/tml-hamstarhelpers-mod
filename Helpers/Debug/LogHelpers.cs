using System;
using Terraria;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogHelpers {
		/// <summary>
		/// Formats a given message as it woudl appear in the log output.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="addContext">Prefixes the caller's namespace, class, and method "context".</param>
		/// <returns></returns>
		public static string FormatMessage( string msg, bool addContext ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			var logHelpers = mymod.LogHelpers;
			string output = msg;

			try {
				double nowSeconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds - logHelpers.StartTime;

				string nowSecondsWhole = ( (int)nowSeconds ).ToString( "D6" );
				string nowSecondsDecimal = ( nowSeconds - (int)nowSeconds ).ToString( "N2" );
				string now = nowSecondsWhole + "." + ( nowSecondsDecimal.Length > 2 ? nowSecondsDecimal.Substring( 2 ) : nowSecondsDecimal );

				string from = Main.myPlayer.ToString( "D3" );
				string logged = Main.netMode + ":" + from + " - " + now;
				if( logged.Length < 26 ) {
					logged += new String( ' ', 26 - logged.Length );
				} else {
					logged += "  ";
				}

				output = logged + msg;
			} catch( Exception e ) {
				output = "FORMATTING ERROR (" + e.GetType().Name + ") - " + msg;
			}

			if( addContext ) {
				output = DebugHelpers.GetCurrentContext( 2 ) + ( ( output != "" ) ? " - " + output : "" );
			}

			return output;
		}


		////////////////

		/// <summary>
		/// Outputs a plain log message.
		/// </summary>
		/// <param name="msg"></param>
		public static void Log( string msg="" ) {
			lock( LogHelpers.MyLock ) {
				ModHelpersMod mymod = ModHelpersMod.Instance;
				mymod.Logger.Info( LogHelpers.FormatMessage( msg, false ) );
			}
		}

		/// <summary>
		/// Outputs an "alert" log message (TML considers it an error-type message).
		/// </summary>
		/// <param name="msg"></param>
		public static void Alert( string msg="" ) {
			lock( LogHelpers.MyLock ) {
				ModHelpersMod mymod = ModHelpersMod.Instance;
				string fmtMsg = LogHelpers.FormatMessage( msg, true );

				mymod.Logger.Error( fmtMsg );
				//LogHelpers.Log( DebugHelpers.GetCurrentContext( 2 ) + ((msg != "") ? " - " + msg : "") );
			}
		}

		/// <summary>
		/// Outputs a warning log message (TML considers it a fatal-type message).
		/// </summary>
		/// <param name="msg"></param>
		public static void Warn( string msg="" ) {
			lock( LogHelpers.MyLock ) {
				ModHelpersMod mymod = ModHelpersMod.Instance;
				string fmtMsg = LogHelpers.FormatMessage( msg, true );

				mymod.Logger.Fatal( fmtMsg );
				//LogHelpers.Log( DebugHelpers.GetCurrentContext( 2 ) + ((msg != "") ? " - " + msg: "") );
			}
		}
	}
}
