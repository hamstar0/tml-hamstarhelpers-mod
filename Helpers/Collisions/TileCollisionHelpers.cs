using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Collisions {
	/// <summary>
	/// Assorted static "helper" functions pertaining to collisions between objects and/or tiles.
	/// </summary>
	public class TileCollisionHelpers {
		/// <summary>
		/// Measures world distance to the nearest tile from a given point and heading.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="direction"></param>
		/// <param name="maxDistance"></param>
		/// <param name="notFound"></param>
		/// <param name="ignoredTiles">A list of tile X and Y points to ignore.</param>
		/// <returns></returns>
		public static float MeasureWorldDistanceToTile( Vector2 position, Vector2 direction, float maxDistance,
					out bool notFound,
					List<Tuple<int, int>> ignoredTiles = null ) {
			int fromTileX = (int)position.X / 16;
			int fromTileY = (int)position.Y / 16;
			Vector2 from = position + direction * maxDistance;
			int toTileX = (int)from.X / 16;
			int toTileY = (int)from.Y / 16;

			Tuple<int, int> toTile;
			float dist;

			ignoredTiles = ignoredTiles ?? new List<Tuple<int, int>>();
			notFound = Collision.TupleHitLine( fromTileX, fromTileY, toTileX, toTileY, 0, 0, ignoredTiles, out toTile );

			if( notFound && toTile.Item1 == toTileX && toTile.Item2 == toTileY ) {
				dist = maxDistance;
			} else {
				dist = new Vector2( Math.Abs( fromTileX - toTile.Item1 ), Math.Abs( fromTileY - toTile.Item2 ) ).Length() * 16f;
			}

			return dist;
		}


		/// <summary>
		/// Attempts to find a path between 2 points using a simple brute force method.
		/// </summary>
		/// <param name="tilePointA"></param>
		/// <param name="tilePointB"></param>
		/// <param name="maxTilesCovered"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool FindPathSimple(
					(int x, int y) tilePointA,
					(int x, int y) tilePointB,
					int maxTilesCovered,
					out IList<(int tileX, int tileY)> path ) {
			path = new List<(int, int)>();
			var chartedTiles = new HashSet<(int, int)>();

			//

			int getDistSqr( (int x, int y) a, (int x, int y) b ) {
				int diffX = a.x - b.x;
				int diffY = a.y - b.y;
				return (diffX * diffX) + (diffY * diffY);
			}

			bool tryGetDistSqrOfValidTile( (int x, int y) point, out int distSqr ) {
				distSqr = (Main.maxTilesX * Main.maxTilesX) + (Main.maxTilesY * Main.maxTilesY);

				if( point.x < 0 || point.x >= Main.maxTilesX ) { return false; }
				if( point.y < 0 || point.y >= Main.maxTilesY ) { return false; }
				if( chartedTiles.Contains(point) ) { return false; }

				Tile tile = Framing.GetTileSafely( point.x, point.y );
				if( tile.active() && Main.tileSolid[tile.type] ) { return false; }

				distSqr = getDistSqr( point, tilePointB );
				return true;
			}

			IEnumerable<(int x, int y, int distSqr)> getDistSqrNeighborsNear( (int x, int y) point ) {
				(int x, int y) curr;
				int distSqr;

				curr = (point.x - 1, point.y - 1);
				if( tryGetDistSqrOfValidTile( curr, out distSqr) ) {
					yield return ( curr.x, curr.y, distSqr );
				}
				curr = (point.x, point.y - 1);
				if( tryGetDistSqrOfValidTile( curr, out distSqr) ) {
					yield return ( curr.x, curr.y, distSqr );
				}
				curr = (point.x + 1, point.y - 1);
				if( tryGetDistSqrOfValidTile( curr, out distSqr) ) {
					yield return ( curr.x, curr.y, distSqr );
				}
				curr = (point.x - 1, point.y);
				if( tryGetDistSqrOfValidTile( curr, out distSqr) ) {
					yield return ( curr.x, curr.y, distSqr );
				}
				curr = (point.x + 1, point.y);
				if( tryGetDistSqrOfValidTile( curr, out distSqr) ) {
					yield return ( curr.x, curr.y, distSqr );
				}
				curr = (point.x - 1, point.y + 1);
				if( tryGetDistSqrOfValidTile( curr, out distSqr) ) {
					yield return ( curr.x, curr.y, distSqr );
				}
				curr = (point.x, point.y + 1);
				if( tryGetDistSqrOfValidTile( curr, out distSqr) ) {
					yield return ( curr.x, curr.y, distSqr );
				}
				curr = (point.x + 1, point.y + 1);
				if( tryGetDistSqrOfValidTile( curr, out distSqr) ) {
					yield return ( curr.x, curr.y, distSqr );
				}
			}

			bool tryGetNeighborClosestToTarget( (int x, int y) point, out (int x, int y) closestNeighborToTarget ) {
				(int x, int y, int distSqr) prev = (-1, -1, Main.maxTilesX * Main.maxTilesX);
				IEnumerable<(int x, int y, int distSqr)> validNeighbors = getDistSqrNeighborsNear( point );

				foreach( (int x, int y, int distSqr) validNeighbor in validNeighbors ) {
					if( validNeighbor.distSqr < prev.distSqr ) {
						prev = validNeighbor;
					}
				}

				closestNeighborToTarget = (prev.x, prev.y);
				return closestNeighborToTarget.x != -1;
			}

			//

			(int x, int y) currPt = tilePointA;

			while( currPt.x != tilePointB.x || currPt.y != tilePointB.y ) {
				(int x, int y) closestNeighborToTarget;

				if( !tryGetNeighborClosestToTarget(currPt, out closestNeighborToTarget) ) {
					if( path.Count == 0 ) {
						return false;
					}
					if( chartedTiles.Count >= maxTilesCovered ) {
						return false;
					}

					chartedTiles.Add( currPt );

					currPt = path[ path.Count - 1 ];
					path.RemoveAt( path.Count - 1 );
					continue;
				}

				path.Add( currPt );
				chartedTiles.Add( currPt );

				currPt = closestNeighborToTarget;
			}

/*int blah = 120;
var pathCopy = path.ToList();
Timers.SetTimer( "blah_"+tilePointA.x+"_"+tilePointA.y, 4, false, () => {
	foreach( (int x, int y) node in pathCopy.ToList() ) {
		Dust.QuickDust( new Point(node.x, node.y), Color.Red );
	}
	return blah-- > 0;
} );*/
			return true;
		}
	}
}
