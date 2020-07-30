using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.Tiles.TilePattern;


namespace HamstarHelpers.Classes.TileStructure {
	/// <summary>
	/// Represents an arbitrary arrangement of Tile data. No bounding size or contiguity expected.
	/// </summary>
	[Serializable]
	public partial class TileStructure {
		/// <summary></summary>
		public Rectangle Bounds;

		/// <summary>
		/// 2D collection of Tile data.
		/// </summary>
		public SerializeableTile[,] Structure;



		////////////////

		/// <summary></summary>
		public TileStructure() { }

		/// <summary></summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <param name="pattern"></param>
		public TileStructure( int left, int top, int right, int bottom, TilePattern pattern ) {
			if( left < 0 || right >= Main.maxTilesX || top < 0 || bottom >= Main.maxTilesY ) {
				throw new ArgumentException( "Ranges exceed map boundaries" );
			}
			if( left >= right || top >= bottom ) {
				throw new ArgumentException( "Invalid ranges" );
			}

			int width = right - left;
			int height = bottom - top;
			this.Structure = new SerializeableTile[ width, height ];

			for( int x=left; x<right; x++ ) {
				for( int y=top; y<bottom; y++ ) {
					if( pattern.Check(x, y) ) {
						int i = x - left;
						int j = y - top;

						this.Structure[i, j] = new SerializeableTile( Main.tile[i, j] );
					}
				}
			}

			this.Bounds = new Rectangle( left, top, right - left, bottom - top );
		}


		////////////////

		/// <summary>
		/// </summary>
		/// <param name="leftTileX"></param>
		/// <param name="topTileY"></param>
		/// <param name="flipHorizontally"></param>
		/// <param name="flipVertically"></param>
		public void PaintToWorld( int leftTileX, int topTileY, bool flipHorizontally=false, bool flipVertically=false ) {
			int width = this.Bounds.Width;
			int height = this.Bounds.Height;
			int i, j;

			for( int x=0; x<width; x++ ) {
				for( int y=0; y<height; y++ ) {
					if( flipHorizontally ) {
						i = leftTileX + width - x;
					} else {
						i = x + leftTileX;
					}
					if( flipVertically ) {
						j = topTileY + height - y;
					} else {
						j = y + topTileY;
					}

					Main.tile[i, j] = this.Structure[x, y].ToTile();
				}
			}
		}
	}
}
