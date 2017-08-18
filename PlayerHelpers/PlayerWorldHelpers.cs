using Terraria;


namespace HamstarHelpers.PlayerHelpers {
	public static class PlayerWorldHelpers {
		[System.Obsolete( "use WorldHelpers.IsAboveWorldSurface(player.position)", true )]
		public static bool IsAboveWorldSurface( Player player ) {
			return player.position.Y < (Main.worldSurface * 16);
		}

		[System.Obsolete( "use WorldHelpers.IsWithinUnderworld(player.position)", true )]
		public static bool IsWithinUnderworld( Player player ) {
			return player.position.Y > ((Main.maxTilesY - 200) * 16);
		}
	}
}
