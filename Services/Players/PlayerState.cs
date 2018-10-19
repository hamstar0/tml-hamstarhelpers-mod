using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Players {
	public class PlayerState {
		public static bool AddBuffExpireAction( string which, Action<Player, int> action ) {
			var player_state = ModHelpersMod.Instance.PlayerState;

			if( player_state.BuffExpireHooks.ContainsKey(which) ) { return false; }
			player_state.BuffExpireHooks[ which ] = action;
			return true;
		}

		public static bool AddArmorEquipAction( string which, Action<Player, int, Item> action ) {
			var player_state = ModHelpersMod.Instance.PlayerState;

			if( player_state.ArmorEquipHooks.ContainsKey( which ) ) { return false; }
			player_state.ArmorEquipHooks[ which ] = action;
			return true;
		}

		public static bool AddArmorUnequipAction( string which, Action<Player, int, int> action ) {
			var player_state = ModHelpersMod.Instance.PlayerState;

			if( player_state.ArmorUnequipHooks.ContainsKey( which ) ) { return false; }
			player_state.ArmorUnequipHooks[ which ] = action;
			return true;
		}


		////////////////

		internal static void OnBuffExpire( Player player, int buff_id ) {
			var player_state = ModHelpersMod.Instance.PlayerState;

			foreach( var action in player_state.BuffExpireHooks ) {
				action.Value( player, buff_id );
			}
		}
		
		internal static void OnArmorEquip( Player player, int slot, Item item ) {
			var tml_player = ModHelpersMod.Instance.PlayerState;

			foreach( var action in tml_player.ArmorEquipHooks ) {
				action.Value( player, slot, item );
			}
		}

		internal static void OnArmorUnequip( Player player, int slot, int item_type ) {
			var player_state = ModHelpersMod.Instance.PlayerState;

			foreach( var action in player_state.ArmorUnequipHooks ) {
				action.Value( player, slot, item_type );
			}
		}



		////////////////

		internal IDictionary<string, Action<Player, int>> BuffExpireHooks = new Dictionary<string, Action<Player, int>>();
		internal IDictionary<string, Action<Player, int, Item>> ArmorEquipHooks = new Dictionary<string, Action<Player, int, Item>>();
		internal IDictionary<string, Action<Player, int, int>> ArmorUnequipHooks = new Dictionary<string, Action<Player, int, int>>();
	}
}
