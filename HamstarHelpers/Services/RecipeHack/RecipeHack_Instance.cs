using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace HamstarHelpers.Services.RecipeHack {
	/// <summary>
	/// Provides a way to lock the given current loaded mods with a given world. May also be accessed in-game via. the
	/// Mod Helpers control panel.
	/// </summary>
	public partial class RecipeHack {
		private static object MyLock = new object();



		////////////////

		internal static void BindRecipeItem( int recipeIdx ) {
			var myitem = Main.recipe[ recipeIdx ].createItem.GetGlobalItem<ModHelpersItem>();
			myitem.FromRecipeIdx = recipeIdx;
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

		private static bool ForceRemoveRecipe( int recipeIdx ) {
			int removeAt = -1;

			for( int i=0; i<Main.numAvailableRecipes; i++ ) {
				if( removeAt == -1 ) {
					if( Main.availableRecipe[i] != recipeIdx ) {
						continue;
					}
					removeAt = i;
				}

				Main.availableRecipeY[i] -= 65;
				Main.availableRecipe[i] = Main.availableRecipe[i + 1];
			}

			if( removeAt != -1 ) {
				Main.numAvailableRecipes--;
			}
			return removeAt != -1;
		}



		////////////////

		private IDictionary<string, Func<Player, IEnumerable<Item>>> IngredientOutsources = new Dictionary<string, Func<Player, IEnumerable<Item>>>();
		private int LastAvailableRecipeCount = -1;
		private int RefreshTimer = 300;
		private int OldFocusRecipe = -1;



		////////////////

		internal RecipeHack() {
			LoadHooks.AddPostModLoadHook( () => {
				for( int i = 0; i < Recipe.maxRecipes; i++ ) {
					if( Main.recipe[i].createItem.type == ItemID.None ) { break; }

					RecipeHack.BindRecipeItem( i );
				}
			} );
		}


		////////////////

		internal void Update() {
			if( this.IngredientOutsources.Count == 0 ) { return; }

			if( this.LastAvailableRecipeCount != Main.numAvailableRecipes || this.RefreshTimer-- < 0 ) {
				try {
					this.UpdateRecipes();
				} catch( Exception e ) {
					throw new ModHelpersException( "", e );
				}

				this.LastAvailableRecipeCount = Main.numAvailableRecipes;
				this.RefreshTimer = 300;
			}

			this.OldFocusRecipe = Main.focusRecipe;
		}

		private void UpdateRecipes() {
			IEnumerable<Item> ingredients = RecipeHack.GetOutsourcedItems( Main.LocalPlayer );
			ISet<int> recipeIndexes = RecipeHack.GetAvailableRecipesOfIngredients( Main.LocalPlayer, ingredients );
				
			for( int i=0; i<Main.numAvailableRecipes; i++ ) {
				// Force-remove recipes that aren't within the current set
				if( !recipeIndexes.Contains(Main.availableRecipe[i]) ) {
					while ( RecipeHack.ForceRemoveRecipe( Main.availableRecipe[i] ) ) {
					}
				} else {
					recipeIndexes.Remove( Main.availableRecipe[i] );
				}
			}

			foreach( int idx in recipeIndexes ) {
				RecipeHack.ForceAddRecipe( idx );
			}

			if( this.OldFocusRecipe >= 0 ) {
				int toIdx = Math.Min( this.OldFocusRecipe, Main.numAvailableRecipes );
				int shift = (Main.focusRecipe - toIdx) * 65;

				for( int i=0; i<Main.numAvailableRecipes; i++ ) {
					Main.availableRecipeY[i] += shift;
				}

				Main.focusRecipe = toIdx;
			}
		}
	}
}
