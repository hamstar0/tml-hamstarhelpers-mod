using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Recipes;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		//...
	}




	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups2( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Food Ingredient",
				new string[] { "Any Food" },
				new ItemGroupMatcher( ( item, grps ) => {
					foreach( int foodId in grps["Any Food"] ) {
						IEnumerable<Recipe> recipes = RecipeFinderHelpers.GetRecipesOfItem( foodId );

						foreach( Recipe recipe in recipes ) {
							for( int i = 0; i < recipe.requiredItem.Length; i++ ) {
								Item reqItem = recipe.requiredItem[i];
								if( reqItem == null || reqItem.IsAir ) { continue; }

								if( reqItem.type == item.type ) {
									return true;
								}
							}
						}
					}
					return false;
				} )
			) );
		}
	}
}
