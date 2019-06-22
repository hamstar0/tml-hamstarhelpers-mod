using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Entities;
using HamstarHelpers.Helpers.Items;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	/** <summary>Assorted static "helper" functions pertaining to unique player identification.</summary> */
	public partial class PlayerIdentityHelpers {
		public const int InventorySize = 58;
		public const int InventoryHotbarSize = 10;
		public const int InventoryMainSize = 40;



		////////////////
		
		public static string GetMyProperUniqueId() {
			int hash = Math.Abs( Main.ActivePlayerFileData.Path.GetHashCode() ^ Main.ActivePlayerFileData.IsCloudSave.GetHashCode() );
			return Main.clientUUID + "_" + hash;
		}

		public static string GetProperUniqueId( Player player ) {
			var mymod = ModHelpersMod.Instance;
			string id;

			if( !mymod.PlayerIdentityHelpers.PlayerIds.TryGetValue( player.whoAmI, out id ) ) {
				if( Main.netMode != 2 && player.whoAmI == Main.myPlayer ) {
					id = PlayerIdentityHelpers.GetMyProperUniqueId();
					mymod.PlayerIdentityHelpers.PlayerIds[ player.whoAmI ] = id;
				} else {
					//throw new HamstarException( "!ModHelpers.PlayerIdentityHelpers.GetProperUniqueId - Could not find player " + player.name + "'s id." );
					return null;
				}
			}
			return id;
		}


		public static Player GetPlayerByProperId( string uid ) {
			int len = Main.player.Length;

			for( int i=0; i<len; i++ ) {
				Player plr = Main.player[ i ];
//LogHelpers.Log( "GetPlayerByProperId: "+PlayerIdentityHelpers.GetProperUniqueId( plr )+" == "+uid+": "+plr.name+" ("+plr.whoAmI+")" );
				if( plr == null /*|| !plr.active*/ ) { continue; }	// <- This is WEIRD!
				
				if( PlayerIdentityHelpers.GetProperUniqueId(plr) == uid ) {
					return plr;
				}
			}

			return null;
		}


		////////////////

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
