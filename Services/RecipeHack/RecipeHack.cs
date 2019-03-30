using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.RecipeHack {
	public partial class RecipeHack {
		public static void RegisterIngredientSource( string sourceName, Func<Player, IEnumerable<Item>> itemSource ) {
			var mymod = ModHelpersMod.Instance;

			mymod.RecipeHack.IngredientOutsources[ sourceName ] = itemSource;
		}

		public static void UnregisterIngredientSource( string sourceName ) {
			var mymod = ModHelpersMod.Instance;

			mymod.RecipeHack.IngredientOutsources.Remove( sourceName );
		}

		
		////////////////

		private static void ForceAddRecipe( int recipeIdx ) {
			float y = 0f;
			if( Main.numAvailableRecipes > 0 ) {
				y = Main.availableRecipeY[Main.numAvailableRecipes - 1] + 65f;
			}

			Main.availableRecipe[Main.numAvailableRecipes] = recipeIdx;
			Main.availableRecipeY[Main.numAvailableRecipes] = y;
			Main.numAvailableRecipes++;
		}
	}
}
