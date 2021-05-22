using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.Recipes;


namespace HamstarHelpers.Services.RecipeHack {
	/// <summary>
	/// Provides a method for forcing new recipes into the player's recipe selection list. May be unstable. Has known mod incompatibilities.
	/// </summary>
	[Obsolete("Will be replaced eventually")]
	public partial class RecipeHack {
		/// @private
		public static void ForceRecipeRefresh() {
			var mymod = ModHelpersMod.Instance;
			mymod.RecipeHack.RefreshTimer = 0;
		}


		////////////////

		/// @private
		public static void RegisterIngredientSource( string sourceName, Func<Player, IEnumerable<Item>> itemSource ) {
			var mymod = ModHelpersMod.Instance;
			mymod.RecipeHack.IngredientOutsources[ sourceName ] = itemSource;
		}

		/// @private
		public static void UnregisterIngredientSource( string sourceName ) {
			var mymod = ModHelpersMod.Instance;
			mymod.RecipeHack.IngredientOutsources.Remove( sourceName );
		}


		////////////////

		/// @private
		public static IEnumerable<Item> GetOutsourcedItems( Player player ) {
			return ModHelpersMod.Instance.RecipeHack.IngredientOutsources.Values
					.SelectMany( src => src(player) );
		}


		////////////////

		/// @private
		public static ISet<int> GetAvailableRecipesOfIngredients( Player player, IEnumerable<Item> ingredients ) {
			int[] _;
			IDictionary<int, int> __;
			ISet<int> addedRecipeIdxs = new HashSet<int>();
			ISet<int> possibleRecipeIdxs = new HashSet<int>();

			// Find all potential recipes of each individual ingredient
			foreach( Item ingredient in ingredients ) {
				IEnumerable<int> ingredientRecipeIdxs = RecipeIdentityLibraries.GetRecipeIndexesUsingIngredient( ingredient.netID );
				possibleRecipeIdxs.UnionWith( ingredientRecipeIdxs );
			}

			// Filter potential recipes list to actual recipes only
			foreach( int recipeIdx in possibleRecipeIdxs ) {
				Recipe recipe = Main.recipe[recipeIdx];
				if( recipe == null || recipe.createItem.type == ItemID.None ) { continue; } // Just in case?

				if( RecipeLibraries.GetRecipeFailReasons( player, recipe, out _, out __, ingredients ) == 0 ) {
					addedRecipeIdxs.Add( recipeIdx );
				}
			}

			return addedRecipeIdxs;
		}
	}
}
