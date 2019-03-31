using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.RecipeHack {
	public partial class RecipeHack {
		private static int AwaitingRecipeIdx = -1;
		private static IDictionary<int, int> AwaitingRecipeMissingIngredients = null;



		////////////////

		internal static void AwaitCraft( Player player, int recipeIdx ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod.RecipeHack.IngredientOutsources.Count == 0 ) {
				return;
			}

			int[] _;
			Recipe recipe = Main.recipe[ recipeIdx ];
			IDictionary<int, int> missingItemTypesStacks;

			if( RecipeHelpers.GetRecipeFailReasons( Main.LocalPlayer, recipe, out _, out missingItemTypesStacks) != 0 ) {
				RecipeHack.AwaitingRecipeIdx = recipeIdx;
				RecipeHack.AwaitingRecipeMissingIngredients = missingItemTypesStacks;
			} else {
				RecipeHack.AwaitingRecipeIdx = -1;
				RecipeHack.AwaitingRecipeMissingIngredients = null;
			}
		}

		internal static void ConfirmCraft( Player player, Recipe recipe ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod.RecipeHack.IngredientOutsources.Count == 0 ) {
				return;
			}

			if( RecipeHack.AwaitingRecipeIdx == -1 || RecipeHack.AwaitingRecipeMissingIngredients == null ) {
LogHelpers.Log("0");
				return;
			}
			if( !RecipeIdentityHelpers.Equals( Main.recipe[RecipeHack.AwaitingRecipeIdx], recipe ) ) {
LogHelpers.Log("1");
				RecipeHack.AwaitingRecipeIdx = -1;
				RecipeHack.AwaitingRecipeMissingIngredients = null;
				return;
			}
			if( Main.mouseItem.IsNotTheSameAs( recipe.createItem ) ) {
LogHelpers.Log("2");
				RecipeHack.AwaitingRecipeIdx = -1;
				RecipeHack.AwaitingRecipeMissingIngredients = null;
				return;
			}
			
LogHelpers.Log("3a");
			IEnumerable<Item> outsourcedItems = RecipeHack.GetOutsourcedItems( player );

			ItemHelpers.ConsumeItems( RecipeHack.AwaitingRecipeMissingIngredients, outsourcedItems );

			RecipeHack.AwaitingRecipeIdx = -1;
			RecipeHack.AwaitingRecipeMissingIngredients = null;
LogHelpers.Log("3b "+recipe.createItem.HoverName);
		}
	}
}
