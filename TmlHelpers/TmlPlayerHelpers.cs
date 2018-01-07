using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.TmlHelpers {
	public class TmlPlayerHelpers {
		public static string GetUniqueId( Player player ) {
			return player.GetModPlayer<HamstarHelpersPlayer>().UID;
		}


		////////////////

		internal IDictionary<string, Action<Player, int>> BuffExpireHooks = new Dictionary<string, Action<Player, int>>();
		internal IDictionary<string, Action<Player, int, Item>> ArmorEquipHooks = new Dictionary<string, Action<Player, int, Item>>();
		internal IDictionary<string, Action<Player, int, int>> ArmorUnequipHooks = new Dictionary<string, Action<Player, int, int>>();

		
		////////////////

		public static bool AddBuffExpireAction( string which, Action<Player, int> action ) {
			var tml_player = HamstarHelpersMod.Instance.TmlPlayerHelpers;

			if( tml_player.BuffExpireHooks.ContainsKey(which) ) { return false; }
			tml_player.BuffExpireHooks[ which ] = action;
			return true;
		}

		public static bool AddArmorEquipAction( string which, Action<Player, int, Item> action ) {
			var tml_player = HamstarHelpersMod.Instance.TmlPlayerHelpers;

			if( tml_player.ArmorEquipHooks.ContainsKey( which ) ) { return false; }
			tml_player.ArmorEquipHooks[ which ] = action;
			return true;
		}

		public static bool AddArmorUnequipAction( string which, Action<Player, int, int> action ) {
			var tml_player = HamstarHelpersMod.Instance.TmlPlayerHelpers;

			if( tml_player.ArmorUnequipHooks.ContainsKey( which ) ) { return false; }
			tml_player.ArmorUnequipHooks[ which ] = action;
			return true;
		}


		////////////////

		internal static void OnBuffExpire( Player player, int buff_id ) {
			var tml_player = HamstarHelpersMod.Instance.TmlPlayerHelpers;

			foreach( var action in tml_player.BuffExpireHooks ) {
				action.Value( player, buff_id );
			}
		}
		
		internal static void OnArmorEquip( Player player, int slot, Item item ) {
			var tml_player = HamstarHelpersMod.Instance.TmlPlayerHelpers;

			foreach( var action in tml_player.ArmorEquipHooks ) {
				action.Value( player, slot, item );
			}
		}

		internal static void OnArmorUnequip( Player player, int slot, int item_type ) {
			var tml_player = HamstarHelpersMod.Instance.TmlPlayerHelpers;

			foreach( var action in tml_player.ArmorUnequipHooks ) {
				action.Value( player, slot, item_type );
			}
		}
	}
}
