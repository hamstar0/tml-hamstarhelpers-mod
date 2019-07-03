using System;
using Terraria;


namespace HamstarHelpers.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for assorted helpful functions not currently available in tModLoader.
	/// </summary>
	public partial class ExtendedPlayerHooks {
		public static bool AddBuffExpireAction( string which, Action<Player, int> action ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				if( playerHooks.BuffExpireHooks.ContainsKey( which ) ) { return false; }
				playerHooks.BuffExpireHooks[which] = action;
			}

			return true;
		}

		public static bool AddArmorEquipAction( string which, Action<Player, int, Item> action ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				if( playerHooks.ArmorEquipHooks.ContainsKey( which ) ) { return false; }
				playerHooks.ArmorEquipHooks[ which ] = action;
			}

			return true;
		}

		public static bool AddArmorUnequipAction( string which, Action<Player, int, int> action ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				if( playerHooks.ArmorUnequipHooks.ContainsKey( which ) ) { return false; }
				playerHooks.ArmorUnequipHooks[ which ] = action;
			}

			return true;
		}


		////////////////

		internal static void OnBuffExpire( Player player, int buffId ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				foreach( var action in playerHooks.BuffExpireHooks ) {
					action.Value( player, buffId );
				}
			}
		}
		
		internal static void OnArmorEquip( Player player, int slot, Item item ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				foreach( var action in playerHooks.ArmorEquipHooks ) {
					action.Value( player, slot, item );
				}
			}
		}

		internal static void OnArmorUnequip( Player player, int slot, int itemType ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				foreach( var action in playerHooks.ArmorUnequipHooks ) {
					action.Value( player, slot, itemType );
				}
			}
		}
	}
}
