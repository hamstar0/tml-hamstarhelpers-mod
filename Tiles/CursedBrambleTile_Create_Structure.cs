/*using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;


namespace HamstarHelpers.Tiles {
	/// <summary>
	/// Represents a tile that works like a standard corruption/crimson/jungle bramble, but cannot be removed by melee weapons,
	/// and may support additional custom behavior.
	/// </summary>
	public partial class CursedBrambleTile : ModTile {
		public static IDictionary<int, ISet<int>> TraceTileWall( int tileX, int tileY, int length, bool horizontal ) {
			var tilePositions = new Dictionary<int, ISet<int>>();
			int halfLen = length / 2;

			if( horizontal ) {
				for( int x = 0; x < halfLen; x++ ) {
					int y = x < 5 ? -3 : 0;

					tilePositions.Set2D( tileX + x, tileY + y );
					tilePositions.Set2D( tileX - x, tileY + y );
				}
			} else {
				for( int y = 0; y < halfLen; y++ ) {
					int x = y < 5 ? -3 : 0;

					tilePositions.Set2D( tileX + x, tileY + y );
					tilePositions.Set2D( tileX + x, tileY - y );
				}
			}

			return tilePositions;
		}


		public static IDictionary<int, ISet<int>> TraceTileEnclosure( int tileX, int tileY, int radius ) {
			var tilePositionOffets = new Dictionary<int, ISet<int>>();
			var tilePositions = new Dictionary<int, ISet<int>>();

			// Trace outer rectangle
			for( int x = 0; x < radius; x++ ) {
				tilePositionOffets.Set2D( -x, -radius );
				tilePositionOffets.Set2D( -x, radius );
				tilePositionOffets.Set2D( x, -radius );
				tilePositionOffets.Set2D( x, radius );
			}
			for( int y = 0; y < radius; y++ ) {
				tilePositionOffets.Set2D( -radius, -y );
				tilePositionOffets.Set2D( -radius, y );
				tilePositionOffets.Set2D( radius, -y );
				tilePositionOffets.Set2D( radius, y );
			}

			// Compress rectangle
			foreach( (int offTileX, ISet<int> offTileYs) in tilePositionOffets ) {
				foreach( int offTileY in offTileYs ) {
					int distX = offTileX;
					int distY = offTileY;
					double dist = Math.Sqrt( (distX * distX) + (distY * distY) );
					double scale = (double)radius / (((dist - radius) * 0.5d) + radius);//radius / dist
					int offX = (int)((double)distX * scale);
					int offY = (int)((double)distY * scale);

					tilePositions.Set2D( tileX + offX, tileY + offY );
				}
			}

			return tilePositions;
		}
	}
}*/