using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;


namespace HamstarHelpers.Classes.TileStructure {
	/// <summary>
	/// Represents an arbitrary arrangement of Tile data. No bounding size or contiguity expected.
	/// </summary>
	public partial class TileStructure {
		/// <summary></summary>
		/// <param name="leftTileX"></param>
		/// <param name="topTileY"></param>
		/// <param name="paintAir">Include 'air' tiles when painting to world.</param>
		/// <param name="respectLiquids">Preserves any liquids.</param>
		/// <param name="flipHorizontally"></param>
		/// <param name="flipVertically"></param>
		/// <returns>Count of painted tiles.</returns>
		public int PaintToWorld(
					int leftTileX,
					int topTileY,
					bool paintAir = false,
					bool respectLiquids = false,
					bool flipHorizontally = false,
					bool flipVertically = false ) {
			int width = this.Bounds.Width;
			int height = this.Bounds.Height;
			int i, j, count = 0;

			for( int x = 0; x < width; x++ ) {
				if( flipHorizontally ) {
					i = leftTileX + width - x;
				} else {
					i = x + leftTileX;
				}
				if( i < 0 ) { continue; }
				if( i >= Main.maxTilesX ) { break; }

				for( int y = 0; y < height; y++ ) {
					if( flipVertically ) {
						j = topTileY + height - y;
					} else {
						j = y + topTileY;
					}
					if( j < 0 ) { continue; }
					if( j >= Main.maxTilesY ) { break; }

					int idx = ( x * height ) + y;
					SerializeableTile rawTile = this.Structure[idx];

					if( this.PaintTileToWorld( i, j, rawTile, paintAir, respectLiquids, flipHorizontally, flipVertically ) ) {
						count++;
					}
				}
			}

			for( int x = 0; x < width; x++ ) {
				i = x + leftTileX;
				if( i < 0 ) { continue; }
				if( i >= Main.maxTilesX ) { break; }

				for( int y = height - 1; y >= 0; y++ ) {
					j = y + topTileY;
					if( y < 0 ) { continue; }
					if( y >= Main.maxTilesY ) { break; }

					WorldGen.SquareTileFrame( i, j );
				}
			}

			return count;
		}

		private bool PaintTileToWorld(
					int i,
					int j,
					SerializeableTile rawTile,
					bool paintAir = false,
					bool respectLiquids = false,
					bool flipHorizontally = false,
					bool flipVertically = false ) {
			byte liquid = 0;
			bool isHoney = false, isLava = false;
			if( respectLiquids ) {
				liquid = Main.tile[i, j].liquid;
				isHoney = Main.tile[i, j].honey();
				isLava = Main.tile[i, j].lava();
			}

			if( rawTile != null ) {
				Main.tile[i, j] = rawTile.ToTile();
				Main.tile[i, j].liquid = liquid;
				Main.tile[i, j].honey( isHoney );
				Main.tile[i, j].lava( isLava );

				if( flipHorizontally ) {
					TileStateHelpers.FlipSlopeHorizontally( Main.tile[i, j] );
				}
				if( flipVertically ) {
					TileStateHelpers.FlipSlopeVertically( Main.tile[i, j] );
				}

				return true;
			} else if( paintAir ) {
				Main.tile[i, j].active( false );
				Main.tile[i, j].wall = 0;
				Main.tile[i, j].type = 0;
				Main.tile[i, j].liquid = liquid;
				Main.tile[i, j].honey( isHoney );
				Main.tile[i, j].lava( isLava );
				//Main.tile[i, j] = new Tile();

				return true;
			}

			return false;
		}
	}
}
