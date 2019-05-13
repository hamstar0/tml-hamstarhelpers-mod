using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups2( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Food Ingredient",
				new string[] { "Any Food" },
				( item, grps ) => {
					foreach( int foodId in grps["Any Food"] ) {
						IEnumerable<Recipe> recipes = RecipeIdentityHelpers.GetRecipesOfItem( foodId );

						foreach( Recipe recipe in recipes ) {
							for( int i=0; i<recipe.requiredItem.Length; i++ ) {
								Item reqItem = recipe.requiredItem[i];
								if( reqItem == null || reqItem.IsAir ) { continue; }

								if( reqItem.type == item.type ) {
									return true;
								}
							}
						}
					}
					return false;
				} );
		}
	}
}
