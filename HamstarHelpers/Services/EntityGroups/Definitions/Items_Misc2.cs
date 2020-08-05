using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Recipes;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyFoodIngredient = "Any Food Ingredient";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups2( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyFoodIngredient,
				grpDeps: new string[] { ItemGroupIDs.AnyFood },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					foreach( int foodId in grps[ItemGroupIDs.AnyFood] ) {
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
