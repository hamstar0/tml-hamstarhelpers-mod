using System.Linq;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		private bool CheckInverted( bool value ) {
			if( value ) {
				if( !this.Invert ) {
					return true;
				}
			} else {
				if( this.Invert ) {
					return true;
				}
			}
			return false;
		}


		////

		/// <summary>
		/// Tests a given tile against the current settings. Also indicates what type of collision occurs.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="collideType"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool CheckPoint( int tileX, int tileY, out TileCollideType collideType ) {
			if( tileX < 0 || tileX >= Main.maxTilesX ) {
				LogHelpers.AlertOnce( "Tile out of X range." );
				collideType = TileCollideType.WorldEdge;
				return false;
			}
			if( tileY < 0 || tileY >= Main.maxTilesY ) {
				LogHelpers.AlertOnce( "Tile out of Y range." );
				collideType = TileCollideType.WorldEdge;
				return false;
			}

			Tile tile = Framing.GetTileSafely( tileX, tileY );

			bool isActive = tile.active();
			if( this.IsActive.HasValue ) {
				if( this.CheckInverted( this.IsActive.Value != isActive ) ) {
					collideType = TileCollideType.Active;
					return false;
				}
			}

			/*if( TileHelpers.IsAir(tile, false, false) ) {
				if( !this.CheckBrightness(tileX, tileY, out collideType) ) {
					return false;
				}
				collideType = TileCollideType.None;
				return true;
			}*/

			if( this.CustomCheck != null ) {
				if( this.CheckInverted( !this.CustomCheck.Invoke(tileX, tileY) ) ) {
					collideType = TileCollideType.Custom;
					return false;
				}
			}

			if( this.AnyPattern != null ) {
				bool subPatternFound = false;
				TileCollideType subCollideType = TileCollideType.None;

				foreach( TilePattern subPattern in this.AnyPattern ) {
					if( subPattern.CheckPoint(tileX, tileY, out subCollideType) ) {
						subPatternFound = true;
						break;
					}
				}

				if( this.CheckInverted( !subPatternFound ) ) {
					collideType = subCollideType;
					return false;
				}
			}

			if( isActive ) {
				if( !this.CheckActivePoint(tileX, tileY, out collideType) ) {
					return false;
				}
			} else {
				if( !this.CheckNonActivePoint(tileX, tileY, out collideType) ) {
					return false;
				}
			}

			return this.CheckGeneralPoint( tileX, tileY, out collideType );
		}


		////

		private bool CheckActivePoint( int tileX, int tileY, out TileCollideType collideType ) {
			Tile tile = Main.tile[ tileX, tileY ];

			if( this.IsAnyOfType != null && this.IsAnyOfType.Count > 0 ) {
				if( this.CheckInverted( !this.IsAnyOfType.Any(t => t == tile.type) ) ) {
					collideType = TileCollideType.TileType;
					return false;
				}
			}

			if( this.IsNotAnyOfType != null && this.IsNotAnyOfType.Count > 0 ) {
				if( this.CheckInverted( this.IsNotAnyOfType.Any(t => t == tile.type) ) ) {
					collideType = TileCollideType.TileType;
					return false;
				}
			}

			if( this.IsActuated.HasValue ) {
				if( this.CheckInverted( tile.inActive() != this.IsActuated.Value ) ) {
					collideType = TileCollideType.Actuated;
					return false;
				}
			}

			if( this.HasSolidProperties.HasValue ) {
				if( this.CheckInverted( Main.tileSolid[tile.type] != this.HasSolidProperties.Value ) ) {
					collideType = TileCollideType.Solid;
					return false;
				}
			}

			if( this.IsPlatform.HasValue ) {
				if( this.CheckInverted( Main.tileSolidTop[tile.type] != this.IsPlatform.Value ) ) {
					collideType = TileCollideType.Platform;
					return false;
				}
			}

			/*if( this.IsSolid.HasValue ) {
				if( !Main.tileSolid[tile.type] ) {
					if( this.CheckInverted( this.IsSolid.Value ) ) {
						collideType = TileCollideType.Solid;
						return false;
					}
				} else {//Main.tileSolid[tile.type] == true
					if( Main.tileSolidTop[tile.type] ) {
						if( this.CheckInverted( !this.IsPlatform.HasValue || !this.IsPlatform.Value ) ) {
							collideType = TileCollideType.Platform;
							return false;
						}
					} else {//Main.tileSolidTop[tile.type] == false
						if( this.CheckInverted( !this.IsSolid.Value ) ) {
							collideType = TileCollideType.Solid;
							return false;
						}
					}
				}
			}*/

			if( this.Shape.HasValue ) {
				switch( this.Shape.Value ) {
				case TileShapeType.None:
					if( this.CheckInverted( tile.slope() != 0 ) ) {
						collideType = TileCollideType.None;
						return false;
					}
					break;
				case TileShapeType.Any:
					if( this.CheckInverted( tile.slope() == 0 ) ) {
						collideType = TileCollideType.SlopeAny;
						return false;
					}
					break;
				case TileShapeType.HalfBrick:
					if( this.CheckInverted( !tile.halfBrick() ) ) {
						collideType = TileCollideType.SlopeHalfBrick;
						return false;
					}
					break;
				case TileShapeType.TopRightSlope:
					if( this.CheckInverted( tile.slope() == 1 ) ) {
						collideType = TileCollideType.SlopeTopRight;
						return false;
					}
					break;
				case TileShapeType.TopLeftSlope:
					if( this.CheckInverted( tile.slope() == 2 ) ) {
						collideType = TileCollideType.SlopeTopLeft;
						return false;
					}
					break;
				case TileShapeType.BottomRightSlope:
					if( this.CheckInverted( tile.slope() == 3 ) ) {
						collideType = TileCollideType.SlopeBottomRight;
						return false;
					}
					break;
				case TileShapeType.BottomLeftSlope:
					if( this.CheckInverted( tile.slope() == 4 ) ) {
						collideType = TileCollideType.SlopeBottomLeft;
						return false;
					}
					break;
				case TileShapeType.TopSlope:
					if( this.CheckInverted( !tile.topSlope() ) ) {
						collideType = TileCollideType.SlopeTop;
						return false;
					}
					break;
				case TileShapeType.BottomSlope:
					if( this.CheckInverted( !tile.bottomSlope() ) ) {
						collideType = TileCollideType.SlopeBottom;
						return false;
					}
					break;
				case TileShapeType.LeftSlope:
					if( this.CheckInverted( !tile.leftSlope() ) ) {
						collideType = TileCollideType.SlopeLeft;
						return false;
					}
					break;
				case TileShapeType.RightSlope:
					if( this.CheckInverted( !tile.rightSlope() ) ) {
						collideType = TileCollideType.SlopeRight;
						return false;
					}
					break;
				}
			}

			collideType = TileCollideType.None;
			return true;
		}

		private bool CheckNonActivePoint( int tileX, int tileY, out TileCollideType collideType ) {
			if( this.CheckInverted( this.HasSolidProperties.HasValue && this.HasSolidProperties.Value ) ) {
				collideType = TileCollideType.Solid;
				return false;
			}
			if( this.CheckInverted( this.IsPlatform.HasValue && this.IsPlatform.Value ) ) {
				collideType = TileCollideType.Platform;
				return false;
			}
			if( this.CheckInverted( this.IsAnyOfType != null && this.IsAnyOfType.Count > 0 ) ) {
				collideType = TileCollideType.TileType;
				return false;
			}

			collideType = TileCollideType.None;
			return true;
		}


		private bool CheckGeneralPoint( int tileX, int tileY, out TileCollideType collideType ) {
			Tile tile = Main.tile[tileX, tileY];

			if( this.IsAnyOfWallType != null && this.IsAnyOfWallType.Count > 0 ) {
				if( this.CheckInverted( !this.IsAnyOfWallType.Any( w => tile.wall == w ) ) ) {
					collideType = TileCollideType.WallType;
					return false;
				}
			}

			if( this.IsNotAnyOfWallType != null && this.IsNotAnyOfWallType.Count > 0 ) {
				if( this.CheckInverted( this.IsNotAnyOfWallType.Any( w => tile.wall == w ) ) ) {
					collideType = TileCollideType.WallTypeNot;
					return false;
				}
			}

			if( this.HasWire1.HasValue ) {
				if( this.CheckInverted( this.HasWire1.Value != tile.wire() ) ) {
					collideType = TileCollideType.Wire1;
					return false;
				}
			}
			if( this.HasWire2.HasValue ) {
				if( this.CheckInverted( this.HasWire2.Value != tile.wire2() ) ) {
					collideType = TileCollideType.Wire2;
					return false;
				}
			}
			if( this.HasWire3.HasValue ) {
				if( this.CheckInverted( this.HasWire3.Value != tile.wire3() ) ) {
					collideType = TileCollideType.Wire3;
					return false;
				}
			}
			if( this.HasWire4.HasValue ) {
				if( this.CheckInverted( this.HasWire4.Value != tile.wire4() ) ) {
					collideType = TileCollideType.Wire4;
					return false;
				}
			}

			/*if( this.HasSolidProperties.HasValue ) {
				if( !tile.active() ) {
					if( this.CheckInverted( this.HasSolidProperties.Value ) ) {
						collideType = TileCollideType.Solid;
						return false;
					}
				}
			}*/

			if( this.HasWall.HasValue ) {
				if( this.CheckInverted( ( tile.wall > 0 ) != this.HasWall.Value ) ) {
					collideType = TileCollideType.Wall;
					return false;
				}
			}

			if( this.HasLava.HasValue ) {
				if( this.CheckInverted( tile.lava() != this.HasLava.Value ) ) {
					collideType = TileCollideType.Lava;
					return false;
				}
			}
			if( this.HasHoney.HasValue ) {
				if( this.CheckInverted( tile.honey() != this.HasHoney.Value ) ) {
					collideType = TileCollideType.Honey;
					return false;
				}
			}
			if( this.HasWater.HasValue ) {
				if( this.CheckInverted( tile.liquid > 0 != this.HasWater.Value ) ) {
					collideType = TileCollideType.Water;
					return false;
				}
			}

			if( this.MinimumBrightness.HasValue || this.MaximumBrightness.HasValue ) {
				if( !this.CheckBrightness( tileX, tileY, out collideType ) ) {
					return false;
				}
			}

			collideType = TileCollideType.None;
			return true;
		}


		////////////////

		/// <summary>
		/// Checks for valid brightness of the given tile.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="collideType"></param>
		/// <returns></returns>
		public bool CheckBrightness( int tileX, int tileY, out TileCollideType collideType ) {
			float brightness = Lighting.Brightness( tileX, tileY );

			if( this.MinimumBrightness.HasValue ) {
				if( this.CheckInverted( this.MinimumBrightness > brightness ) ) {
					collideType = TileCollideType.BrightnessLow;
					return false;
				}
			}
			if( this.MaximumBrightness.HasValue ) {
				if( this.CheckInverted( this.MaximumBrightness < brightness ) ) {
					collideType = TileCollideType.BrightnessHigh;
					return false;
				}
			}

			collideType = TileCollideType.None;
			return true;
		}
	}
}
