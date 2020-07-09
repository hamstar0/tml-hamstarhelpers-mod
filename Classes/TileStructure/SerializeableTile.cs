using System;
using Terraria;


namespace HamstarHelpers.Classes.TileStructure {
	/// <summary></summary>
	[Serializable]
	public class SerializeableTile {
		private ushort type;
		private short frameY;
		private short frameX;
		private byte bTileHeader3;
		private byte bTileHeader2;
		private byte bTileHeader;
		private ushort sTileHeader;
		private byte liquid;
		private ushort wall;



		////////////////

		/// <summary></summary>
		public SerializeableTile( Tile tile ) {
			this.type = tile.type;
			this.frameY = tile.frameY;
			this.frameX = tile.frameX;
			this.bTileHeader3 = tile.bTileHeader3;
			this.bTileHeader2 = tile.bTileHeader2;
			this.bTileHeader = tile.bTileHeader;
			this.sTileHeader = tile.sTileHeader;
			this.liquid = tile.liquid;
			this.wall = tile.wall;
		}

		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public Tile GetTile() {
			return new Tile {
				frameY = this.frameY,
				frameX = this.frameX,
				bTileHeader3 = this.bTileHeader3,
				bTileHeader2 = this.bTileHeader2,
				bTileHeader = this.bTileHeader,
				sTileHeader = this.sTileHeader,
				liquid = this.liquid,
				wall = this.wall,
			};
		}
	}
}
