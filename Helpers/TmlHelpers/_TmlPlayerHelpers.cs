using HamstarHelpers.Services.Players;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.TmlHelpers {
	[Obsolete("use Services.Players.PlayerHooks", true)]
	public class TmlPlayerHelpers {
		public static bool AddBuffExpireAction( string which, Action<Player, int> action ) {
			return ExtendedPlayerHooks.AddBuffExpireAction( which, action );
		}

		public static bool AddArmorEquipAction( string which, Action<Player, int, Item> action ) {
			return ExtendedPlayerHooks.AddArmorEquipAction( which, action );
		}

		public static bool AddArmorUnequipAction( string which, Action<Player, int, int> action ) {
			return ExtendedPlayerHooks.AddArmorUnequipAction( which, action );
		}
	}
}
