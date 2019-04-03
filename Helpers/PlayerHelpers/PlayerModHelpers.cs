﻿using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static class PlayerModHelpers {
		public static void ModdedExtensionsReset( Player player ) {
			PlayerModHelpers.ModdedExtensionsReset( player, new HashSet<string>() );
		}
		
		public static void ModdedExtensionsReset( Player player, ISet<string> exemptMods ) {
			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( exemptMods.Contains(mod.Name) ) { continue; }

				try {
					mod.Call( "ResetPlayerModData", player );
				} catch( Exception e ) {
					LogHelpers.Warn( "Mod.Call failed for " + mod.Name+": "+e.ToString() );
				}
			}

			var wingSlotMod = ModLoader.GetMod( "WingSlot" );
			var thoriumMod = ModLoader.GetMod( "ThoriumMod" );
			var weaponOutMod = ModLoader.GetMod( "WeaponOut" );
			var weaponOutLiteMod = ModLoader.GetMod( "WeaponOutLite" );

			if( wingSlotMod != null && !exemptMods.Contains("WingSlot") ) {
				ModPlayer modplayer = player.GetModPlayer( wingSlotMod, "WingSlotPlayer" );

				PlayerModHelpers.RemoveWingSlotProperty( modplayer, "EquipSlot" );
				PlayerModHelpers.RemoveWingSlotProperty( modplayer, "VanitySlot" );
				PlayerModHelpers.RemoveWingSlotProperty( modplayer, "DyeSlot" );
			}

			if( thoriumMod != null && !exemptMods.Contains("ThoriumMod") ) {
				ModPlayer modplayer = player.GetModPlayer( thoriumMod, "ThoriumPlayer" );

				// "Inspiration" resets to the recommended default:
				ReflectionHelpers.Set( modplayer, "bardResource", 8 );
			}

			if( weaponOutMod != null && !exemptMods.Contains("WeaponOut") ) {
				ModPlayer modplayer = player.GetModPlayer( weaponOutMod, "PlayerFX" );

				// "Frenzy Heart" resets:
				ReflectionHelpers.Set( modplayer, "demonBlood", false );
			}

			if( weaponOutLiteMod != null && !exemptMods.Contains("WeaponOutLite") ) {
				ModPlayer modplayer = player.GetModPlayer( weaponOutLiteMod, "PlayerFX" );

				// "Frenzy Heart" resets:
				ReflectionHelpers.Set( modplayer, "demonBlood", false );
			}
		}


		////////////////

		private static void RemoveWingSlotProperty( ModPlayer mywingplayer, string propName ) {
			object wingEquipSlot;

			if( ReflectionHelpers.Get( mywingplayer, propName, out wingEquipSlot ) && wingEquipSlot != null ) {
				Item wingItem;

				if( ReflectionHelpers.Get( wingEquipSlot, "Item", out wingItem ) ) {
					if( wingItem != null && !wingItem.IsAir ) {
						ReflectionHelpers.Set( wingEquipSlot, "Item", new Item() );
						ReflectionHelpers.Set( mywingplayer, propName, wingEquipSlot );
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
