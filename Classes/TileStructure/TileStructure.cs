using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;


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
		public SerializeableTile[] Structure;

		/// <summary></summary>
		[NonSerialized]
		public int TileCount;



		////////////////

		/// <summary></summary>
		public TileStructure() { }

		private void RecountTiles() {
			this.TileCount = 0;

			for( int i=0; i<this.Bounds.Width; i++ ) {
				for( int j=0; j<this.Bounds.Height; j++ ) {
					if( this.Structure[i*j] != null ) {
						this.TileCount++;
					}
				}
			}
		}


		////////////////

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
				throw new ArgumentException( "Invalid ranges. left:"+left+", right:"+right+", top:"+top+", bottom:"+bottom );
			}

			int width = right - left;
			int height = bottom - top;
			this.Structure = new SerializeableTile[ width * height ];

			for( int x=left; x<right; x++ ) {
				int i = x - left;

				for( int y=top; y<bottom; y++ ) {
					if( pattern.CheckPoint(x, y, out TileCollideType collideType) ) {
						int j = y - top;
						int idx = (i * height) + j;

						this.Structure[idx] = new SerializeableTile( Framing.GetTileSafely(x, y) );
						this.TileCount++;
					} else {
						LogHelpers.LogOnce( "Pattern match failed: "+collideType );
					}
				}
			}

			this.Bounds = new Rectangle( left, top, right - left, bottom - top );
		}


		////////////////

		/// <summary></summary>
		/// <param name="leftTileX"></param>
		/// <param name="topTileY"></param>
		/// <param name="paintAir">Include 'air' tiles when painting to world.</param>
		/// <param name="flipHorizontally"></param>
		/// <param name="flipVertically"></param>
		/// <returns>Count of painted tiles.</returns>
		public int PaintToWorld(
					int leftTileX,
					int topTileY,
					bool paintAir=false,
					bool flipHorizontally=false,
					bool flipVertically=false ) {
			int width = this.Bounds.Width;
			int height = this.Bounds.Height;
			int i, j, count = 0;

			for( int x=0; x<width; x++ ) {
				if( flipHorizontally ) {
					i = leftTileX + width - x;
				} else {
					i = x + leftTileX;
				}
				if( i < 0 ) {
					continue;
				}
				if( i >= Main.maxTilesX ) {
					break;
				}

				for( int y=0; y<height; y++ ) {
					if( flipVertically ) {
						j = topTileY + height - y;
					} else {
						j = y + topTileY;
					}
					if( j<0 || j>=Main.maxTilesY ) {
						continue;
					}

					int idx = ( x * height ) + y;
					SerializeableTile rawTile = this.Structure[ idx ];

					if( rawTile != null ) {
						Main.tile[i, j] = rawTile.ToTile();
						count++;
					} else if( paintAir ) {
						Main.tile[i, j].active( false );
						Main.tile[i, j].wall = 0;
						Main.tile[i, j].type = 0;
						Main.tile[i, j].liquid = 0;
						Main.tile[i, j] = new Tile();
						count++;
					}
				}
			}

			return count;
		}
	}
}
