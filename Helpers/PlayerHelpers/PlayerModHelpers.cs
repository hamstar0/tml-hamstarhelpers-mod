using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static class PlayerModHelpers {
		private static void RemoveWingSlotProperty( ModPlayer mywingplayer, string prop_name ) {
			object wing_equip_slot;

			if( ReflectionHelpers.GetField( mywingplayer, prop_name, out wing_equip_slot ) && wing_equip_slot != null ) {
				Item wing_item;

				if( ReflectionHelpers.GetProperty( wing_equip_slot, "Item", out wing_item ) ) {
					if( wing_item != null && !wing_item.IsAir ) {
						ReflectionHelpers.SetProperty( wing_equip_slot, "Item", new Item() );
						ReflectionHelpers.SetField( mywingplayer, prop_name, wing_equip_slot );
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
