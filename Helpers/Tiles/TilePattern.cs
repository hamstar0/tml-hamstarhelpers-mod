using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary>
		/// Preset for walls, no tiles.
		/// </summary>
		public readonly static TilePattern OpenWall = new TilePattern {
			IsSolid = false,
			HasWall = true
		};

		/// <summary>
		/// Preset for common solid tiles.
		/// </summary>
		public readonly static TilePattern CommonSolid = new TilePattern {
			IsSolid = true,
			IsActuatedSolid = false,
			IsPlatformSolid = false
		};
	}
}
