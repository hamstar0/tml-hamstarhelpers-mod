using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.DebugHelpers {
	public static class DebugHelpers {
		internal static bool Once;
		internal static int OnceInAWhile;
		public static IDictionary<string, string> Display = new Dictionary<string, string>();
		public static IDictionary<string, int> DisplayTime = new Dictionary<string, int>();
		public static IDictionary<string, int> DisplayTimeStart = new Dictionary<string, int>();
		public static IDictionary<string, int> DisplayShade = new Dictionary<string, int>();
		
		private static int LoggedMessages = 0;


		////////////////

		public static void MsgOnce( string msg ) {
			if( DebugHelpers.Once ) { return; }
			DebugHelpers.Once = true;

			Main.NewText( msg );
		}

		public static void MsgOnceInAWhile( string msg ) {
			if( DebugHelpers.OnceInAWhile > 0 ) { return; }
			DebugHelpers.OnceInAWhile = 60 * 10;

			Main.NewText( msg );
		}


		////////////////

		public static void SetDisplay( string msg_label, string msg, int duration ) {
			DebugHelpers.Display[msg_label] = msg;
			DebugHelpers.DisplayTime[msg_label] = duration;
			DebugHelpers.DisplayTimeStart[msg_label] = duration;
			DebugHelpers.DisplayShade[msg_label] = 255;
			
			if( DebugHelpers.Display.Count > 16 ) {
				foreach( string key in DebugHelpers.DisplayTime.Keys.ToList() ) {
					if( DebugHelpers.DisplayTime[key] > 0 ) { continue; }

					DebugHelpers.Display.Remove( key );
					DebugHelpers.DisplayTime.Remove( key );
					DebugHelpers.DisplayTimeStart.Remove( key );
					DebugHelpers.DisplayShade.Remove( key );

					if( DebugHelpers.Display.Count <= 16 ) { break; }
				}
			}
		}

		public static void PrintToBatch( SpriteBatch sb ) {
			int y_pos = 0;

			foreach( string key in DebugHelpers.Display.Keys.ToList() ) {
				string msg = key + ":  " + DebugHelpers.Display[key];
				Color color = Color.White;

				if( DebugHelpers.DisplayShade.ContainsKey(key) ) {
					int shade = DebugHelpers.DisplayShade[key];
					if( DebugHelpers.DisplayTime.ContainsKey(key) ) {
						float time_ratio = (float)DebugHelpers.DisplayTime[key] / (float)DebugHelpers.DisplayTimeStart[key];
						shade = (int)Math.Min( 255f, 255f * time_ratio );
					} else {
						DebugHelpers.DisplayShade[key]--;
					}
					color.R = color.G = color.B = color.A = (byte)Math.Max(shade, 16);
				}

				sb.DrawString( Main.fontMouseText, msg, new Vector2( 8, (Main.screenHeight - 32) - y_pos ), color );

				if( DebugHelpers.DisplayTime.ContainsKey(key) ) {
					if( DebugHelpers.DisplayTime[key] > 0 ) {
						DebugHelpers.DisplayTime[key]--;
					}
				}
				y_pos += 24;
			}
		}


		////////////////

		public static void Log( string msg ) {
			string now = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds.ToString( "N2" );
			string logged = "" + Main.netMode + ":" + DebugHelpers.LoggedMessages + " " + now;
			logged += new String( ' ', 24 - logged.Length );

			ErrorLogger.Log( logged+msg );

			DebugHelpers.LoggedMessages++;
		}
	}
}
