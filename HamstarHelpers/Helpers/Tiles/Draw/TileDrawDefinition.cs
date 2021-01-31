using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.Tiles.Draw {
	/// <summary>
	/// Defines a basic tile.
	/// </summary>
	public class TileDrawDefinition {
		/// <summary>TileTyle and TileStyle are ignored</summary>
		public bool NotActive = false;
		/// <summary></summary>
		public bool SkipWall = false;
		/// <summary></summary>
		public ushort TileType = 0;
		/// <summary></summary>
		public ushort WallType = 0;
		/// <summary></summary>
		public int TileStyle = 0;
		/// <summary></summary>
		public sbyte Direction = -1;
		/// <summary></summary>
		public TileSlopeType? Slope = 0;
		/// <summary></summary>
		public bool IsHalfBrick = false;
		/// <summary></summary>
		public byte LiquidVolume = 0;
		/// <summary></summary>
		public bool IsLava = false;
		/// <summary></summary>
		public bool IsHoney = false;



		////////////////

		/// <summary>
		/// Places the current tile. Applies `WorldGen.SquareTileFrame(...)`.
		/// </summary>
		/// <param name="leftTileX"></param>
		/// <param name="bottomTileY"></param>
		/// <returns></returns>
		public bool Place( int leftTileX, int bottomTileY ) {
			if( !this.NotActive ) {
				bool placed = TilePlacementHelpers.PlaceObject(
					leftX: leftTileX,
					bottomY: bottomTileY,
					type: this.TileType,
					style: this.TileStyle,
					direction: this.Direction,
					forced: true
				);

				if( !placed ) {
					placed = WorldGen.PlaceTile(
						i: leftTileX,
						j: bottomTileY,
						type: this.TileType,
						mute: false,
						forced: true,
						plr: -1,
						style: this.TileStyle
					);

					if( !placed ) {
						return false;
					}

					WorldGen.SquareTileFrame( leftTileX, bottomTileY );
				}
			}

			Tile tile = Main.tile[ leftTileX, bottomTileY ];

			if( !this.SkipWall ) {
				tile.wall = this.WallType;
			}

			if( this.Slope.HasValue ) {
				tile.slope( (byte)this.Slope.Value );
			}
			if( this.IsHalfBrick ) {
				tile.halfBrick( true );
			}
			if( this.IsLava ) {
				tile.lava( true );
			} else if( this.IsHoney ) {
				tile.honey( true );
			}

			return true;
		}
	}
}
