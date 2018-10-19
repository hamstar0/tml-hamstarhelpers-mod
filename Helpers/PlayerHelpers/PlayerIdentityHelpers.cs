using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.ItemHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.PlayerHelpers {
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
			string id;
			if( !ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds.TryGetValue( player.whoAmI, out id ) ) {
				throw new HamstarException("!ModHelpers.PlayerIdentityHelpers.GetProperUniqueId - Could not find player "+player.name+"'s id.");
			}
			return id;
		}


		public static Player GetPlayerByProperId( string uid ) {
			int len = Main.player.Length;

			for( int i=0; i<len; i++ ) {
				Player plr = Main.player[ i ];
				if( plr == null || !plr.active ) { continue; }
				
				if( PlayerIdentityHelpers.GetProperUniqueId(plr) == uid ) {
					return plr;
				}
			}

			return null;
		}


		////////////////

		public static int GetVanillaSnapshotHash( Player player, bool no_context, bool looks_matter ) {
			int hash = EntityHelpers.EntityHelpers.GetVanillaSnapshotHash( player, no_context );
			int item_hash;

			hash ^= ( "statLifeMax" + player.statLifeMax ).GetHashCode();
			hash ^= ( "statManaMax" + player.statManaMax ).GetHashCode();
			hash ^= ( "extraAccessory" + player.extraAccessory ).GetHashCode();
			hash ^= ( "difficulty" + player.difficulty ).GetHashCode();

			if( !no_context ) {
				hash ^= ( "team" + player.team ).GetHashCode();
				hash ^= ( "hostile" + player.hostile ).GetHashCode();   //pvp?
				hash ^= ( "name" + player.name ).GetHashCode();
			}

			if( looks_matter ) {
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
					item_hash = ( "inv" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context, true );
				}
				hash ^= item_hash;
			}
			for( int i = 0; i < player.armor.Length; i++ ) {
				Item item = player.armor[i];
				if( item == null || !item.active || item.stack == 0 ) {
					item_hash = ( "arm" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context, true );
				}
				hash ^= item_hash;
			}
			for( int i = 0; i < player.bank.item.Length; i++ ) {
				Item item = player.bank.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					item_hash = ( "bank" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context, true );
				}
				hash ^= item_hash;
			}
			for( int i = 0; i < player.bank2.item.Length; i++ ) {
				Item item = player.bank2.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					item_hash = ( "bank2" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context, true );
				}
				hash ^= item_hash;
			}
			for( int i = 0; i < player.bank3.item.Length; i++ ) {
				Item item = player.bank3.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					item_hash = ( "bank3" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context, true );
				}
				hash ^= item_hash;
			}
			return hash;
		}


		////////////////

		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();
	}
}
