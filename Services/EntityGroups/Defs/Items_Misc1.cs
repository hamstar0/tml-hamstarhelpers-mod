using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using System;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	public partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups1( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Item", null,
				( item, grps ) => {
					return true;
				} );

			for( int i = -12; i <= ItemAttributeHelpers.HighestVanillaRarity; i++ ) {
				if( i >= -10 && i <= -3 ) { i = -2; }

				int tier = i;
				addDef( "Any " + ItemAttributeHelpers.RarityColorText[i] + " Tier", null,
					( item, grps ) => {
						return item.rare == tier;
					} );
			}

			addDef( "Any Dye", null,
				( item, grps ) => {
					return item.dye != 0 || item.hairDye != 0;
				} );
		}
	}
}
