using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to player head drawing (currently empty).
	/// </summary>
	public class PlayerHeadDrawHelpers {
		/// @private
		[Obsolete( "uses vanilla's Main.instance.DrawPlayerHead via. reflection; not library code", true)]
		public static void DrawPlayerHead( SpriteBatch sb, Player player, float x, float y, float alpha = 1f, float scale = 1f ) {
			object _;
			ReflectionHelpers.RunMethod( Main.instance, "DrawPlayerHead", new object[] { player, x, y, alpha, scale }, out _ );
		}
	}
}
