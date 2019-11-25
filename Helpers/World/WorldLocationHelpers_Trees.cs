using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to locating things in the world.
	/// </summary>
	public partial class WorldLocationHelpers {
		private static Rectangle GetGiantTreeAt( int x, int y ) {
			var rect = new Rectangle( x, y, 1, 1 );

			int minY = 80;
			int maxY = WorldHelpers.SurfaceLayerBottomTileY - 50;
			Tile tile;
			bool foundColumn = true;

			for( int x2 = x; foundColumn; x2-- ) {
				foundColumn = false;

				for( int y2 = minY; y2 < maxY; y2++ ) {
					tile = Framing.GetTileSafely( x2, y2 );
					if( !tile.active() || ( tile.type != TileID.LivingWood && tile.type != TileID.LeafBlock ) ) {
						continue;
					}

					rect.X = x2;
					rect.Y = rect.Y > y2 ? y2 : rect.Y;
					rect.Height = rect.Y - minY;
					foundColumn = true;
				}

				if( !foundColumn ) {
					break;
				}
			}

			for( int x2 = x+1; foundColumn; x2++ ) {
				foundColumn = false;

				for( int y2 = minY; y2 < maxY; y2++ ) {
					tile = Framing.GetTileSafely( x2, y2 );
					if( !tile.active() || ( tile.type != TileID.LivingWood && tile.type != TileID.LeafBlock ) ) {
						continue;
					}

					rect.Width = x2 - rect.X;
					rect.Y = rect.Y > y2 ? y2 : rect.Y;
					rect.Height = rect.Y - minY;
					foundColumn = true;
				}
			}

			return rect;
		}


		////////////////

		/// <summary>
		/// Gets rectangles containing all giant trees in the world.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Rectangle> GetGiantTrees() {
			IDictionary<int, Rectangle> trees = new Dictionary<int, Rectangle>();

			int minX = 300;
			int minY = 80;
			int maxX = Main.maxTilesX - 300;
			int maxY = WorldHelpers.SurfaceLayerBottomTileY - 50;
			Tile tile;

			for( int x=minX; x<maxX; x++ ) {
				if( x > (Main.maxTilesX / 2) - 100 ) {
					x = (Main.maxTilesX / 2) + 100;
					continue;
				}

				if( trees.ContainsKey( x ) ) {
					x += trees[x].Width;
					continue;
				}

				for( int y=minY; y<maxY; y++ ) {
					tile = Framing.GetTileSafely( x, y );
					if( !tile.active() || (tile.type != TileID.LivingWood && tile.type != TileID.LeafBlock) ) {
						continue;
					}

					Rectangle tree = WorldLocationHelpers.GetGiantTreeAt( x, y );
					trees[ tree.X ] = tree;

					x = tree.X + tree.Width;
					break;
				}
			}

			return trees.Values;
		}
	}
}
