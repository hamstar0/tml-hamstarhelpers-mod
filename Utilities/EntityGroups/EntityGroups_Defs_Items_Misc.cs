using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemMiscGroups3( Action<string, Func<Item, bool>> add_def ) {
			for( int i = -12; i <= ItemAttributeHelpers.HighestVanillaRarity; i++ ) {
				if( i >= -10 && i <= -3 ) { i = -2; }

				int tier = i;
				add_def( "Any "+ItemAttributeHelpers.RarityColorText[i]+ " Tier", ( Item item ) => {
					return item.rare == tier;
				} );
			}

			add_def( "Any Plain Material", ( Item item ) => {
				return item.material &&
					//!EntityGroups.ItemGroups["Any Placeable"].Contains( item.type ) &&
					!EntityGroups.ItemGroups["Any Equipment"].Contains( item.type );
			} );
		}
	}
}
