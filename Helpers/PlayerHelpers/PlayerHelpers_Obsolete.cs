using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace HamstarHelpers.PlayerHelpers {
	public static partial class PlayerHelpers {
		[System.Obsolete( "use PlayerWorldHelpers.IsAboveWorldSurface", true )]
		public static bool IsAboveWorldSurface( Player player ) {
			return PlayerWorldHelpers.IsAboveWorldSurface( player );
		}

		[System.Obsolete( "use PlayerNPCHelpers.HasUsedNurse", true )]
		public static bool HasUsedNurse( Player player ) {
			return PlayerNPCHelpers.HasUsedNurse( player );
		}
		
		[System.Obsolete( "use PlayerMovementHelpers.IsRelaxed", true )]
		public static bool IsRelaxed( Player player, bool not_mounted = true, bool not_grappled = true,
				bool not_pulleyed = true, bool not_frozen = true, bool not_inverted = true ) {
			return PlayerMovementHelpers.IsRelaxed( player, not_mounted, not_grappled, not_pulleyed, not_frozen, not_inverted );
		}
		
		[System.Obsolete( "use PlayerMovementHelpers.IsFlying", true )]
		public static bool IsFlying( Player player ) {
			return PlayerMovementHelpers.IsFlying( player );
		}
		
		[System.Obsolete( "use PlayerMovementHelpers.MinimumRunSpeed", true )]
		public static float MinimumRunSpeed( Player player ) {
			return PlayerMovementHelpers.MinimumRunSpeed( player );
		}

		[System.Obsolete( "use PlayerMovementHelpers.CanPlayerJump", true )]
		public static bool CanPlayerJump( Player player ) {
			return PlayerMovementHelpers.CanPlayerJump( player );
		}

		[System.Obsolete( "use PlayerIdentityHelpers.GetUniqueId", true )]
		public static string GetUniqueId( Player player, out bool has_loaded ) {
			return PlayerIdentityHelpers.GetUniqueId( player, out has_loaded );
		}
	}
}
