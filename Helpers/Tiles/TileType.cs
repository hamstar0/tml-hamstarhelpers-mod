using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Supplies identifying information usable by any given tile (WIP).
	/// </summary>
	public partial class TileType {
		/// <summary>
		/// Preset for walls, no tiles.
		/// </summary>
		public readonly static TileType OpenWall = new TileType {
			IsSolid = false,
			HasWall = true
		};

		/// <summary>
		/// Preset for common solid tiles.
		/// </summary>
		public readonly static TileType CommonSolid = new TileType {
			IsSolid = true,
			IsActuatedSolid = false,
			IsPlatformSolid = false
		};
	}
}
