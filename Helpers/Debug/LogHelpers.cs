using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Debug {
	public partial class LogHelpers {
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
						lock( LogHelpers.MyLock ) {
							ErrorLogger.Log( logged + msg );
						}
					}
				} else {
					lock( LogHelpers.MyLock ) {
						ErrorLogger.Log( logged + msg );
					}
				}

				logHelpers.LoggedMessages++;
			} catch( Exception e ) {
				try {
					lock( LogHelpers.MyLock ) {
						ErrorLogger.Log( "FALLBACK LOGGER 2 (" + e.GetType().Name + ") " + msg );
					}
				} catch { }
			}
		}

		public static void Alert( string msg="" ) {
			LogHelpers.Log( DebugHelpers.GetCurrentContext( 2 ) + ((msg != "") ? " - " + msg : "") );
		}

		public static void Warn( string msg="" ) {
			LogHelpers.Log( "!" + DebugHelpers.GetCurrentContext( 2 ) + ((msg != "") ? " - " + msg: "") );
		}


		////

		public static void LogOnce( string msg ) {
			var logHelpers = ModHelpersMod.Instance.LogHelpers;
			bool isShown = false;

			lock( LogHelpers.MyLock ) {
				if( !logHelpers.UniqueMessages.ContainsKey( msg ) ) {
					logHelpers.UniqueMessages[msg] = 1;
					isShown = true;
				} else {
					logHelpers.UniqueMessages[msg]++;

					if( ( Math.Log10( logHelpers.UniqueMessages[msg] ) % 1d ) == 0 ) {
						msg = "(" + logHelpers.UniqueMessages[msg] + "th) " + msg;
						isShown = true;
					}
				}
			}

			if( isShown ) {
				LogHelpers.Log( "~" + msg );
			}
		}
		
		public static void AlertOnce( string msg = "" ) {
			LogHelpers.LogOnce( DebugHelpers.GetCurrentContext( 2 ) + ( ( msg != "" ) ? " - " + msg : "" ) );
		}

		public static void WarnOnce( string msg = "" ) {
			LogHelpers.LogOnce( "!" + DebugHelpers.GetCurrentContext( 2 ) + ( ( msg != "" ) ? " - " + msg : "" ) );
		}

		////

		public static bool LogOnceReset( string msg ) {
			lock( LogHelpers.MyLock ) {
				return ModHelpersMod.Instance.LogHelpers.UniqueMessages.Remove( msg );
			}
		}
	}
}
