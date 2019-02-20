using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static class PlayerModHelpers {
		public static void ModdedExtensionsReset( Player player ) {
			foreach( Mod mod in ModLoader.LoadedMods ) {
				try {
					mod.Call( "ResetPlayerModData", player );
				} catch( Exception e ) {
					LogHelpers.Warn( e.ToString() );
				}
			}

			var wingMod = ModLoader.GetMod( "WingSlot" );
			var thoriumMod = ModLoader.GetMod( "ThoriumMod" );
			var weaponOutMod = ModLoader.GetMod( "WeaponOut" );
			var weaponOutLiteMod = ModLoader.GetMod( "WeaponOutLite" );

			if( wingMod != null ) {
				ModPlayer modplayer = player.GetModPlayer( wingMod, "WingSlotPlayer" );

				PlayerModHelpers.RemoveWingSlotProperty( modplayer, "EquipSlot" );
				PlayerModHelpers.RemoveWingSlotProperty( modplayer, "VanitySlot" );
				PlayerModHelpers.RemoveWingSlotProperty( modplayer, "DyeSlot" );
			}

			if( thoriumMod != null ) {
				ModPlayer modplayer = player.GetModPlayer( thoriumMod, "ThoriumPlayer" );

				// "Inspiration" resets to the recommended default:
				ReflectionHelpers.Set( modplayer, "bardResource", 8 );
			}

			if( weaponOutMod != null ) {
				ModPlayer modplayer = player.GetModPlayer( weaponOutMod, "PlayerFX" );

				// "Frenzy Heart" resets:
				ReflectionHelpers.Set( modplayer, "demonBlood", false );
			}

			if( weaponOutLiteMod != null ) {
				ModPlayer modplayer = player.GetModPlayer( weaponOutLiteMod, "PlayerFX" );

				// "Frenzy Heart" resets:
				ReflectionHelpers.Set( modplayer, "demonBlood", false );
			}
		}


		////////////////

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
					LogHelpers.Warn( "Invalid Wing Mod item slot for " + propName );
				}
			} else {
				LogHelpers.Log( "No Wing Mod item slot recognized for " + propName );
			}
		}
	}
}
