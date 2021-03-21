using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to locating things in the world.
	/// </summary>
	public partial class WorldLocationHelpers {
		private static Rectangle GetGiantTreeAt( int tileX, int tileY ) {
			int minY = 80;
			int maxY = WorldHelpers.SurfaceLayerBottomTileY - 50;

			Tile tile;
			int xLeft = tileX, xRight = tileX + 1;
			int yTop = tileY, yBottom = tileY + 1;

			bool foundColumn = true;
			for( int x = tileX; foundColumn; x-- ) {
				foundColumn = false;

				for( int y = minY; y < maxY; y++ ) {
					tile = Main.tile[ x, y ];
					if( tile == null || !tile.active() || (tile.type != TileID.LivingWood && tile.type != TileID.LeafBlock) ) {
						continue;
					}

					xLeft = x;
					yTop = y < yTop
						? y
						: yTop;
					yBottom = y > yBottom
						? y
						: yBottom;

					foundColumn = true;
				}
			}

			foundColumn = true;
			for( int x = tileX+1; foundColumn; x++ ) {
				foundColumn = false;

				for( int y = minY; y < maxY; y++ ) {
					tile = Main.tile[ x, y ];
					if( tile == null || !tile.active() || (tile.type != TileID.LivingWood && tile.type != TileID.LeafBlock) ) {
						continue;
					}

					xRight = x;
					yTop = y < yTop
						? y
						: yTop;
					yBottom = y > yBottom
						? y
						: yBottom;

					foundColumn = true;
				}
			}

			return new Rectangle(
				xLeft,
				yBottom,
				(xRight - xLeft),
				(yBottom - yTop)
			);
		}


		////////////////

		/// <summary>
		/// Gets rectangles containing all giant trees in the world.
		/// </summary>
		/// <returns></returns>
		public static IList<Rectangle> GetGiantTrees() {
			IDictionary<int, Rectangle> trees = new Dictionary<int, Rectangle>();

			int minX = 300;
			int maxX = Main.maxTilesX - 300;
			int minY = 80;
			int maxY = WorldHelpers.SurfaceLayerBottomTileY - 50;
			int midX1 = ( Main.maxTilesX / 2 ) - 100;
			int midX2 = ( Main.maxTilesX / 2 ) + 100;
			Tile tile;

			for( int x=minX; x<maxX; x++ ) {
				if( x > midX1 && x < midX2 ) {
					x = midX2;
				}

				if( trees.ContainsKey( x ) ) {
					x += trees[x].Width;
				}
				
				for( int y=minY; y<maxY; y++ ) {
					tile = Main.tile[x, y];
					if( tile == null || !tile.active() || (tile.type != TileID.LivingWood && tile.type != TileID.LeafBlock) ) {
						continue;
					}
					
					Rectangle tree = WorldLocationHelpers.GetGiantTreeAt( x, y );
					trees[ tree.X ] = tree;

					x = tree.X + tree.Width + 1;
					break;
				}
			}
			
			return trees.Values.ToList();
		}
	}
}
