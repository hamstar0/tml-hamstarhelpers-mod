using HamstarHelpers.Helpers.Debug;
using System;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups4( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Vanilla Alchemy Ingredient",
				new string[] { "Any Vanilla Alchemy Herb", "Any Vanilla Alchemy Fish", "Any Vanilla Alchemy Misc" },
				( item, grps ) => {
					switch( item.type ) {
					case ItemID.BottledWater:
					case ItemID.Bottle:
						return true;
					}
					return grps["Any Vanilla Alchemy Herb"].Contains( item.type )
						|| grps["Any Vanilla Alchemy Fish"].Contains( item.type )
						|| grps["Any Vanilla Alchemy Misc"].Contains( item.type );
				} );
		}
	}
}
