using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;


namespace HamstarHelpers.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to debugging and debug outputs.
	/// </summary>
	public partial class DebugLibraries {
		private static object MyPrintLock = new object();

		////////////////

		private static IDictionary<string, string> Texts = new Dictionary<string, string>();
		private static IDictionary<string, int> TextTimes = new Dictionary<string, int>();
		private static IDictionary<string, int> TextTimeStart = new Dictionary<string, int>();
		private static IDictionary<string, int> TextShade = new Dictionary<string, int>();



		////////////////

		/// <summary>
		/// Prints a message to the screen for the given tick duration. Message overlaps chat area. Repeat calls to display a
		/// message of a given label merely update that existing message.
		/// </summary>
		/// <param name="labelAndMsg">Identifier and message to display. Identifier is delineated by first occurrence of `.`
		/// in the string. Calling `Print(...)` with this same identifier replaces any existing displayed message of this
		/// identifier.</param>
		public static void Print( string labelAndMsg ) {
			string[] msgSplit = labelAndMsg.Split( '.' );
			string label = "";
			string msg = labelAndMsg;
			
			if( msgSplit.Length > 0 ) {
				label = msgSplit[0];

				string[] afterMsgSplit = new string[ msgSplit.Length - 1 ];
				Array.Copy(
					sourceArray: msgSplit,
					sourceIndex: 1,
					destinationArray: afterMsgSplit,
					destinationIndex: 0,
					length: afterMsgSplit.Length
				);

				msg = string.Join( ".", afterMsgSplit );
			}

			DebugLibraries.Print( label, msg );
		}

		/// <summary>
		/// Prints a message to the screen for the given tick duration. Message overlaps chat area. Repeat calls to display a message of a
		/// given label merely update that existing message.
		/// </summary>
		/// <param name="msgLabel">Identifier of the given message. Calling `Print(...)` with this same identifier replaces any existing
		/// displayed message of this identifier.</param>
		/// <param name="msg">Message to display.</param>
		public static void Print( string msgLabel, string msg ) {
			DebugLibraries.Print( msgLabel, msg, 20 );
		}

		/// <summary>
		/// Prints a message to the screen for the given tick duration. Message overlaps chat area. Repeat calls to display a message of a
		/// given label merely update that existing message.
		/// </summary>
		/// <param name="msgLabel">Identifier of the given message. Calling `Print(...)` with this same identifier replaces any existing
		/// displayed message of this identifier.</param>
		/// <param name="msg">Message to display.</param>
		/// <param name="tickDuration">Tick duration to display the given message.</param>
		public static void Print( string msgLabel, string msg, int tickDuration ) {
			lock( DebugLibraries.MyPrintLock ) {
				DebugLibraries.Texts[msgLabel] = msg;
				DebugLibraries.TextTimes[msgLabel] = tickDuration;
				DebugLibraries.TextTimeStart[msgLabel] = tickDuration;
				DebugLibraries.TextShade[msgLabel] = 255;

				if( DebugLibraries.Texts.Count > 16 ) {
					foreach( string key in DebugLibraries.TextTimes.Keys.ToList() ) {
						if( DebugLibraries.TextTimes[key] > 0 ) { continue; }

						DebugLibraries.Texts.Remove( key );
						DebugLibraries.TextTimes.Remove( key );
						DebugLibraries.TextTimeStart.Remove( key );
						DebugLibraries.TextShade.Remove( key );

						if( DebugLibraries.Texts.Count <= 16 ) { break; }
					}
				}
			}
		}

		////////////////

		internal static void PrintAll( SpriteBatch sb ) {
			int yPos = 0;
			var strings = new List<(string, Vector2, Color)>();

			lock( DebugLibraries.MyPrintLock ) {
				foreach( string key in DebugLibraries.Texts.Keys.ToList() ) {
					string msg = key + ":  " + DebugLibraries.Texts[key];
					Color color = Color.White;

					if( DebugLibraries.TextShade.ContainsKey(key) ) {
						int shade = DebugLibraries.TextShade[key];
						if( DebugLibraries.TextTimes.ContainsKey(key) ) {
							float timeRatio = (float)DebugLibraries.TextTimes[key] / (float)DebugLibraries.TextTimeStart[key];
							shade = (int)Math.Min( 255f, 255f * timeRatio );
						} else {
							DebugLibraries.TextShade[key]--;
						}
						color.R = color.G = color.B = color.A = (byte)Math.Max(shade, 16);
						color.R = (byte)Math.Min( (int)((float)color.R * 1.25f), 255 );
					}

					var pos = new Vector2( 8, (Main.screenHeight - 32) - yPos );

					strings.Add( (msg, pos, color) );

					if( DebugLibraries.TextTimes.ContainsKey(key) ) {
						if( DebugLibraries.TextTimes[key] > 0 ) {
							DebugLibraries.TextTimes[key]--;
						}
					}
					yPos += 24;
				}
			}

			foreach( (string msg, Vector2 pos, Color color) in strings ) {
				sb.DrawString( Main.fontMouseText, msg, pos, color );
			}
		}
	}
}
