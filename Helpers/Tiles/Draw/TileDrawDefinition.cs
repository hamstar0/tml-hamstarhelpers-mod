using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles.Draw {
	/// <summary>
	/// Defines a basic tile.
	/// </summary>
	public class TileDrawDefinition {
		/// <summary></summary>
		public ushort TileType;
		/// <summary></summary>
		public ushort WallType = 0;
		/// <summary></summary>
		public int TileStyle = 0;
		/// <summary></summary>
		public sbyte Direction = -1;
		/// <summary></summary>
		public TileSlopeType Slope = 0;
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
		/// Places the current tile.
		/// </summary>
		/// <param name="leftTileX"></param>
		/// <param name="bottomTileY"></param>
		/// <returns></returns>
		public bool Place( int leftTileX, int bottomTileY ) {
			bool placed = TilePlacementHelpers.PlaceObject( leftTileX, bottomTileY, this.TileType, this.TileStyle, this.Direction, false );
			if( !placed ) {
				if( !WorldGen.PlaceTile(leftTileX, bottomTileY, this.TileType, false, true, -1, this.TileStyle) ) {
					return false;
				}
				WorldGen.SquareTileFrame( leftTileX, bottomTileY );
			}

			Tile tile = Main.tile[leftTileX, bottomTileY];

			tile.wall = this.WallType;

			if( this.Slope != 0 ) {
				tile.slope( (byte)this.Slope );
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
