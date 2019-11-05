using HamstarHelpers.Helpers.Debug;
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
		internal static void DefineItemMiscGroups4( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Vanilla Alchemy Ingredient",
				new string[] { "Any Vanilla Alchemy Herb", "Any Vanilla Alchemy Fish", "Any Vanilla Alchemy Misc" },
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.type ) {
					case ItemID.BottledWater:
					case ItemID.Bottle:
						return true;
					}
					return grps["Any Vanilla Alchemy Herb"].Contains( item.type )
						|| grps["Any Vanilla Alchemy Fish"].Contains( item.type )
						|| grps["Any Vanilla Alchemy Misc"].Contains( item.type );
				} )
			) );
		}
	}
}
