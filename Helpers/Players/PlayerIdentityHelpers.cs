using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Entities;
using HamstarHelpers.Helpers.Items;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to unique player identification.
	/// </summary>
	public partial class PlayerIdentityHelpers {
		/// <summary></summary>
		public const int InventorySize = 58;
		/// <summary></summary>
		public const int InventoryHotbarSize = 10;
		/// <summary></summary>
		public const int InventoryMainSize = 40;



		////////////////
		
		/// <summary>
		/// Gets a code to uniquely identify the current player.
		/// </summary>
		/// <returns></returns>
		public static string GetUniqueId() {
			if( Main.netMode == 2 ) {
				throw new HamstarException( "No 'current' player on a server." );
			}

			int hash = Math.Abs( Main.ActivePlayerFileData.Path.GetHashCode() ^ Main.ActivePlayerFileData.IsCloudSave.GetHashCode() );
			return Main.clientUUID + "_" + hash;
		}

		/// <summary>
		/// Gets a code to uniquely identify a given player. These are synced to the server in multiplayer.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string GetUniqueId( Player player ) {
			var mymod = ModHelpersMod.Instance;
			string id;

			if( !mymod.PlayerIdentityHelpers.PlayerIds.TryGetValue( player.whoAmI, out id ) ) {
				if( Main.netMode != 2 && player.whoAmI == Main.myPlayer ) {
					id = PlayerIdentityHelpers.GetUniqueId();
					mymod.PlayerIdentityHelpers.PlayerIds[ player.whoAmI ] = id;
				} else {
					//throw new HamstarException( "!ModHelpers.PlayerIdentityHelpers.GetProperUniqueId - Could not find player " + player.name + "'s id." );
					return null;
				}
			}
			return id;
		}


		/// <summary>
		/// Gets an active player by a given unique id (if present).
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public static Player GetPlayerByUniqueId( string uid ) {
			int len = Main.player.Length;

			for( int i=0; i<len; i++ ) {
				Player plr = Main.player[ i ];
//LogHelpers.Log( "GetPlayerByProperId: "+PlayerIdentityHelpers.GetProperUniqueId( plr )+" == "+uid+": "+plr.name+" ("+plr.whoAmI+")" );
				if( plr == null /*|| !plr.active*/ ) { continue; }	// <- This is WEIRD!
				
				if( PlayerIdentityHelpers.GetUniqueId(plr) == uid ) {
					return plr;
				}
			}

			return null;
		}


		////////////////

		/// <summary>
		/// Gets a unique code for a player's current state. Useful for detecting state changes.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="noContext">Omits team, pvp state, and name.</param>
		/// <param name="looksMatter">Includes appearance elements.</param>
		/// <returns></returns>
		public static int GetVanillaSnapshotHash( Player player, bool noContext, bool looksMatter ) {
			int hash = EntityHelpers.GetVanillaSnapshotHash( player, noContext );
			int itemHash;

			hash ^= ( "statLifeMax" + player.statLifeMax ).GetHashCode();
			hash ^= ( "statManaMax" + player.statManaMax ).GetHashCode();
			hash ^= ( "extraAccessory" + player.extraAccessory ).GetHashCode();
			hash ^= ( "difficulty" + player.difficulty ).GetHashCode();

			if( !noContext ) {
				hash ^= ( "team" + player.team ).GetHashCode();
				hash ^= ( "hostile" + player.hostile ).GetHashCode();   //pvp?
				hash ^= ( "name" + player.name ).GetHashCode();
			}

			if( looksMatter ) {
				hash ^= ( "Male" + player.Male ).GetHashCode();
				hash ^= ( "skinColor" + player.skinColor ).GetHashCode();
				hash ^= ( "hair" + player.hair ).GetHashCode();
				hash ^= ( "hairColor" + player.hairColor ).GetHashCode();
				hash ^= ( "shirtColor" + player.shirtColor ).GetHashCode();
				hash ^= ( "underShirtColor" + player.underShirtColor ).GetHashCode();
				hash ^= ( "pantsColor" + player.pantsColor ).GetHashCode();
				hash ^= ( "shoeColor" + player.shoeColor ).GetHashCode();
			}
			
			for( int i = 0; i < player.inventory.Length; i++ ) {
				Item item = player.inventory[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "inv" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash ^= itemHash;
			}
			for( int i = 0; i < player.armor.Length; i++ ) {
				Item item = player.armor[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "arm" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash ^= itemHash;
			}
			for( int i = 0; i < player.bank.item.Length; i++ ) {
				Item item = player.bank.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "bank" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash ^= itemHash;
			}
			for( int i = 0; i < player.bank2.item.Length; i++ ) {
				Item item = player.bank2.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "bank2" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash ^= itemHash;
			}
			for( int i = 0; i < player.bank3.item.Length; i++ ) {
				Item item = player.bank3.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "bank3" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash ^= itemHash;
			}
			return hash;
		}
	}
}
