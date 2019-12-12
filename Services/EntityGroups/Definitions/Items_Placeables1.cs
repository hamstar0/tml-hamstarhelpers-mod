using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyPlaceable = "Any Placeable";
		/// <summary></summary>
		public const string AnyTile = "Any Tile";
		/// <summary></summary>
		public const string AnyWall = "Any Wall";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemPlaceablesGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyPlaceable, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile != -1 || item.createWall != -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyTile, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile != -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyWall, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createWall != -1;
				} )
			) );
		}
	}
}
