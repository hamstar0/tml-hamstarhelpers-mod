using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		//...
	}




	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Item", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return true;
				} )
			) );

			for( int i = -12; i <= ItemRarityAttributeHelpers.HighestVanillaRarity; i++ ) {
				if( i >= -10 && i <= -3 ) { i = -2; }

				int tier = i;
				defs.Add( new EntityGroupMatcherDefinition<Item>(
					"Any " + ItemRarityAttributeHelpers.RarityColorText[i] + " Tier", null,
					new ItemGroupMatcher( ( item, grps ) => {
							return item.rare == tier;
					} )
				) );
			}

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Dye", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.dye != 0 || item.hairDye != 0;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Food", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.buffType == BuffID.WellFed;
				} )
			) );
		}
	}
}
