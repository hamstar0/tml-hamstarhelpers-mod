using System;
using System.Collections.Generic;
using Terraria;

namespace HamstarHelpers.TmlHelpers {
	public class TmlPlayerHelpers {
		internal static IDictionary<string, Action<Player, int>> BuffExpireHooks;
		internal static IDictionary<string, Action<Player, int, Item>> ArmorEquipHooks;
		internal static IDictionary<string, Action<Player, int, int>> ArmorUnequipHooks;

		static TmlPlayerHelpers() {
			TmlPlayerHelpers.Reset();
		}
		internal static void Reset() {
			TmlPlayerHelpers.BuffExpireHooks = new Dictionary<string, Action<Player, int>>();
			TmlPlayerHelpers.ArmorEquipHooks = new Dictionary<string, Action<Player, int, Item>>();
			TmlPlayerHelpers.ArmorUnequipHooks = new Dictionary<string, Action<Player, int, int>>();
		}


		////////////////

		public static bool AddBuffExpireAction( string which, Action<Player, int> action ) {
			if( TmlPlayerHelpers.BuffExpireHooks.ContainsKey(which) ) { return false; }
			TmlPlayerHelpers.BuffExpireHooks[ which ] = action;
			return true;
		}

		public static bool AddArmorEquipAction( string which, Action<Player, int, Item> action ) {
			if( TmlPlayerHelpers.ArmorEquipHooks.ContainsKey( which ) ) { return false; }
			TmlPlayerHelpers.ArmorEquipHooks[ which ] = action;
			return true;
		}

		public static bool AddArmorUnequipAction( string which, Action<Player, int, int> action ) {
			if( TmlPlayerHelpers.ArmorUnequipHooks.ContainsKey( which ) ) { return false; }
			TmlPlayerHelpers.ArmorUnequipHooks[ which ] = action;
			return true;
		}


		////////////////

		internal static void OnBuffExpire( Player player, int buff_id ) {
			foreach( var action in TmlPlayerHelpers.BuffExpireHooks ) {
				action.Value( player, buff_id );
			}
		}
		
		internal static void OnArmorEquip( Player player, int slot, Item item ) {
			foreach( var action in TmlPlayerHelpers.ArmorEquipHooks ) {
				action.Value( player, slot, item );
			}
		}

		internal static void OnArmorUnequip( Player player, int slot, int item_type ) {
			foreach( var action in TmlPlayerHelpers.ArmorUnequipHooks ) {
				action.Value( player, slot, item_type );
			}
		}
	}
}
