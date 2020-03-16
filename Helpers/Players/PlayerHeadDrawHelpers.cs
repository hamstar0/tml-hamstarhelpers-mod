using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to player head drawing.
	/// </summary>
	public class PlayerHeadDrawHelpers {
		/// <summary>
		/// Draws a player's head.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically `Main.spriteBatch`.</param>
		/// <param name="player"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="alpha"></param>
		/// <param name="scale"></param>
		public static void DrawPlayerHead( SpriteBatch sb, Player player, float x, float y, float alpha = 1f, float scale = 1f ) {
			object _;
			ReflectionHelpers.RunMethod( Main.instance, "DrawPlayerHead", new object[] { player, x, y, alpha, scale }, out _ );
		}
	}
}
