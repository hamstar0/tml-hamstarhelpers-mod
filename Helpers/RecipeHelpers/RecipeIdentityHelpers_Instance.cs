using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeIdentityHelpers {
		private static object MyLock = new object();



		////////////////

		private IDictionary<int, IList<Recipe>> RecipesByItem = new Dictionary<int, IList<Recipe>>();
		private IDictionary<int, IList<int>> RecipeIndicesByItem = new Dictionary<int, IList<int>>();



		////////////////

		private void CacheRecipesOfItem( int itemType ) {
			lock( RecipeIdentityHelpers.MyLock ) {
				for( int i = 0; i < Main.recipe.Length; i++ ) {
					Recipe recipe = Main.recipe[i];
					int recipeItemType = recipe.createItem.type;
					if( recipeItemType == 0 ) {
						break;
					}

					if( !this.RecipesByItem.ContainsKey( recipeItemType ) ) {
						this.RecipesByItem[recipeItemType] = new List<Recipe>();
					}
					this.RecipesByItem[recipeItemType].Add( recipe );

					if( !this.RecipeIndicesByItem.ContainsKey( recipeItemType ) ) {
						this.RecipeIndicesByItem[recipeItemType] = new List<int>();
					}
					this.RecipeIndicesByItem[recipeItemType].Add( i );
				}
			}
		}
	}
}
