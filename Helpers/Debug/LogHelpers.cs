using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogHelpers {
		public static void Log( string msg="" ) {
			lock( LogHelpers.MyLock ) {
				ModHelpersMod mymod = ModHelpersMod.Instance;
				mymod.Logger.Info( LogHelpers.FormatMessage( msg ) );
			}
		}

		public static void Alert( string msg="" ) {
			lock( LogHelpers.MyLock ) {
				ModHelpersMod mymod = ModHelpersMod.Instance;
				mymod.Logger.Error( LogHelpers.FormatMessage( msg ) );
			}
			LogHelpers.Log( DebugHelpers.GetCurrentContext( 2 ) + ((msg != "") ? " - " + msg : "") );
		}

		public static void Warn( string msg="" ) {
			lock( LogHelpers.MyLock ) {
				ModHelpersMod mymod = ModHelpersMod.Instance;
				mymod.Logger.Fatal( LogHelpers.FormatMessage( msg ) );
			}
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
