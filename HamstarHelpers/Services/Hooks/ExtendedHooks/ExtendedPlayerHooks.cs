using System;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for player-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class ExtendedPlayerHooks {
		/// <summary>
		/// Runs when a buff expires.
		/// </summary>
		/// <param name="hookName">Identifier of the hook.</param>
		/// <param name="action">Action to run. Includes the player and buff type id as parameters.</param>
		/// <returns>`false` if hook is already defined.</returns>
		public static bool AddBuffExpireAction( string hookName, Action<Player, int> action ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				if( playerHooks.BuffExpireHooks.ContainsKey( hookName ) ) {
					return false;
				}
				playerHooks.BuffExpireHooks[hookName] = action;
			}

			return true;
		}

		/// <summary>
		/// Runs when an armor item is equipped.
		/// </summary>
		/// <param name="hookName">Identifier of the hook.</param>
		/// <param name="action">Action to run. Includes the player, `player.armor` index, and the armor `Item` as
		/// parameters.</param>
		/// <returns>`false` if hook is already defined.</returns>
		public static bool AddArmorEquipAction( string hookName, Action<Player, int, Item> action ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				if( playerHooks.ArmorEquipHooks.ContainsKey( hookName ) ) {
					return false;
				}
				playerHooks.ArmorEquipHooks[ hookName ] = action;
			}

			return true;
		}

		/// <summary>
		/// Runs when an armor item is unequipped.
		/// </summary>
		/// <param name="hookName">Identifier of the hook.</param>
		/// <param name="action">Action to run. Includes the player, `player.armor` index, and the armor `Item` as
		/// parameters.</param>
		/// <returns>`false` if hook is already defined.</returns>
		public static bool AddArmorUnequipAction( string hookName, Action<Player, int, int> action ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			lock( ExtendedPlayerHooks.MyLock ) {
				if( playerHooks.ArmorUnequipHooks.ContainsKey( hookName ) ) {
					return false;
				}
				playerHooks.ArmorUnequipHooks[ hookName ] = action;
			}

			return true;
		}


		////////////////

		internal static void OnBuffExpire( Player player, int buffId ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			Action<Player, int>[] hooks;
			lock( ExtendedPlayerHooks.MyLock ) {
				hooks = playerHooks.BuffExpireHooks.Values.ToArray();
			}

			foreach( var action in hooks ) {
				action( player, buffId );
			}
		}
		
		internal static void OnArmorEquip( Player player, int slot, Item item ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			Action<Player, int, Item>[] hooks;
			lock( ExtendedPlayerHooks.MyLock ) {
				hooks = playerHooks.ArmorEquipHooks.Values.ToArray();
			}

			foreach( var action in hooks ) {
				action( player, slot, item );
			}
		}

		internal static void OnArmorUnequip( Player player, int slot, int itemType ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			Action<Player, int, int>[] hooks;
			lock( ExtendedPlayerHooks.MyLock ) {
				hooks = playerHooks.ArmorUnequipHooks.Values.ToArray();
			}

			foreach( var action in hooks ) {
				action( player, slot, itemType );
			}
		}
	}
}
