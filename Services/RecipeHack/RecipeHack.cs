using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
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

		internal static bool ConsumeItemsFromSources( Player player, Item[] neededIngredients ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod.RecipeHack.IngredientOutsources.Count == 0 ) {
				return false;
			}

			IDictionary<int, int> ingredientAmounts = new Dictionary<int, int>();
			foreach( Item item in neededIngredients ) {
				ingredientAmounts.AddOrSet( item.netID, item.stack );
			}
			
			Item[] outItems = mymod.RecipeHack.IngredientOutsources.Values
				.SelectMany( src => src( player ) )
				.ToArray();

			ItemHelpers.ConsumeItems( ingredientAmounts, outItems );
			return ingredientAmounts.Count == 0;
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
