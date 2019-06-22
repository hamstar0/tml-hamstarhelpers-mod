using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/** <summary>Supplies identifying information usable by any given tile (WIP).</summary> */
	public partial class TileType {
		public readonly static TileType OpenWall = new TileType {
			IsSolid = false,
			HasWall = true
		};
		public readonly static TileType CommonSolid = new TileType {
			IsSolid = true,
			IsActuatedSolid = true,
			IsPlatformSolid = false
		};
	}
}
