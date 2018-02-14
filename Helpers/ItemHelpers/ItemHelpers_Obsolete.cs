using HamstarHelpers.PlayerHelpers;
using Terraria;


namespace HamstarHelpers.ItemHelpers {
	public static partial class ItemHelpers {
		[System.Obsolete( "use ItemIdentityHelpers.GetUniqueId", true )]
		public static string GetUniqueId( Item item ) {
			return ItemIdentityHelpers.GetUniqueId( item );
		}

		[System.Obsolete( "use PlayerItemHelpers.GetGrappleItem", true )]
		public static Item GetGrappleItem( Player player ) {
			return PlayerItemHelpers.GetGrappleItem( player );
		}
	}
}
