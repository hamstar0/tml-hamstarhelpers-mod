using HamstarHelpers.DebugHelpers;
using HamstarHelpers.DotNetHelpers;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.PlayerHelpers {
	public static class PlayerModHelpers {
		private static void RemoveWingSlotProperty( ModPlayer mywingplayer, string prop_name ) {
			bool success;
			object wing_equip_slot = ReflectionHelpers.GetField( mywingplayer, prop_name, out success );

			if( success && wing_equip_slot != null ) {
				Item wing_item = (Item)ReflectionHelpers.GetProperty( wing_equip_slot, "Item", out success );

				if( success ) {
					if( wing_item != null && !wing_item.IsAir ) {
						ReflectionHelpers.SetProperty( wing_equip_slot, "Item", new Item(), out success );
						ReflectionHelpers.SetField( mywingplayer, prop_name, wing_equip_slot, out success );
					}
				} else {
					LogHelpers.Log( "Invalid Wing Mod item slot for " + prop_name );
				}
			} else {
				LogHelpers.Log( "No Wing Mod item slot recognized for " + prop_name );
			}
		}


		public static void ModdedExtensionsReset( Player player ) {
			var wing_mod = ModLoader.GetMod( "WingSlot" );

			if( wing_mod != null ) {
				ModPlayer mywingplayer = player.GetModPlayer( wing_mod, "WingSlotPlayer" );

				PlayerModHelpers.RemoveWingSlotProperty( mywingplayer, "EquipSlot" );
				PlayerModHelpers.RemoveWingSlotProperty( mywingplayer, "VanitySlot" );
				PlayerModHelpers.RemoveWingSlotProperty( mywingplayer, "DyeSlot" );
			}
		}
	}
}
