using HamstarHelpers.Helpers.DotNetHelpers;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.WorldHelpers {
	public partial class WorldHelpers {
		public static string GetUniqueIdWithSeed() {
			return FileHelpers.SanitizePath( Main.worldName ) + "@" + Main.worldID + "." + Main.ActiveWorldFileData.Seed;
		}


		////////////////

		public static WorldSize GetSize() {
			int size = Main.maxTilesX * Main.maxTilesY;

			if( size <= ( 4200 * 1200 ) / 2 ) {
				return WorldSize.SubSmall;
			} else if( size <= 4200 * 1200 + 1000 ) {
				return WorldSize.Small;
			} else if( size <= 6400 * 1800 + 1000 ) {
				return WorldSize.Medium;
			} else if( size <= 8400 * 2400 + 1000 ) {
				return WorldSize.Large;
			} else {
				return WorldSize.SuperLarge;
			}
		}
		

		////////////////

		public static bool IsAboveWorldSurface( Vector2 world_pos ) {
			return world_pos.Y < (Main.worldSurface * 16);
		}

		public static bool IsWithinUnderworld( Vector2 world_pos ) {
			return world_pos.Y > ((Main.maxTilesY - 200) * 16);
		}
	}
}
