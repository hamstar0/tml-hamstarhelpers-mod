using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			addDef( "Any Item", null,
				( item, grps ) => {
					return true;
				} );

			for( int i = -12; i <= ItemRarityAttributeHelpers.HighestVanillaRarity; i++ ) {
				if( i >= -10 && i <= -3 ) { i = -2; }

				int tier = i;
				addDef( "Any " + ItemRarityAttributeHelpers.RarityColorText[i] + " Tier", null,
					( item, grps ) => {
						return item.rare == tier;
					} );
			}

			addDef( "Any Dye", null,
				( item, grps ) => {
					return item.dye != 0 || item.hairDye != 0;
				} );

			addDef( "Any Food", null,
				( item, grps ) => {
					return item.buffType == BuffID.WellFed;
				} );
		}
	}
}
