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

		public static IEnumerable<Item> GetOutsourcedItems( Player player ) {
			var mymod = ModHelpersMod.Instance;

			return mymod.RecipeHack.IngredientOutsources.Values
				.SelectMany( src => src( player ) );
		}
	}
}
