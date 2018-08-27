using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.DebugHelpers {
	public static partial class DebugHelpers {
		private static object MyRectLock = new object();

		////////////////
		
		public static IDictionary<string, Rectangle> Rects = new Dictionary<string, Rectangle>();
		public static IDictionary<string, int> RectsTime = new Dictionary<string, int>();
		public static IDictionary<string, int> RectsTimeStart = new Dictionary<string, int>();
		public static IDictionary<string, int> RectsShade = new Dictionary<string, int>();



		////////////////
		
		public static void DrawRect( string msg_label, Rectangle rect, bool is_world_pos, int duration ) {
			lock( DebugHelpers.MyRectLock ) {
				if( is_world_pos ) {
					rect.X -= (int)Main.screenPosition.X;
					rect.Y -= (int)Main.screenPosition.Y;
				}

				DebugHelpers.Rects[msg_label] = rect;
				DebugHelpers.RectsTime[msg_label] = duration;
				DebugHelpers.RectsTimeStart[msg_label] = duration;
				DebugHelpers.RectsShade[msg_label] = 255;

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
			int y_pos = 0;

			lock( DebugHelpers.MyRectLock ) {
				foreach( string key in DebugHelpers.Rects.Keys.ToList() ) {
					Rectangle rect = DebugHelpers.Rects[key];
					Color color = Color.White;

					if( DebugHelpers.RectsShade.ContainsKey(key) ) {
						int shade = DebugHelpers.RectsShade[key];
						if( DebugHelpers.RectsTime.ContainsKey(key) ) {
							float time_ratio = (float)DebugHelpers.RectsTime[key] / (float)DebugHelpers.RectsTimeStart[key];
							shade = (int)Math.Min( 255f, 255f * time_ratio );
						} else {
							DebugHelpers.RectsShade[key]--;
						}
						color.R = color.G = color.B = color.A = (byte)Math.Max(shade, 16);
					}

					HudHelpers.HudHelpers.DrawBorderedRect( sb, null, color, rect, 1 );

					if( DebugHelpers.RectsTime.ContainsKey(key) ) {
						if( DebugHelpers.RectsTime[key] > 0 ) {
							DebugHelpers.RectsTime[key]--;
						}
					}
					y_pos += 24;
				}
			}
		}
	}
}
