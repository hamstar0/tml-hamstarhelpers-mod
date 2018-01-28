using HamstarHelpers.ItemHelpers;
using Terraria;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static class PlayerIdentityHelpers {
		public const int InventorySize = 58;
		public const int InventoryHotbarSize = 10;
		public const int InventoryMainSize = 40;


		////////////////

		public static string GetUniqueId( Player player, out bool has_loaded ) {
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			has_loaded = myplayer.Logic.HasUID;
			return myplayer.Logic.PrivateUID;
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
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context );
				}
				hash ^= item_hash;
			}
			for( int i = 0; i < player.armor.Length; i++ ) {
				Item item = player.armor[i];
				if( item == null || !item.active || item.stack == 0 ) {
					item_hash = ( "arm" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context );
				}
				hash ^= item_hash;
			}
			for( int i = 0; i < player.bank.item.Length; i++ ) {
				Item item = player.bank.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					item_hash = ( "bank" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context );
				}
				hash ^= item_hash;
			}
			for( int i = 0; i < player.bank2.item.Length; i++ ) {
				Item item = player.bank2.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					item_hash = ( "bank2" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context );
				}
				hash ^= item_hash;
			}
			for( int i = 0; i < player.bank3.item.Length; i++ ) {
				Item item = player.bank3.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					item_hash = ( "bank3" + i ).GetHashCode();
				} else {
					item_hash = i + ItemIdentityHelpers.GetVanillaSnapshotHash( item, no_context );
				}
				hash ^= item_hash;
			}
			return hash;
		}
	}
}
