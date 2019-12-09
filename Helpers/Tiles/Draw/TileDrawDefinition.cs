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
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public bool Place( int tileX, int tileY ) {
			bool placed = TilePlacementHelpers.Place( tileX, tileY, this.TileType, this.TileStyle, this.Direction );
			Tile tile = Main.tile[tileX, tileY];

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

			return placed;
		}
	}
}
