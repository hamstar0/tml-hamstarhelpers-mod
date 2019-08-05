using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to debugging and debug outputs.
	/// </summary>
	public partial class DebugHelpers {
		private static object MyPrintLock = new object();

		////////////////

		private static IDictionary<string, string> Texts = new Dictionary<string, string>();
		private static IDictionary<string, int> TextTimes = new Dictionary<string, int>();
		private static IDictionary<string, int> TextTimeStart = new Dictionary<string, int>();
		private static IDictionary<string, int> TextShade = new Dictionary<string, int>();



		////////////////
		
		/// <summary>
		/// Prints a message to the screen for the given tick duration. Message overlaps chat area. Repeat calls to display a message of a
		/// given label merely update that existing message.
		/// </summary>
		/// <param name="msgLabel">Identifier of the given message. Calling `Print(...)` with this same identifier replaces any existing
		/// displayed message of this identifier.</param>
		/// <param name="msg">Message to display.</param>
		/// <param name="tickDuration">Tick duration to display the given message.</param>
		public static void Print( string msgLabel, string msg, int tickDuration ) {
			lock( DebugHelpers.MyPrintLock ) {
				DebugHelpers.Texts[msgLabel] = msg;
				DebugHelpers.TextTimes[msgLabel] = tickDuration;
				DebugHelpers.TextTimeStart[msgLabel] = tickDuration;
				DebugHelpers.TextShade[msgLabel] = 255;

				if( DebugHelpers.Texts.Count > 16 ) {
					foreach( string key in DebugHelpers.TextTimes.Keys.ToList() ) {
						if( DebugHelpers.TextTimes[key] > 0 ) { continue; }

						DebugHelpers.Texts.Remove( key );
						DebugHelpers.TextTimes.Remove( key );
						DebugHelpers.TextTimeStart.Remove( key );
						DebugHelpers.TextShade.Remove( key );

						if( DebugHelpers.Texts.Count <= 16 ) { break; }
					}
				}
			}
		}

		////////////////

		internal static void PrintAll( SpriteBatch sb ) {
			int yPos = 0;

			lock( DebugHelpers.MyPrintLock ) {
				foreach( string key in DebugHelpers.Texts.Keys.ToList() ) {
					string msg = key + ":  " + DebugHelpers.Texts[key];
					Color color = Color.White;

					if( DebugHelpers.TextShade.ContainsKey(key) ) {
						int shade = DebugHelpers.TextShade[key];
						if( DebugHelpers.TextTimes.ContainsKey(key) ) {
							float timeRatio = (float)DebugHelpers.TextTimes[key] / (float)DebugHelpers.TextTimeStart[key];
							shade = (int)Math.Min( 255f, 255f * timeRatio );
						} else {
							DebugHelpers.TextShade[key]--;
						}
						color.R = color.G = color.B = color.A = (byte)Math.Max(shade, 16);
						color.R = (byte)Math.Min( (int)((float)color.R * 1.25f), 255 );
					}

					sb.DrawString( Main.fontMouseText, msg, new Vector2( 8, (Main.screenHeight - 32) - yPos ), color );

					if( DebugHelpers.TextTimes.ContainsKey(key) ) {
						if( DebugHelpers.TextTimes[key] > 0 ) {
							DebugHelpers.TextTimes[key]--;
						}
					}
					yPos += 24;
				}
			}
		}
	}
}
