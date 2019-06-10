using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.RecipeHack {
	public partial class RecipeHack {
		public static void ForceRecipeRefresh() {
			var mymod = ModHelpersMod.Instance;
			mymod.RecipeHack.RefreshTimer = 0;
		}


		////////////////

		public static void RegisterIngredientSource( string sourceName, Func<Player, IEnumerable<Item>> itemSource ) {
			var mymod = ModHelpersMod.Instance;
			mymod.RecipeHack.IngredientOutsources[ sourceName ] = itemSource;
		}

		public static void UnregisterIngredientSource( string sourceName ) {
			var mymod = ModHelpersMod.Instance;
			mymod.RecipeHack.IngredientOutsources.Remove( sourceName );
		}


		////////////////

		public static IEnumerable<Item> GetOutsourcedItems( Player player ) {
			return ModHelpersMod.Instance.RecipeHack.IngredientOutsources.Values
					.SelectMany( src => src(player) );
		}


		////////////////

		public static ISet<int> GetAvailableRecipesOfIngredients( Player player, IEnumerable<Item> ingredients ) {
			int[] _;
			IDictionary<int, int> __;
			ISet<int> addedRecipeIdxs = new HashSet<int>();
			ISet<int> possibleRecipeIdxs = new HashSet<int>();

			// Find all potential recipes of each individual ingredient
			foreach( Item ingredient in ingredients ) {
				IEnumerable<int> ingredientRecipeIdxs = RecipeIdentityHelpers.GetRecipeIndexesUsingIngredient( ingredient.netID );
				possibleRecipeIdxs.UnionWith( ingredientRecipeIdxs );
			}

			// Filter potential recipes list to actual recipes only
			foreach( int recipeIdx in possibleRecipeIdxs ) {
				Recipe recipe = Main.recipe[recipeIdx];
				if( recipe == null || recipe.createItem.type == 0 ) { continue; } // Just in case?

				if( RecipeHelpers.GetRecipeFailReasons( player, recipe, out _, out __, ingredients ) == 0 ) {
					addedRecipeIdxs.Add( recipeIdx );
				}
			}

			return addedRecipeIdxs;
		}
	}
}
