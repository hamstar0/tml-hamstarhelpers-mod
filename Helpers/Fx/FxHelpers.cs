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
		/// Creates a lightning effect relative to the screen.
		/// </summary>
		/// <param name="screenStartPos"></param>
		/// <param name="screenEndPos"></param>
		/// <param name="scale"></param>
		/// <param name="color"></param>
		public static void MakeLightning( Vector2 screenStartPos, Vector2 screenEndPos, float scale, Color color ) {
			Texture2D tex = Main.extraTexture[33];

			DelegateMethods.c_1 = color;
			DelegateMethods.f_1 = 1f;

			Utils.DrawLaser(
				sb: Main.spriteBatch,
				tex: tex,
				start: screenStartPos,
				end: screenEndPos,
				scale: new Vector2(scale),
				framing: new Utils.LaserLineFraming( DelegateMethods.LightningLaserDraw )
			);
		}
	}
}
