using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.FX {
	/// <summary>
	/// Assorted static "helper" functions pertaining to visual effects.
	/// </summary>
	public class FxHelpers {
		/// <summary>
		/// Creates a lightning effect relative to the screen. Must be called in a Draw function.
		/// </summary>
		/// <param name="screenStartPos"></param>
		/// <param name="screenEndPos"></param>
		/// <param name="scale"></param>
		/// <param name="color"></param>
		public static void MakeScreenLightning( Vector2 screenStartPos, Vector2 screenEndPos, float scale, Color color ) {
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

		/// <summary>
		/// Creates a lightning effect. Must be called in a Draw function.
		/// </summary>
		/// <param name="wldStartPos"></param>
		/// <param name="wldEndPos"></param>
		/// <param name="scale"></param>
		/// <param name="color"></param>
		public static void MakeLightning( Vector2 wldStartPos, Vector2 wldEndPos, float scale, Color color ) {
			FxHelpers.MakeScreenLightning( wldStartPos-Main.screenPosition, wldEndPos-Main.screenPosition, scale, color );
		}
	}
}
