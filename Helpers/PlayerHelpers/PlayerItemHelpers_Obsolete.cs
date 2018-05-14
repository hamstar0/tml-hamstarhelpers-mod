using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.PlayerHelpers {
	public static partial class PlayerItemHelpers {
		[System.Obsolete( "use PlayerItemFinderHelpers.FindPossiblePurchaseTypes", true )]
		public static ISet<int> FindPossiblePurchaseTypes( Player player, long spent ) {
			return PlayerItemFinderHelpers.FindPossiblePurchaseTypes( player, spent );
		}

		[System.Obsolete( "use PlayerItemFinderHelpers.FindFirstOfItemFor", true )]
		public static Item FindFirstOfItemFor( Player player, ISet<int> item_types ) {
			return PlayerItemFinderHelpers.FindFirstOfItemFor( player, item_types );
		}

		[System.Obsolete( "use PlayerItemFinderHelpers.FindInventoryChanges", true )]
		public static IDictionary<int, KeyValuePair<int, int>> FindInventoryChanges( Player player,
				KeyValuePair<int, int> prev_mouse_info,
				IDictionary<int, KeyValuePair<int, int>> prev_inv ) {
			return PlayerItemFinderHelpers.FindInventoryChanges( player, prev_mouse_info, prev_inv );
		}
	}
}
