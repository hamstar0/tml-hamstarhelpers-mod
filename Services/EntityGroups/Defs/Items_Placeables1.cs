using HamstarHelpers.Helpers.DebugHelpers;
using System;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemPlaceablesGroups1( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Placeable", null,
				( item, grps ) => {
					return item.createTile != -1 || item.createWall != -1;
				} );
			addDef( "Any Tile", null,
				( item, grps ) => {
					return item.createTile != -1;
				} );
			addDef( "Any Wall", null,
				( item, grps ) => {
					return item.createWall != -1;
				} );
		}
	}
}
