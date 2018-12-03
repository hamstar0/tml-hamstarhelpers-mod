using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DebugHelpers {
	public class LogHelpers {
		private static object MyLock = new object();

		
		////////////////

		public static void Log( string msg ) {
			try {
				ModHelpersMod mymod = ModHelpersMod.Instance;
				var logHelpers = mymod.LogHelpers;

				double nowSeconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds - logHelpers.StartTime;

				string nowSecondsWhole = ( (int)nowSeconds ).ToString( "D6" );
				string nowSecondsDecimal = ( nowSeconds - (int)nowSeconds ).ToString( "N2" );
				string now = nowSecondsWhole + "." + ( nowSecondsDecimal.Length > 2 ? nowSecondsDecimal.Substring( 2 ) : nowSecondsDecimal );

				string from = Main.myPlayer.ToString( "D3" );
				string logged = Main.netMode + ":" + from + ":" + logHelpers.LoggedMessages.ToString( "D5" ) + " - " + now;
				if( logged.Length < 26 ) {
					logged += new String( ' ', 26 - logged.Length );
				} else {
					logged += "  ";
				}

				if( mymod.Config.UseCustomLogging ) {
					logHelpers.OutputDirect( logHelpers.GetHourlyLogFileName(), logged + msg );

					if( mymod.Config.UseCustomLoggingPerNetMode ) {
						logHelpers.OutputDirect( logHelpers.GetModalLogFileName(), logged + msg );
					}

					if( mymod.Config.UseAlsoNormalLogging ) {
						ErrorLogger.Log( logged + msg );
					}
				} else {
					ErrorLogger.Log( logged + msg );
				}

				logHelpers.LoggedMessages++;
			} catch( Exception e ) {
				try {
					ErrorLogger.Log( "FALLBACK LOGGER 2 (" + e.GetType().Name + ") " + msg );
				} catch { }
			}
		}


		public static void LogOnce( string msg ) {
			var logHelpers = ModHelpersMod.Instance.LogHelpers;
			bool isShown = false;

			lock( LogHelpers.MyLock ) {
				if( !logHelpers.UniqueMessages.Contains( msg ) ) {
					logHelpers.UniqueMessages.Add( msg );
					isShown = true;
				}
			}

			if( isShown ) {
				LogHelpers.Log( "~" + msg );
			}
		}



		////////////////

		private int LoggedMessages;
		private DateTime StartTimeBase;
		private double StartTime;

		private ISet<string> UniqueMessages = new HashSet<string>();


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
			this.StartTime = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;
		}

		////////////////

		public string GetLogPath() {
			string basePath = ErrorLogger.LogPath + Path.DirectorySeparatorChar;
			string fullPath = basePath + "History" + Path.DirectorySeparatorChar;

			Directory.CreateDirectory( basePath );
			Directory.CreateDirectory( fullPath );

			return fullPath;
		}

		public string GetHourlyLogFileName() {
			DateTime currHour = this.StartTimeBase;
			currHour.AddMinutes( -currHour.Minute );
			currHour.AddSeconds( -currHour.Second );
			currHour.AddMilliseconds( -currHour.Millisecond );

			string when = currHour.ToString( "MM-dd, HH" );

			return "Log Any " + when + ".txt";
		}

		public string GetModalLogFileName() {
			string mode = "";

			if( Main.dedServ ) {
				mode += "Dedi ";
			}
			
			switch( Main.netMode ) {
			case 0:
				mode += "Single";
				break;
			case 1:
				mode += "Client";
				break;
			case 2:
				mode += "Server";
				break;
			default:
				mode += "Unknown Mode";
				break;
			}

			//string when = this.StartTimeBase.ToString( "MM-dd, HH.mm.ss" );
			string when = this.StartTimeBase.ToString( "MM-dd, HH.mm" );

			return "Log " + mode + " " + when + ".txt";
		}


		////////////////

		public void OutputDirect( string fileName, string logEntry ) {
			lock( LogHelpers.MyLock ) {
				string path = this.GetLogPath();

				try {
					using( StreamWriter writer = File.AppendText( path + fileName ) ) {
						writer.WriteLine( logEntry );
					}
				} catch( Exception e ) {
					ErrorLogger.Log( "FALLBACK LOGGER ("+e.GetType().Name+"; "+fileName+") - " + logEntry );
				}
			}
		}
	}
}
