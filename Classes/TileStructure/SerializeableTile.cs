using System;
using Terraria;


namespace HamstarHelpers.Classes.TileStructure {
	/// <summary></summary>
	[Serializable]
	public class SerializeableTile {
		/// <summary></summary>
		public ushort type;
		/// <summary></summary>
		public short frameY;
		/// <summary></summary>
		public short frameX;
		/// <summary></summary>
		public byte bTileHeader3;
		/// <summary></summary>
		public byte bTileHeader2;
		/// <summary></summary>
		public byte bTileHeader;
		/// <summary></summary>
		public ushort sTileHeader;
		/// <summary></summary>
		public byte liquid;
		/// <summary></summary>
		public ushort wall;



		////////////////

		/// <summary></summary>
		/// <param name="tile"></param>
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
		public bool active() {
			return (this.sTileHeader & 32) == 32;
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public Tile ToTile() {
			return new Tile {
				type = this.type,
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
