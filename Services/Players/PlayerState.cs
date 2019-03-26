using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Players {
	public partial class PlayerState {
		public static bool AddBuffExpireAction( string which, Action<Player, int> action ) {
			var playerState = ModHelpersMod.Instance.PlayerState;

			if( playerState.BuffExpireHooks.ContainsKey(which) ) { return false; }
			playerState.BuffExpireHooks[ which ] = action;
			return true;
		}

		public static bool AddArmorEquipAction( string which, Action<Player, int, Item> action ) {
			var playerState = ModHelpersMod.Instance.PlayerState;

			if( playerState.ArmorEquipHooks.ContainsKey( which ) ) { return false; }
			playerState.ArmorEquipHooks[ which ] = action;
			return true;
		}

		public static bool AddArmorUnequipAction( string which, Action<Player, int, int> action ) {
			var playerState = ModHelpersMod.Instance.PlayerState;

			if( playerState.ArmorUnequipHooks.ContainsKey( which ) ) { return false; }
			playerState.ArmorUnequipHooks[ which ] = action;
			return true;
		}


		////////////////

		internal static void OnBuffExpire( Player player, int buffId ) {
			var playerState = ModHelpersMod.Instance.PlayerState;

			foreach( var action in playerState.BuffExpireHooks ) {
				action.Value( player, buffId );
			}
		}
		
		internal static void OnArmorEquip( Player player, int slot, Item item ) {
			var tmlPlayer = ModHelpersMod.Instance.PlayerState;

			foreach( var action in tmlPlayer.ArmorEquipHooks ) {
				action.Value( player, slot, item );
			}
		}

		internal static void OnArmorUnequip( Player player, int slot, int itemType ) {
			var playerState = ModHelpersMod.Instance.PlayerState;

			foreach( var action in playerState.ArmorUnequipHooks ) {
				action.Value( player, slot, itemType );
			}
		}
	}
}
