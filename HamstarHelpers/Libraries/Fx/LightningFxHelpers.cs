using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader;


namespace HamstarHelpers.Libraries.Fx {
	/// <summary>
	/// Assorted static "helper" functions pertaining to visual effects.
	/// </summary>
	public partial class LightningFxLibraries {
		/// <summary>
		/// Creates a lightning effect. Must be called in a Draw function.
		/// </summary>
		/// <param name="wldStartPos"></param>
		/// <param name="wldEndPos"></param>
		/// <param name="scale"></param>
		/// <param name="color"></param>
		public static void DrawLightning( Vector2 wldStartPos, Vector2 wldEndPos, float scale, Color color ) {
			LightningFxLibraries.DrawScreenLightning( wldStartPos - Main.screenPosition, wldEndPos - Main.screenPosition, scale, color );
		}

		/// <summary>
		/// Creates a lightning effect relative to the screen. Must be called in a Draw function.
		/// </summary>
		/// <param name="screenStartPos"></param>
		/// <param name="screenEndPos"></param>
		/// <param name="scale"></param>
		/// <param name="color"></param>
		public static void DrawScreenLightning( Vector2 screenStartPos, Vector2 screenEndPos, float scale, Color color ) {
			var rand = TmlLibraries.SafelyGetRand();
			var segs = new List<(Vector2 Beg, Vector2 End)>();

			Vector2 startPos = screenStartPos;

			while( (startPos - screenEndPos).LengthSquared() > 2304 ) {  //48^2
				Vector2 dir = Vector2.Normalize( screenEndPos - startPos );
				Vector2 reach = dir * rand.Next( 16, 80 );

				float remainingLenFromStartSqr = (screenEndPos - startPos).LengthSquared();
				if( reach.LengthSquared() > remainingLenFromStartSqr ) {
					break;
				}

				Vector2 testEndPos;
				float testLenSqr;
				do {
					testEndPos = startPos + reach;
					testEndPos += new Vector2( rand.Next(80) - 40, rand.Next(80) - 40 );

					testLenSqr = (screenEndPos - testEndPos).LengthSquared();
				} while( testLenSqr < 256 || testLenSqr > remainingLenFromStartSqr );

				segs.Add( (startPos, testEndPos) );

				startPos = testEndPos;
			}

			segs.Add( (startPos, screenEndPos) );

//LogHelpers.LogAndPrintOnce( "segs: "+segs.Count+", length: "+(screenStartPos-screenEndPos).Length()
//	+", scrStart: "+screenStartPos.ToShortString()+", scrEnd: "+screenEndPos.ToShortString()
//	+", segs: "+string.Join(", ", segs.Select(seg=>seg.Beg.ToShortString()+"->"+seg.End.ToShortString())));
			for( int i=0; i<segs.Count; i++ ) {
				LightningFxLibraries.DrawLightningBeam( segs[i].Beg, segs[i].End, scale, color );
			}
		}

		////

		private static void DrawLightningBeam( Vector2 screenStartPos, Vector2 screenEndPos, float scale, Color color ) {
			Texture2D tex = Main.extraTexture[33];

			DelegateMethods.c_1 = color;
			DelegateMethods.f_1 = 1f;

			Utils.DrawLaser(
				sb: Main.spriteBatch,
				tex: tex,
				start: screenStartPos,
				end: screenEndPos,
				scale: new Vector2( scale ),
				framing: new Utils.LaserLineFraming( DelegateMethods.LightningLaserDraw )
			);
		}
	}
}
