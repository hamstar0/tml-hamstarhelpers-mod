using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.RecipeHack {
	/// <summary>
	/// Provides a way to lock the given current loaded mods with a given world. May also be accessed in-game via. the
	/// Mod Helpers control panel.
	/// </summary>
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
				return;
			}
			if( !RecipeIdentityHelpers.Equals( Main.recipe[RecipeHack.AwaitingRecipeIdx], recipe ) ) {
				RecipeHack.AwaitingRecipeIdx = -1;
				RecipeHack.AwaitingRecipeMissingIngredients = null;
				return;
			}
			if( Main.mouseItem.IsNotTheSameAs( recipe.createItem ) ) {
				RecipeHack.AwaitingRecipeIdx = -1;
				RecipeHack.AwaitingRecipeMissingIngredients = null;
				return;
			}
			
			IDictionary<int, int> outsourcedItemTypeAmounts = RecipeHack.GetOutsourcedItems( player )
				.ToDictionary( item=>item.type, _=>1 );

			ItemHelpers.ConsumeItems( RecipeHack.AwaitingRecipeMissingIngredients, outsourcedItemTypeAmounts, true );

			RecipeHack.AwaitingRecipeIdx = -1;
			RecipeHack.AwaitingRecipeMissingIngredients = null;
		}
	}
}
