using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using HamstarHelpers.Helpers.HUD;


namespace HamstarHelpers.Helpers.Buffs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to buff HUD interface.
	/// </summary>
	public class BuffHUDHelpers {
		/// <summary>
		/// Gets all buff icon rectangles by buff index.
		/// </summary>
		/// <param name="applyGameZoom">Factors game zoom into position calculations.</param>
		/// <returns></returns>
		public static IDictionary<int, Rectangle> GetVanillaBuffIconRectanglesByPosition( bool applyGameZoom ) {
			return HUDElementHelpers.GetVanillaBuffIconRectanglesByPosition( applyGameZoom );
		}
	}
}
