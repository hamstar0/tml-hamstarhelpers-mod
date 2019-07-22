using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Collisions {
	/// <summary>
	/// Assorted static "helper" functions pertaining to collisions between objects and/or tiles.
	/// </summary>
	public class TileCollisionHelpers {
		/// <summary>
		/// Measures world distance to a given tile from a given point and heading.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="direction"></param>
		/// <param name="maxDistance"></param>
		/// <param name="notFound"></param>
		/// <param name="ignoredTiles">A list of tile X and Y points to ignore.</param>
		/// <returns></returns>
		public static float MeasureWorldDistanceToTile( Vector2 position, Vector2 direction, float maxDistance,
					out bool notFound, List<Tuple<int, int>> ignoredTiles = null ) {
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
	}
}
