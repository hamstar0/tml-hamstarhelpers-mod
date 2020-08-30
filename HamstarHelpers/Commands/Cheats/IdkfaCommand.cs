using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Helpers.Players;


namespace HamstarHelpers.Commands.Cheats {
	/// @private
	public class IdkfaCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.World;
		/// @private
		public override string Command => "mh-cheat-idkfa";
		/// @private
		public override string Usage => "/" + this.Command;
		/// @private
		public override string Description => "Gives free (vanilla) end-game equipment. Overrides existing items.";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				LogHelpers.Alert( "Not supposed to run on client." );
				return;
			}

			if( !ModHelpersConfig.Instance.DebugModeCheats ) {
				caller.Reply( "Cheats mode not enabled.", Color.Red );
				return;
			}
			/*if( Main.netMode != NetmodeID.SinglePlayer ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}*/

			int i = 0;
			this.GiveInventoryItem( caller.Player, ItemID.SolarFlarePickaxe, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.SolarFlareAxe, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.SolarEruption, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.SDMG, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.RazorbladeTyphoon, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.RodofDiscord, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.Binoculars, i++ );
			i = 10;
			this.GiveInventoryItem( caller.Player, ItemID.CellPhone, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.CosmicCarKey, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.MinecartMech, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.DualHook, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.SuperHealingPotion, i++, 30 );
			i = PlayerItemHelpers.VanillaInventoryLastCoinSlot - 3;
			this.GiveInventoryItem( caller.Player, ItemID.PlatinumCoin, i++, 99 );
			i = PlayerItemHelpers.VanillaInventoryLastAmmoSlot - 3;
			this.GiveInventoryItem( caller.Player, ItemID.ChlorophyteBullet, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.ChlorophyteBullet, i++ );
			this.GiveInventoryItem( caller.Player, ItemID.ChlorophyteBullet, i++ );

			i = 0;
			this.GiveArmorItem( caller.Player, ItemID.SolarFlareHelmet, i++ );
			this.GiveArmorItem( caller.Player, ItemID.SolarFlareBreastplate, i++ );
			this.GiveArmorItem( caller.Player, ItemID.SolarFlareLeggings, i++ );
			this.GiveArmorItem( caller.Player, ItemID.WingsSolar, i++ );
			this.GiveArmorItem( caller.Player, ItemID.SpectreBoots, i++ );
			this.GiveArmorItem( caller.Player, ItemID.AnkhShield, i++ );
		}


		////

		private void GiveInventoryItem( Player player, int itemType, int invIdx, int stack=1 ) {
			var item = new Item();
			item.SetDefaults( itemType );
			item.stack = stack;

			if( item.pick != 0 ) {
				item.Prefix( PrefixID.Light );
			} else if( item.ranged ) {
				item.Prefix( PrefixID.Unreal );
			} else if( item.melee ) {
				item.Prefix( PrefixID.Legendary );
			}
				
			player.inventory[invIdx] = item;
		}

		private void GiveArmorItem( Player player, int itemType, int invIdx ) {
			var item = new Item();
			item.SetDefaults( itemType );
				
			player.armor[invIdx] = item;
		}
	}
}
