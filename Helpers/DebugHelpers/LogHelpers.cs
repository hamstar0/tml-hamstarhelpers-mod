using HamstarHelpers.Services.Promises;
using HamstarHelpers.TmlHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.DebugHelpers {
	public class LogHelpers {
		private static Object MyLock = new Object();



		public static void Log( string msg ) {
			try {
				HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
				var log_helpers = mymod.LogHelpers;

				double now_seconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds - log_helpers.StartTime;

				string now_seconds_whole = ( (int)now_seconds ).ToString( "D6" );
				string now_seconds_decimal = ( now_seconds - (int)now_seconds ).ToString( "N2" );
				string now = now_seconds_whole + "." + ( now_seconds_decimal.Length > 2 ? now_seconds_decimal.Substring( 2 ) : now_seconds_decimal );

				string logged = Main.netMode + ":" + Main.myPlayer.ToString( "D3" ) + ":" + log_helpers.LoggedMessages.ToString( "D5" ) + " - " + now;
				if( logged.Length < 26 ) {
					logged += new String( ' ', 26 - logged.Length );
				} else {
					logged += "  ";
				}

				if( mymod.Config.UseCustomLogging ) {
					log_helpers.OutputDirect( log_helpers.GetHourlyLogFileName(), logged + msg );

					if( mymod.Config.UseCustomLoggingPerNetMode ) {
						log_helpers.OutputDirect( log_helpers.GetModalLogFileName(), logged + msg );
					}

					if( mymod.Config.UseAlsoNormalLogging ) {
						ErrorLogger.Log( logged + msg );
					}
				} else {
					ErrorLogger.Log( logged + msg );
				}

				log_helpers.LoggedMessages++;
			} catch( Exception e ) {
				try {
					ErrorLogger.Log( "FALLBACK LOGGER 2 (" + e.GetType().Name + ") " + msg );
				} catch { }
			}
		}



		////////////////

		private int LoggedMessages;
		private DateTime StartTimeBase;
		private double StartTime;


		////////////////

		internal LogHelpers() {
			this.Reset();

			Promises.AddWorldUnloadEachPromise( this.OnWorldExit );
		}

		private void OnWorldExit() {
			this.Reset();
		}

		internal void Reset() {
			this.LoggedMessages = 0;
			this.StartTimeBase = DateTime.UtcNow;
			this.StartTime = this.StartTimeBase.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;
		}

		////////////////

		public string GetLogPath() {
			string base_path = ErrorLogger.LogPath + Path.DirectorySeparatorChar;
			string full_path = base_path + "History" + Path.DirectorySeparatorChar;

			Directory.CreateDirectory( base_path );
			Directory.CreateDirectory( full_path );

			return full_path;
		}

		public string GetHourlyLogFileName() {
			DateTime curr_hour = this.StartTimeBase;
			curr_hour.AddMinutes( -curr_hour.Minute );
			curr_hour.AddSeconds( -curr_hour.Second );
			curr_hour.AddMilliseconds( -curr_hour.Millisecond );

			string when = curr_hour.ToString( "MM-dd, HH" );

			return "Log Any " + when + ".txt";
		}

		public string GetModalLogFileName() {
			string mode;
			
			switch( Main.netMode ) {
			case 0:
				mode = "Single";
				break;
			case 1:
				mode = "Client";
				break;
			case 2:
				mode = "Server";
				break;
			default:
				mode = "Unknown Mode";
				break;
			}

			string when = this.StartTimeBase.ToString( "MM-dd, HH.mm.ss" );

			return "Log " + mode + " " + when + ".txt";
		}


		////////////////

		public void OutputDirect( string file_name, string log_entry ) {
			lock( LogHelpers.MyLock ) {
				string path = this.GetLogPath();

				try {
					using( StreamWriter writer = File.AppendText( path + file_name ) ) {
						writer.WriteLine( log_entry );
					}
				} catch( Exception e ) {
					ErrorLogger.Log( "FALLBACK LOGGER ("+e.GetType().Name+"; "+file_name+") - " + log_entry );
				}
			}
		}
	}
}
