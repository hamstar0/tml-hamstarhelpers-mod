using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using Terraria;
using Terraria.ModLoader;
using System;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static partial class PlayerHelpers {
		[Obsolete( "use `PlayerModHelpers.ModdedExtensionsReset(Player)`", true)]
		public static void ModdedExtensionsReset( Player player ) {
			var wingmod = ModLoader.GetMod( "Wing Slot" );

			if( wingmod != null ) {
				PlayerHelpers._WingModReset( wingmod, player );
			}
		}


		private static void _WingModReset( Mod wingmod, Player player ) {
			ModPlayer mywingplayer = player.GetModPlayer( wingmod, "WingSlotPlayer" );
			Item wingItem;

			object wingEquipSlot;
			if( ReflectionHelpers.Get(mywingplayer, "EquipSlot", out wingEquipSlot) && wingEquipSlot != null ) {
				if( ReflectionHelpers.Get( wingEquipSlot, "Item", out wingItem ) ) {
					if( wingItem != null && !wingItem.IsAir ) {
						ReflectionHelpers.Set( wingEquipSlot, "Item", new Item() );
						ReflectionHelpers.Set( mywingplayer, "EquipSlot", wingEquipSlot );
					}
				}
			}

			object wingVanitySlot;
			if( ReflectionHelpers.Get(mywingplayer, "VanitySlot", out wingVanitySlot) && wingVanitySlot != null ) {
				if( ReflectionHelpers.Get( wingVanitySlot, "Item", out wingItem ) && wingItem != null && !wingItem.IsAir ) {
					ReflectionHelpers.Set( wingVanitySlot, "Item", new Item() );
					ReflectionHelpers.Set( mywingplayer, "VanitySlot", wingVanitySlot );
				}
			}

			object wingDyeSlot;

			if( ReflectionHelpers.Get(mywingplayer, "DyeSlot", out wingDyeSlot) && wingDyeSlot != null ) {
				if( ReflectionHelpers.Get( wingDyeSlot, "Item", out wingItem ) && wingItem != null && !wingItem.IsAir ) {
					ReflectionHelpers.Set( wingDyeSlot, "Item", new Item() );
					ReflectionHelpers.Set( mywingplayer, "DyeSlot", wingDyeSlot );
				}
			}
		}
	}
}
