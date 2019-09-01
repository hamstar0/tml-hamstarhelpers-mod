using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.HUD {
	/// <summary>
	/// Assorted static "helper" functions pertaining to in-game HUD elements. 
	/// </summary>
	public class HUDElementHelpers {
		/// <summary>
		/// Top left screen position of player's health.
		/// </summary>
		/// <returns></returns>
		public static Vector2 GetVanillaHealthDisplayScreenPosition() =>
			new Vector2( Main.screenWidth - 300, 86 );


		/// <summary>
		/// Top left screen position of player's accessories.
		/// </summary>
		/// <returns></returns>
		public static Vector2 GetVanillaAccessorySlotScreenPosition( int slot ) {
			var pos = new Vector2( Main.screenWidth - 92, 310 );
			pos.Y += ( 48 * slot );

			return pos;
		}
	}
}
