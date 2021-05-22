using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyVanillaAlchemyIngredient = "Any Vanilla Alchemy Ingredient";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups4( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyVanillaAlchemyIngredient,
				grpDeps: new string[] {
					ItemGroupIDs.AnyVanillaAlchemyHerb,
					ItemGroupIDs.AnyVanillaAlchemyFish,
					ItemGroupIDs.AnyVanillaAlchemyMisc },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					switch( item.type ) {
					case ItemID.BottledWater:
					case ItemID.Bottle:
						return true;
					}
					return grps[ItemGroupIDs.AnyVanillaAlchemyHerb].Contains( item.type )
						|| grps[ItemGroupIDs.AnyVanillaAlchemyFish].Contains( item.type )
						|| grps[ItemGroupIDs.AnyVanillaAlchemyMisc].Contains( item.type );
				} )
			) );
		}
	}
}
