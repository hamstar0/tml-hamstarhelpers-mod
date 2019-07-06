using System;
using Terraria;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogHelpers {
		private static string FormatMessage( string msg ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			var logHelpers = mymod.LogHelpers;

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

				return logged + msg;
			} catch( Exception e ) {
				return "FORMATTING ERROR ("+e.GetType().Name+") - "+msg;
			}
		}


		////////////////

		private static void DirectInfo( string msg ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			var logHelpers = mymod.LogHelpers;

			try {
				lock( LogHelpers.MyLock ) {
					mymod.Logger.Info( msg );
					//ErrorLogger.Log( logged + msg );
				}
			} catch( Exception e ) {
				try {
					lock( LogHelpers.MyLock ) {
						mymod.Logger.Info( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );
						//ErrorLogger.Log( "FALLBACK LOGGER 2 (" + e.GetType().Name + ") " + msg );
					}
				} catch { }
			}
		}

		private static void DirectAlert( string msg ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			var logHelpers = mymod.LogHelpers;

			try {
				lock( LogHelpers.MyLock ) {
					mymod.Logger.Error( msg );
				}
			} catch( Exception e ) {
				try {
					lock( LogHelpers.MyLock ) {
						mymod.Logger.Error( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );
					}
				} catch { }
			}
		}

		private static void DirectWarn( string msg ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			var logHelpers = mymod.LogHelpers;

			try {
				lock( LogHelpers.MyLock ) {
					mymod.Logger.Fatal( msg );
				}
			} catch( Exception e ) {
				try {
					lock( LogHelpers.MyLock ) {
						mymod.Logger.Fatal( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );
					}
				} catch { }
			}
		}
	}
}
