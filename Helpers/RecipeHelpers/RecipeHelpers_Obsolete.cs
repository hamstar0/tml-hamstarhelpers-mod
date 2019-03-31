using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeHelpers {
		[Obsolete( "use `RecipeGroupHelpers.Groups`", true )]
		public static IDictionary<string, RecipeGroup> Groups => RecipeGroupHelpers.Groups;
		
		[Obsolete( "use `RecipeIdentityHelpers.Equals(Recipe, Recipe)`", true )]
		public static bool Equals( Recipe recipe1, Recipe recipe2 ) {
			return RecipeIdentityHelpers.Equals( recipe1, recipe2 );
		}

		[Obsolete( "use `RecipeIdentityHelpers.GetRecipesOfItem(int)`", true )]
		public static IList<Recipe> GetRecipesOfItem( int itemType ) {
			return RecipeIdentityHelpers.GetRecipesOfItem( itemType );
		}
		
		[Obsolete( "use `RecipeHelpers.ItemHasIngredients(int, ISet<int>, int)`", true )]
		public static bool ItemHasIngredients( Item item, ISet<int> ingredients, int minStack ) {
			return RecipeHelpers.ItemHasIngredients( item.type, ingredients, minStack );
		}
	}
}
