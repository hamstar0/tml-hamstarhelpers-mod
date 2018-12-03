using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static class PlayerModHelpers {
		private static void RemoveWingSlotProperty( ModPlayer mywingplayer, string propName ) {
			object wingEquipSlot;

			if( ReflectionHelpers.GetField( mywingplayer, propName, out wingEquipSlot ) && wingEquipSlot != null ) {
				Item wingItem;

				if( ReflectionHelpers.GetProperty( wingEquipSlot, "Item", out wingItem ) ) {
					if( wingItem != null && !wingItem.IsAir ) {
						ReflectionHelpers.SetProperty( wingEquipSlot, "Item", new Item() );
						ReflectionHelpers.SetField( mywingplayer, propName, wingEquipSlot );
					}
				} else {
					LogHelpers.Log( "Invalid Wing Mod item slot for " + propName );
				}
			} else {
				LogHelpers.Log( "No Wing Mod item slot recognized for " + propName );
			}
		}


		public static void ModdedExtensionsReset( Player player ) {
			var wingMod = ModLoader.GetMod( "WingSlot" );

			if( wingMod != null ) {
				ModPlayer mywingplayer = player.GetModPlayer( wingMod, "WingSlotPlayer" );

				PlayerModHelpers.RemoveWingSlotProperty( mywingplayer, "EquipSlot" );
				PlayerModHelpers.RemoveWingSlotProperty( mywingplayer, "VanitySlot" );
				PlayerModHelpers.RemoveWingSlotProperty( mywingplayer, "DyeSlot" );
			}
		}
	}
}
