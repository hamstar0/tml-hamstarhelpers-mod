using HamstarHelpers.Libraries.Draw;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to debugging and debug outputs.
	/// </summary>
	public partial class DebugLibraries {
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
			lock( DebugLibraries.MyRectLock ) {
				if( isWorldPos ) {
					rect.X -= (int)Main.screenPosition.X;
					rect.Y -= (int)Main.screenPosition.Y;
				}

				DebugLibraries.Rects[id] = rect;
				DebugLibraries.RectsTime[id] = duration;
				DebugLibraries.RectsTimeStart[id] = duration;
				DebugLibraries.RectsShade[id] = 255;

				if( DebugLibraries.Rects.Count > 16 ) {
					foreach( string key in DebugLibraries.RectsTime.Keys.ToList() ) {
						if( DebugLibraries.RectsTime[key] > 0 ) { continue; }

						DebugLibraries.Rects.Remove( key );
						DebugLibraries.RectsTime.Remove( key );
						DebugLibraries.RectsTimeStart.Remove( key );
						DebugLibraries.RectsShade.Remove( key );

						if( DebugLibraries.Rects.Count <= 16 ) { break; }
					}
				}
			}
		}

		////////////////

		internal static void DrawAllRects( SpriteBatch sb ) {
			int yPos = 0;
			var rects = new List<(Rectangle, Color)>();

			lock( DebugLibraries.MyRectLock ) {
				foreach( string key in DebugLibraries.Rects.Keys.ToList() ) {
					Rectangle rect = DebugLibraries.Rects[key];
					Color color = Color.White;

					if( DebugLibraries.RectsShade.ContainsKey(key) ) {
						int shade = DebugLibraries.RectsShade[key];
						if( DebugLibraries.RectsTime.ContainsKey(key) ) {
							float timeRatio = (float)DebugLibraries.RectsTime[key] / (float)DebugLibraries.RectsTimeStart[key];
							shade = (int)Math.Min( 255f, 255f * timeRatio );
						} else {
							DebugLibraries.RectsShade[key]--;
						}
						color.R = color.G = color.B = color.A = (byte)Math.Max(shade, 16);
					}

					rects.Add( (rect, color) );

					if( DebugLibraries.RectsTime.ContainsKey(key) ) {
						if( DebugLibraries.RectsTime[key] > 0 ) {
							DebugLibraries.RectsTime[key]--;
						}
					}
					yPos += 24;
				}
			}

			foreach( (Rectangle rect, Color color) in rects ) {
				DrawLibraries.DrawBorderedRect( sb, null, color, rect, 1 );
			}
		}
	}
}
