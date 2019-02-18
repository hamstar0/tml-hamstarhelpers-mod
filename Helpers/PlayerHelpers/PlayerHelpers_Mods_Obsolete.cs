using HamstarHelpers.Helpers.DotNetHelpers;
using Terraria;
using Terraria.ModLoader;
using System;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static partial class PlayerHelpers {
		[Obsolete( "use PlayerModHelpers.ModdedExtensionsReset(Player)", true)]
		public static void ModdedExtensionsReset( Player player ) {
			var wingmod = ModLoader.GetMod( "Wing Slot" );

			if( wingmod != null ) {
				PlayerHelpers.WingModReset( wingmod, player );
			}
		}


		private static void WingModReset( Mod wingmod, Player player ) {
			object _;
			ModPlayer mywingplayer = player.GetModPlayer( wingmod, "WingSlotPlayer" );
			Item wingItem;

			object wingEquipSlot;
			if( ReflectionHelpers.GetField(mywingplayer, "EquipSlot", out wingEquipSlot) && wingEquipSlot != null ) {
				if( ReflectionHelpers.GetProperty( wingEquipSlot, "Item", out wingItem ) ) {
					if( wingItem != null && !wingItem.IsAir ) {
						ReflectionHelpers.SetProperty( wingEquipSlot, "Item", new Item() );
						ReflectionHelpers.SetField( mywingplayer, "EquipSlot", wingEquipSlot );
					}
				}
			}

			object wingVanitySlot;
			if( ReflectionHelpers.GetField(mywingplayer, "VanitySlot", out wingVanitySlot) && wingVanitySlot != null ) {
				if( ReflectionHelpers.GetProperty( wingVanitySlot, "Item", out wingItem ) && wingItem != null && !wingItem.IsAir ) {
					ReflectionHelpers.SetProperty( wingVanitySlot, "Item", new Item() );
					ReflectionHelpers.SetField( mywingplayer, "VanitySlot", wingVanitySlot );
				}
			}

			object wingDyeSlot;

			if( ReflectionHelpers.GetField(mywingplayer, "DyeSlot", out wingDyeSlot) && wingDyeSlot != null ) {
				if( ReflectionHelpers.GetProperty( wingDyeSlot, "Item", out wingItem ) && wingItem != null && !wingItem.IsAir ) {
					ReflectionHelpers.SetProperty( wingDyeSlot, "Item", new Item() );
					ReflectionHelpers.SetField( mywingplayer, "DyeSlot", wingDyeSlot );
				}
			}
		}
	}
}
