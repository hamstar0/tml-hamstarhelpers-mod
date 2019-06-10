using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
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
