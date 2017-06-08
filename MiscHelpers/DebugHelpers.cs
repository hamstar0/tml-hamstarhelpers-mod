using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.MiscHelpers {
	public static class DebugHelpers {
		public static bool Once;
		public static int OnceInAWhile;
		public static IDictionary<string, string> Display = new Dictionary<string, string>();

		private static int Logged = 0;


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

		
		public static void PrintToBatch( SpriteBatch sb ) {
			int i = 0;

			foreach( string key in DebugHelpers.Display.Keys.ToList() ) {
				string msg = key + ":  " + DebugHelpers.Display[key];
				sb.DrawString( Main.fontMouseText, msg, new Vector2( 8, (Main.screenHeight - 32) - (i * 24) ), Color.White );
				i++;
			}
		}
		

		public static void Log( string msg ) {
			string now = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds.ToString( "N2" );
			string logged = "" + Main.netMode + ":" + DebugHelpers.Logged + " " + now;
			logged += new String( ' ', 24 - logged.Length );

			ErrorLogger.Log( logged+msg );

			DebugHelpers.Logged++;
		}
	}
}
