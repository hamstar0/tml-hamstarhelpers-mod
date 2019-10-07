using HamstarHelpers.Helpers.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to debugging and debug outputs.
	/// </summary>
	public partial class DebugHelpers {
		private static object MyRectLock = new object();

		////////////////

		private static IDictionary<string, Rectangle> Rects = new Dictionary<string, Rectangle>();
		private static IDictionary<string, int> RectsTime = new Dictionary<string, int>();
		private static IDictionary<string, int> RectsTimeStart = new Dictionary<string, int>();
		private static IDictionary<string, int> RectsShade = new Dictionary<string, int>();



		////////////////

		/// <summary>
		/// Draws a rectangle to screen.
		/// </summary>
		/// <param name="id">Unique identifier.</param>
		/// <param name="rect"></param>
		/// <param name="isWorldPos"></param>
		/// <param name="duration"></param>
		public static void DrawRect( string id, Rectangle rect, bool isWorldPos, int duration ) {
			lock( DebugHelpers.MyRectLock ) {
				if( isWorldPos ) {
					rect.X -= (int)Main.screenPosition.X;
					rect.Y -= (int)Main.screenPosition.Y;
				}

				DebugHelpers.Rects[id] = rect;
				DebugHelpers.RectsTime[id] = duration;
				DebugHelpers.RectsTimeStart[id] = duration;
				DebugHelpers.RectsShade[id] = 255;

				if( DebugHelpers.Rects.Count > 16 ) {
					foreach( string key in DebugHelpers.RectsTime.Keys.ToList() ) {
						if( DebugHelpers.RectsTime[key] > 0 ) { continue; }

						DebugHelpers.Rects.Remove( key );
						DebugHelpers.RectsTime.Remove( key );
						DebugHelpers.RectsTimeStart.Remove( key );
						DebugHelpers.RectsShade.Remove( key );

						if( DebugHelpers.Rects.Count <= 16 ) { break; }
					}
				}
			}
		}

		////////////////

		internal static void DrawAllRects( SpriteBatch sb ) {
			int yPos = 0;

			lock( DebugHelpers.MyRectLock ) {
				foreach( string key in DebugHelpers.Rects.Keys.ToList() ) {
					Rectangle rect = DebugHelpers.Rects[key];
					Color color = Color.White;

					if( DebugHelpers.RectsShade.ContainsKey(key) ) {
						int shade = DebugHelpers.RectsShade[key];
						if( DebugHelpers.RectsTime.ContainsKey(key) ) {
							float timeRatio = (float)DebugHelpers.RectsTime[key] / (float)DebugHelpers.RectsTimeStart[key];
							shade = (int)Math.Min( 255f, 255f * timeRatio );
						} else {
							DebugHelpers.RectsShade[key]--;
						}
						color.R = color.G = color.B = color.A = (byte)Math.Max(shade, 16);
					}

					HUDHelpers.DrawBorderedRect( sb, null, color, rect, 1 );

					if( DebugHelpers.RectsTime.ContainsKey(key) ) {
						if( DebugHelpers.RectsTime[key] > 0 ) {
							DebugHelpers.RectsTime[key]--;
						}
					}
					yPos += 24;
				}
			}
		}
	}
}
