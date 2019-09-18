using HamstarHelpers.Helpers.Tiles;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// <summary></summary>
	public enum TileCollideType {
		None = 0,
		Solid,
		Wall,
		Platform,
		Wire1,
		Wire2,
		Wire3,
		Wire4,
		Actuated,
		Water,
		Honey,
		Lava,
		SlopeAny,
		SlopeHalfBrick,
		SlopeTopRight,
		SlopeTopLeft,
		SlopeBottomRight,
		SlopeBottomLeft,
		SlopeTop,
		SlopeBottom,
		SlopeLeft,
		SlopeRight,
		BrightnessLow,
		BrightnessHigh,
	}




	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary>
		/// Tests a given tile against the current settings.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool Check( int tileX, int tileY ) {
			TileCollideType _;
			Point __;
			return this.Check( tileX, tileY, out _, out __ );
		}

		/// <summary>
		/// Tests a given tile against the current settings.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="collideType"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool Check( int tileX, int tileY, out TileCollideType collideType ) {
			Point __;
			return this.Check( tileX, tileY, out collideType, out __ );
		}

		/// <summary>
		/// Tests a given tile against the current settings.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="collideType"></param>
		/// <param name="collisionAt"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool Check( int tileX, int tileY, out TileCollideType collideType, out Point collisionAt ) {
			if( !this.AreaFromCenter.HasValue || (this.AreaFromCenter.Value.X == 1 && this.AreaFromCenter.Value.Y == 1) ) {
				collisionAt = new Point( tileX, tileY );
				return this.CheckPoint( tileX, tileY, out collideType );
			}

			int leftTileX = tileX - this.AreaFromCenter.Value.X;
			int topTileY = tileY - this.AreaFromCenter.Value.Y;

			return this.CheckArea(
				leftTileX,
				topTileY,
				this.AreaFromCenter.Value.X * 2,
				this.AreaFromCenter.Value.Y * 2,
				out collideType,
				out collisionAt
			);
		}


		////////////////

		/// <summary>
		/// Tests a given tile against the current settings. Also indicates what type of collision occurs.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="collideType"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool CheckPoint( int tileX, int tileY, out TileCollideType collideType ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );

			if( TileHelpers.IsAir(tile) ) {
				if( !this.CheckBrightness(tileX, tileY, out collideType) ) {
					return false;
				}
				collideType = TileCollideType.None;
				return true;
			}

			if( this.HasWire1.HasValue ) {
				if( this.HasWire1.Value != tile.wire() ) {
					collideType = TileCollideType.Wire1;
					return false;
				}
			}
			if( this.HasWire2.HasValue ) {
				if( this.HasWire2.Value != tile.wire2() ) {
					collideType = TileCollideType.Wire2;
					return false;
				}
			}
			if( this.HasWire3.HasValue ) {
				if( this.HasWire3.Value != tile.wire3() ) {
					collideType = TileCollideType.Wire3;
					return false;
				}
			}
			if( this.HasWire4.HasValue ) {
				if( this.HasWire4.Value != tile.wire4() ) {
					collideType = TileCollideType.Wire4;
					return false;
				}
			}

			if( this.IsPlatform.HasValue ) {
				if( Main.tileSolidTop[tile.type] != this.IsPlatform ) {
					collideType = TileCollideType.Platform;
					return false;
				}
			}

			if( this.IsActuated.HasValue ) {
				if( tile.inActive() != this.IsActuated ) {
					collideType = TileCollideType.Actuated;
					return false;
				}
			}

			if( this.IsSolid.HasValue ) {
				bool isPlatform = this.IsPlatform.GetValueOrDefault();
				bool isActuated = this.IsActuated.GetValueOrDefault();
				if( TileHelpers.IsSolid(tile, isPlatform, isActuated) != this.IsSolid.Value ) {
					collideType = TileCollideType.Solid;
					return false;
				}
			}

			if( this.HasWall.HasValue ) {
				if( (tile.wall > 0) != this.HasWall.Value ) {
					collideType = TileCollideType.Wall;
					return false;
				}
			}

			if( this.HasLava.HasValue ) {
				if( tile.lava() != this.HasLava.Value ) {
					collideType = TileCollideType.Lava;
					return false;
				}
			}
			if( this.HasHoney.HasValue ) {
				if( tile.honey() != this.HasHoney.Value ) {
					collideType = TileCollideType.Honey;
					return false;
				}
			}
			if( this.HasWater.HasValue ) {
				if( tile.liquid > 0 != this.HasWater.Value ) {
					collideType = TileCollideType.Water;
					return false;
				}
			}
			
			if( this.Slope.HasValue ) {
				switch( this.Slope.Value ) {
				case TileSlopeType.None:
					if( tile.slope() != 0 ) {
						collideType = TileCollideType.None;
						return false;
					}
					break;
				case TileSlopeType.Any:
					if( tile.slope() == 0 ) {
						collideType = TileCollideType.SlopeAny;
						return false;
					}
					break;
				case TileSlopeType.HalfBrick:
					if( !tile.halfBrick() ) {
						collideType = TileCollideType.SlopeHalfBrick;
						return false;
					}
					break;
				case TileSlopeType.TopRightSlope:
					if( tile.slope() == 1 ) {
						collideType = TileCollideType.SlopeTopRight;
						return false;
					}
					break;
				case TileSlopeType.TopLeftSlope:
					if( tile.slope() == 2 ) {
						collideType = TileCollideType.SlopeTopLeft;
						return false;
					}
					break;
				case TileSlopeType.BottomRightSlope:
					if( tile.slope() == 3 ) {
						collideType = TileCollideType.SlopeBottomRight;
						return false;
					}
					break;
				case TileSlopeType.BottomLeftSlope:
					if( tile.slope() == 4 ) {
						collideType = TileCollideType.SlopeBottomLeft;
						return false;
					}
					break;
				case TileSlopeType.Top:
					if( !tile.topSlope() ) {
						collideType = TileCollideType.SlopeTop;
						return false;
					}
					break;
				case TileSlopeType.Bottom:
					if( !tile.bottomSlope() ) {
						collideType = TileCollideType.SlopeBottom;
						return false;
					}
					break;
				case TileSlopeType.Left:
					if( !tile.leftSlope() ) {
						collideType = TileCollideType.SlopeLeft;
						return false;
					}
					break;
				case TileSlopeType.Right:
					if( !tile.rightSlope() ) {
						collideType = TileCollideType.SlopeRight;
						return false;
					}
					break;
				}
			}

			if( !this.CheckBrightness(tileX, tileY, out collideType) ) {
				return false;
			}

			collideType = TileCollideType.None;
			return true;
		}


		////////////////

		/// <summary>
		/// Checks all tiles in an area.
		/// </summary>
		/// <param name="tileArea"></param>
		/// <returns></returns>
		public bool CheckArea( Rectangle tileArea ) {
			return this.CheckArea( tileArea.X, tileArea.Y, tileArea.Width, tileArea.Height );
		}

		/// <summary>
		/// Checks all tiles in an area.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public bool CheckArea( int tileX, int tileY, int width, int height ) {
			TileCollideType _;
			Point __;
			return this.CheckArea( tileX, tileY, width, height, out _, out __ );
		}

		/// <summary>
		/// Checks all tiles in an area.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="collideType"></param>
		/// <param name="collision"></param>
		/// <returns></returns>
		public bool CheckArea( int tileX, int tileY, int width, int height,
				out TileCollideType collideType,
				out Point collision ) {
			int maxX = tileX + width;
			int maxY = tileY + height;

			for( int i = tileX; i < maxX; i++ ) {
				for( int j = tileY; j < maxY; j++ ) {
					if( !this.Check( i, j, out collideType ) ) {
						collision = new Point( i, j );
						return false;
					}
				}
			}

			collision = new Point(-1, -1);
			collideType = TileCollideType.None;
			return true;
		}


		////

		public bool CheckBrightness( int tileX, int tileY, out TileCollideType collideType ) {
			if( this.MinimumBrightness.HasValue || this.MaximumBrightness.HasValue ) {
				float brightness = Lighting.Brightness( tileX, tileY );

				if( this.MinimumBrightness.HasValue ) {
					if( this.MinimumBrightness > brightness ) {
						collideType = TileCollideType.BrightnessLow;
						return false;
					}
				}
				if( this.MaximumBrightness.HasValue ) {
					if( this.MaximumBrightness < brightness ) {
						collideType = TileCollideType.BrightnessHigh;
						return false;
					}
				}
			}

			collideType = TileCollideType.None;
			return true;
		}
	}
}
