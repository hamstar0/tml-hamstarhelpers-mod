using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Libraries.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to mod compatibility for players.
	/// </summary>
	public class PlayerModLibraries {
		/// <summary>
		/// Clears mod data for a player.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="exemptMods">Names of mods to skip (internal names).</param>
		public static void ModdedExtensionsReset( Player player, ISet<string> exemptMods ) {
			foreach( Mod mod in ModLoader.Mods ) {
				if( exemptMods.Contains(mod.Name) ) { continue; }

				try {
					mod.Call( "ResetPlayerModData", player );
				} catch( Exception e ) {
					LogLibraries.Warn( "Mod.Call failed for " + mod.Name+": "+e.ToString() );
				}
			}

			var wingSlotMod = ModLoader.GetMod( "WingSlot" );
			var thoriumMod = ModLoader.GetMod( "ThoriumMod" );
			var weaponOutMod = ModLoader.GetMod( "WeaponOut" );
			var weaponOutLiteMod = ModLoader.GetMod( "WeaponOutLite" );

			if( wingSlotMod != null && !exemptMods.Contains("WingSlot") ) {
				ModPlayer modplayer = player.GetModPlayer( wingSlotMod, "WingSlotPlayer" );

				PlayerModLibraries.RemoveWingSlotProperty( modplayer, "EquipSlot" );
				PlayerModLibraries.RemoveWingSlotProperty( modplayer, "VanitySlot" );
				PlayerModLibraries.RemoveWingSlotProperty( modplayer, "DyeSlot" );
			}

			if( thoriumMod != null && !exemptMods.Contains("ThoriumMod") ) {
				ModPlayer modplayer = player.GetModPlayer( thoriumMod, "ThoriumPlayer" );

				// "Inspiration" resets to the recommended default:
				ReflectionLibraries.Set( modplayer, "bardResource", 8 );
			}

			if( weaponOutMod != null && !exemptMods.Contains("WeaponOut") ) {
				ModPlayer modplayer = player.GetModPlayer( weaponOutMod, "PlayerFX" );

				// "Frenzy Heart" resets:
				ReflectionLibraries.Set( modplayer, "demonBlood", false );
			}

			if( weaponOutLiteMod != null && !exemptMods.Contains("WeaponOutLite") ) {
				ModPlayer modplayer = player.GetModPlayer( weaponOutLiteMod, "PlayerFX" );

				// "Frenzy Heart" resets:
				ReflectionLibraries.Set( modplayer, "demonBlood", false );
			}
		}


		////////////////

		private static void RemoveWingSlotProperty( ModPlayer mywingplayer, string propName ) {
			object wingEquipSlot;

			if( ReflectionLibraries.Get( mywingplayer, propName, out wingEquipSlot ) && wingEquipSlot != null ) {
				Item wingItem;

				if( ReflectionLibraries.Get( wingEquipSlot, "Item", out wingItem ) ) {
					if( wingItem != null && !wingItem.IsAir ) {
						ReflectionLibraries.Set( wingEquipSlot, "Item", new Item() );
						ReflectionLibraries.Set( mywingplayer, propName, wingEquipSlot );
					}
				} else {
					LogLibraries.Warn( "Invalid Wing Mod item slot for " + propName );
				}
			} else {
				LogLibraries.Log( "No Wing Mod item slot recognized for " + propName );
			}
		}
	}
}
