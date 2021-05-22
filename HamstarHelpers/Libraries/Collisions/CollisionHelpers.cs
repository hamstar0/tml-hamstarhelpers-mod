using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Libraries.Collisions {
	/// <summary>
	/// Assorted static "helper" functions pertaining to general collision detecting and handling.
	/// </summary>
	public partial class CollisionLibraries {
		/// <summary>
		/// Casts a ray from a given world point along a given heading.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <param name="direction"></param>
		/// <param name="maxWorldDistance"></param>
		/// <param name="bothChecksOnly">If both checks return `true`, the cast ends.</param>
		/// <param name="checkPerUnit"></param>
		/// <param name="checkPerTile"></param>
		/// <returns>`true` if the checker function reports a collision (itself returns `true`).</returns>
		public static bool CastRay(
					Vector2 worldPosition,
					Vector2 direction,
					float maxWorldDistance,
					bool bothChecksOnly=false,
					Func<Vector2, bool> checkPerUnit=null,
					Func<int, int, bool> checkPerTile=null ) {
			float maxDistSqr = maxWorldDistance * maxWorldDistance;
			int tileX, tileY;
			int prevX = (int)worldPosition.X >> 4;
			int prevY = (int)worldPosition.Y >> 4;

			direction.Normalize();
			Vector2 newPosition = worldPosition + direction;
			bool unitFound = false, tileFound = false;

			while( Vector2.DistanceSquared( worldPosition, newPosition ) < maxDistSqr ) {
				tileX = (int)newPosition.X >> 4;
				tileY = (int)newPosition.Y >> 4;
				if( tileX < 0 || tileX >= Main.maxTilesX || tileY < 0 || tileY >= Main.maxTilesY ) {
					return false;
				}

				if( tileX != prevX || tileY != prevY ) {
					prevX = tileX;
					prevY = tileY;
				}

				if( !unitFound && (checkPerUnit?.Invoke(newPosition) ?? false) ) {
					unitFound = true;
				}
				if( !tileFound && (checkPerTile?.Invoke(tileX, tileY) ?? false) ) {
					tileFound = true;
				}

				if( bothChecksOnly ) {
					if( unitFound && tileFound ) {
						return true;
					}
				} else {
					if( unitFound || tileFound ) {
						return true;
					}
				}

				newPosition += direction;
			}

			return false;
		}
	}
}
