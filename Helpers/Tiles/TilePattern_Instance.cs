using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary></summary>
		public bool? HasWire1 { get; private set; }
		/// <summary></summary>
		public bool? HasWire2 { get; private set; }
		/// <summary></summary>
		public bool? HasWire3 { get; private set; }
		/// <summary></summary>
		public bool? HasWire4 { get; private set; }

		/// <summary></summary>
		public bool? IsSolid { get; private set; }
		/// <summary></summary>
		public bool IsPlatformSolid { get; private set; }
		/// <summary></summary>
		public bool IsActuatedSolid { get; private set; }
		/// <summary></summary>
		public bool IsVanillaBombable { get; private set; }

		/// <summary></summary>
		public bool? HasWall { get; private set; }

		/// <summary></summary>
		public bool? HasWater { get; private set; }
		/// <summary></summary>
		public bool? HasHoney { get; private set; }
		/// <summary></summary>
		public bool? HasLava { get; private set; }



		////////////////

		/// <summary>
		/// Tests a given tile against the current settings.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool Check( int tileX, int tileY ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );

			if( TileHelpers.IsAir(tile) ) {
				if( this.HasHoney.HasValue && this.HasHoney.Value ) {
					return false;
				}
				if( this.HasLava.HasValue && this.HasLava.Value ) {
					return false;
				}
				if( this.HasWater.HasValue && this.HasWater.Value ) {
					return false;
				}
				if( this.HasWall.HasValue && this.HasWall.Value ) {
					return false;
				}
				if( this.HasWire1.HasValue && this.HasWire1.Value ) {
					return false;
				}
				if( this.HasWire2.HasValue && this.HasWire2.Value ) {
					return false;
				}
				if( this.HasWire3.HasValue && this.HasWire3.Value ) {
					return false;
				}
				if( this.HasWire4.HasValue && this.HasWire4.Value ) {
					return false;
				}
				if( this.IsSolid.HasValue && this.IsSolid.Value ) {
					return false;
				}
				return true;
			}

			if( this.HasWire1.HasValue ) {
				if( this.HasWire1.Value != tile.wire() ) {
					return false;
				}
			}
			if( this.HasWire2.HasValue ) {
				if( this.HasWire2.Value != tile.wire2() ) {
					return false;
				}
			}
			if( this.HasWire3.HasValue ) {
				if( this.HasWire3.Value != tile.wire3() ) {
					return false;
				}
			}
			if( this.HasWire4.HasValue ) {
				if( this.HasWire4.Value != tile.wire4() ) {
					return false;
				}
			}

			if( this.IsSolid.HasValue ) {
				if( TileHelpers.IsSolid(tile, this.IsPlatformSolid, this.IsActuatedSolid) != this.IsSolid.Value ) {
					return false;
				}
			}

			if( this.HasWall.HasValue ) {
				if( (tile.wall > 0) != this.HasWall.Value ) {
					return false;
				}
			}

			if( this.HasLava.HasValue ) {
				if( tile.lava() != this.HasLava.Value ) {
					return false;
				}
			}
			if( this.HasHoney.HasValue ) {
				if( tile.honey() != this.HasHoney.Value ) {
					return false;
				}
			}
			if( this.HasWater.HasValue ) {
				if( tile.liquid > 0 != this.HasWater.Value ) {
					return false;
				}
			}

			return true;
		}


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
		public bool CheckArea( int tileX, int tileY, int width, int height ) {    //, out int failAtX, out int failAtY
			int maxX = tileX + width;
			int maxY = tileY + height;

			for( int i = tileX; i < maxX; i++ ) {
				for( int j = tileY; j < maxY; j++ ) {
					if( !this.Check( i, j ) ) {
						return false;
					}
				}
			}
			return true;
		}
	}
}
