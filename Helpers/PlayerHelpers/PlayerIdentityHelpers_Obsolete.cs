using System;
using Terraria;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	[Obsolete("use HamstarHelpers.PlayerHelpers", true)]
	public static class PlayerIdentityHelpers {
		[Obsolete( "use HamstarHelpers.PlayerHelpers", true )]
		public const int InventorySize = 58;
		[Obsolete( "use HamstarHelpers.PlayerHelpers", true )]
		public const int InventoryHotbarSize = 10;
		[Obsolete( "use HamstarHelpers.PlayerHelpers", true )]
		public const int InventoryMainSize = 40;


		////////////////

		[Obsolete( "use HamstarHelpers.PlayerHelpers", true )]
		public static string GetUniqueId( Player player, out bool success ) {
			return HamstarHelpers.PlayerHelpers.PlayerIdentityHelpers.GetUniqueId( player, out success );
		}
		[Obsolete( "use HamstarHelpers.PlayerHelpers", true )]
		public static int GetVanillaSnapshotHash( Player player, bool no_context, bool looks_matter ) {
			return HamstarHelpers.PlayerHelpers.PlayerIdentityHelpers.GetVanillaSnapshotHash( player, no_context, looks_matter );
		}
	}
}
