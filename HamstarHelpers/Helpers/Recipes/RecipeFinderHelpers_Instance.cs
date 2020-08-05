using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;


namespace HamstarHelpers.Helpers.Recipes {
	/// @private
	public partial class RecipeFinderHelpers {
		private static object MyLock = new object();



		////////////////
		
		private IDictionary<int, ISet<int>> RecipeIndexesByItemNetID = new Dictionary<int, ISet<int>>();
		private IDictionary<int, ISet<int>> RecipeIndexesOfIngredientNetIDs = new Dictionary<int, ISet<int>>();



		////////////////

		private void CacheItemRecipes() {
			lock( RecipeFinderHelpers.MyLock ) {
				for( int i = 0; i < Main.recipe.Length; i++ ) {
					Recipe recipe = Main.recipe[i];
					int recipeItemType = recipe.createItem.type;
					if( recipeItemType == 0 ) {
						break;
					}
					
					this.RecipeIndexesByItemNetID.Append2D( recipeItemType, i );
				}
			}
		}


		private void CacheIngredientRecipes() {
			lock( RecipeFinderHelpers.MyLock ) {
				for( int i = 0; i < Main.recipe.Length; i++ ) {
					Recipe recipe = Main.recipe[i];
					if( recipe.createItem.type == ItemID.None ) {
						break;
					}

					for( int j=0; j<recipe.requiredItem.Length; j++ ) {
						Item item = recipe.requiredItem[j];
						if( item == null || item.IsAir ) {
							break;
						}

						this.RecipeIndexesOfIngredientNetIDs.Append2D( item.netID, i );
					}
				}
			}
		}
	}
}
