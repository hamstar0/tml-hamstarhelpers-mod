using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.RecipeHack {
	public partial class RecipeHack {
		private static object MyLock = new object();



		////////////////

		internal static void BindRecipeItem( int recipeIdx ) {
			var myitem = Main.recipe[ recipeIdx ].createItem.GetGlobalItem<ModHelpersItem>();
			myitem.FromRecipeIdx = recipeIdx;
		}

		private static void ForceAddRecipe( int recipeIdx ) {
			float y = 0f;
			if( Main.numAvailableRecipes > 0 ) {
				y = Main.availableRecipeY[Main.numAvailableRecipes - 1] + 65f;
			}

			Main.availableRecipe[Main.numAvailableRecipes] = recipeIdx;
			Main.availableRecipeY[Main.numAvailableRecipes] = y;
			Main.numAvailableRecipes++;
		}



		////////////////

		private IDictionary<string, Func<Player, IEnumerable<Item>>> IngredientOutsources = new Dictionary<string, Func<Player, IEnumerable<Item>>>();
		private int LastAvailableRecipeCount = -1;
		private int RefreshTimer = 300;
		private int OldFocusRecipe = -1;



		////////////////

		internal RecipeHack() {
			Promises.Promises.AddPostModLoadPromise( () => {
				for( int i = 0; i < Recipe.maxRecipes; i++ ) {
					if( Main.recipe[i].createItem.type == 0 ) { break; }

					RecipeHack.BindRecipeItem( i );
				}
			} );
		}


		////////////////

		internal void Update() {
			if( this.IngredientOutsources.Count == 0 ) { return; }

			if( this.LastAvailableRecipeCount != Main.numAvailableRecipes || this.RefreshTimer-- < 0 ) {
				this.UpdateRecipes();

				this.LastAvailableRecipeCount = Main.numAvailableRecipes;
				this.RefreshTimer = 300;
			}

			this.OldFocusRecipe = Main.focusRecipe;
		}

		private void UpdateRecipes() {
			try {
				IEnumerable<Item> ingredients = RecipeHack.GetOutsourcedItems( Main.LocalPlayer );
				IList<int> addedRecipeIndexes = RecipeHack.GetAvailableRecipesOfIngredients( Main.LocalPlayer, ingredients );
				ISet<int> availRecipeIdxSet = new HashSet<int>( Main.availableRecipe.Take(Main.numAvailableRecipes) );

				foreach( int idx in addedRecipeIndexes ) {
					if( !availRecipeIdxSet.Contains( idx ) ) {
						RecipeHack.ForceAddRecipe( idx );
					}
				}

				if( this.OldFocusRecipe >= 0 ) {
					int toIdx = Math.Min( this.OldFocusRecipe, Main.numAvailableRecipes );
					int shift = (Main.focusRecipe - toIdx) * 65;

					for( int i=0; i<Main.numAvailableRecipes; i++ ) {
						Main.availableRecipeY[i] += shift;
					}

					Main.focusRecipe = toIdx;
				}
			} catch( Exception e ) {
				throw new HamstarException( "", e );
			}
		}
	}
}
