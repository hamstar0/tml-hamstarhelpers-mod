using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.DebugHelpers {
	public class LogHelpers {
		public static void Log( string msg ) {
			var log_helpers = HamstarHelpersMod.Instance.LogHelpers;

			double now_seconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds - log_helpers.StartTime;

			string now_seconds_whole = ( (int)now_seconds ).ToString( "D6" );
			string now_seconds_decimal = ( now_seconds - (int)now_seconds ).ToString( "N2" );
			string now = now_seconds_whole + "." + (now_seconds_decimal.Length > 2 ? now_seconds_decimal.Substring( 2 ) : now_seconds_decimal);

			string logged = Main.netMode + ":" + Main.myPlayer.ToString( "D3" ) + ":" + log_helpers.LoggedMessages.ToString( "D5" ) + " - " + now;
			if( logged.Length < 26 ) {
				logged += new String( ' ', 26 - logged.Length );
			} else {
				logged += "  ";
			}

			ErrorLogger.Log( logged + msg );

			log_helpers.LoggedMessages++;
		}



		////////////////

		private int LoggedMessages;
		private double StartTime;


		////////////////

		internal LogHelpers() {
			this.Reset();
		}

		internal void OnWorldExit() {
			this.Reset();
		}

		internal void Reset() {
			this.LoggedMessages = 0;
			this.StartTime = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;
		}
	}
}
