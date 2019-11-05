using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		//...
	}




	partial class EntityGroupDefs {
		internal static void DefineItemPlaceablesGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Placeable", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile != -1 || item.createWall != -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Tile", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile != -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Wall", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createWall != -1;
				} )
			) );
		}
	}
}
