using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.TileHelpers {
	public class TileType {
		public bool? IsWire;
		public bool? IsSolid;
		public bool IsPlatformSolid;
		public bool IsActuatedSolid;
		public bool IsVanillaBombable;

		public bool? HasWall;

		public bool? HasWater;
		public bool? HasHoney;
		public bool? HasLava;



		public bool Check( int tile_x, int tile_y ) {
			Tile tile = Main.tile[ tile_x, tile_y ];

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


		public bool CheckArea( Rectangle tile_area ) {
			return this.CheckArea( tile_area.X, tile_area.Y, tile_area.Width, tile_area.Height );
		}
		
		public bool CheckArea( int tile_x, int tile_y, int width, int height ) {    //, out int fail_at_x, out int fail_at_y
			int max_x = tile_x + width;
			int max_y = tile_y + height;

			for( int i = tile_x; i < max_x; i++ ) {
				for( int j = tile_y; j < max_y; j++ ) {
					if( !this.Check( i, j ) ) {
						return false;
					}
				}
			}
			return true;
		}
	}
}
