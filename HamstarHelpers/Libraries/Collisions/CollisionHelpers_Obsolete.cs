using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Collisions {
	/// <summary>
	/// Assorted static "helper" functions pertaining to general collision detecting and handling.
	/// </summary>
	public partial class CollisionHelpers {
		/// @private
		[Obsolete("use other CastRay", true)]
		public static bool CastRay(
					Vector2 worldPosition,
					Vector2 direction,
					float maxWorldDistance,
					Func<Vector2, bool> checkPerUnit=null,
					Func<int, int, bool> checkPerTile=null ) {
			return CollisionHelpers.CastRay( worldPosition, direction, maxWorldDistance, false, checkPerUnit, checkPerTile );
		}
	}
}
