using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	public partial class TileType {
		public bool? IsWire { get; private set; }

		public bool? IsSolid { get; private set; }
		public bool IsPlatformSolid { get; private set; }
		public bool IsActuatedSolid { get; private set; }
		public bool IsVanillaBombable { get; private set; }

		public bool? HasWall { get; private set; }

		public bool? HasWater { get; private set; }
		public bool? HasHoney { get; private set; }
		public bool? HasLava { get; private set; }



		////////////////

		public bool Check( int tileX, int tileY ) {
			Tile tile = Main.tile[ tileX, tileY ];

			if( this.IsWire != null ) {
				if( TileHelpers.IsWire( tile ) != this.IsWire ) {
					return false;
				}
			}

			if( this.IsSolid != null ) {
				if( TileHelpers.IsSolid(tile, this.IsPlatformSolid, this.IsActuatedSolid) != this.IsSolid ) {
					return false;
				}
			}

			if( this.HasWall != null ) {
				if( (tile.wall > 0) != this.HasWall ) {
					return false;
				}
			}

			if( this.HasLava != null ) {
				if( tile.lava() != this.HasLava ) {
					return false;
				}
			}

			if( this.HasHoney != null ) {
				if( tile.honey() != this.HasHoney ) {
					return false;
				}
			}

			if( this.HasWater != null ) {
				if( tile.liquid > 0 != this.HasWater ) {
					return false;
				}
			}

			return true;
		}


		public bool CheckArea( Rectangle tileArea ) {
			return this.CheckArea( tileArea.X, tileArea.Y, tileArea.Width, tileArea.Height );
		}
		
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
