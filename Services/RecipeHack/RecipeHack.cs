using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.RecipeHack {
	public partial class RecipeHack {
		public static void RegisterOutsourcedRecipe( string sourceName, int recipeIdx, Func<IEnumerable<Item>> itemSource ) {
			f
		}

		public static void UnregisterOutsourcedRecipe( string sourceName ) {
			f
		}

		
		////////////////

		private static void ForceAddRecipe( int recipeIdx ) {
			int len = Main.availableRecipe.Length;

			Main.numAvailableRecipes++;
			Array.Copy( Main.availableRecipe, Main.availableRecipe, len + 1 );
			Main.availableRecipe[ len ] = recipeIdx;
		}


		private static bool ForceRemoveRecipe( int recipeIdx ) {
			Recipe recipe = Main.recipe[ recipeIdx ];

			int i = -1;
			for( i = 0; i < Main.availableRecipe.Length; i++ ) {
				if( RecipeHelpers.Equals(recipe, Main.availableRecipe[i]) ) {
					break;
				}
			}

			if( i != -1 ) {
				Main.availableRecipe.RemoveAt( i );
				Main.numAvailableRecipes--;
			}

			return i != -1;
		}
	}
}
