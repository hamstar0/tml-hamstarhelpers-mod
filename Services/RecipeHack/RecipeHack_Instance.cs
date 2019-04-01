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
			var addedRecipeIndexes = new List<int>();
			int len = Main.recipe.Length;

			try {
				for( int i = 0; i < len; i++ ) {
					if( Main.recipe[i].createItem.type == 0 ) { break; }

					//RecipeHack.BindRecipeItem( i );	// <- Paranoia measure; costs CPU, though

					if( this.CheckRecipeAgainstIngredientSources(i) ) {
						addedRecipeIndexes.Add( i );
					}
				}

				ISet<int> availRecipeIdxSet = new HashSet<int>( Main.availableRecipe.Take(Main.numAvailableRecipes) );

				foreach( int idx in addedRecipeIndexes ) {
					if( !availRecipeIdxSet.Contains( idx ) ) {
						RecipeHack.ForceAddRecipe( idx );
//LogHelpers.Log( " Added recipe "+Main.recipe[idx].createItem.Name+"{"+idx+"}" );
					}
//else {
//LogHelpers.Log( " Redundant recipe "+Main.recipe[idx].createItem.Name+"{"+idx+"}" );
//}
				}
//if( availRecipeIdxSet.Count > 0 ) {
//LogHelpers.Log("current recipes ("+Main.numAvailableRecipes+":"+Recipe.maxRecipes+"): "+string.Join(", ", availRecipeIdxSet.Select(idx=>Main.recipe[idx].createItem.Name+"{"+idx+"}")) );
//LogHelpers.Log("adding recipes: "+string.Join(", ",addedRecipeIndexes.Select(idx => Main.recipe[idx].createItem.Name+"{"+idx+"}")) );
//LogHelpers.Log("now recipes: "+string.Join(", ",Main.availableRecipe.Take(Main.numAvailableRecipes).Select(idx=>Main.recipe[idx].createItem.Name+"{"+idx+"}")) );
//LogHelpers.Log(" ");
//}

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


		////////////////

		private bool CheckRecipeAgainstIngredientSources( int recipeIdx ) {
			int[] _;
			IDictionary<int, int> __;

			foreach( var src in this.IngredientOutsources.Values ) {
				Recipe recipe = Main.recipe[ recipeIdx ];
				if( recipe.createItem.type == 0 ) { continue; }	//break?

				IEnumerable<Item> ingredients = src( Main.LocalPlayer );

				RecipeCraftFailReason reason = RecipeHelpers.GetRecipeFailReasons( Main.LocalPlayer, recipe, out _, out __, ingredients );
//if( recipe.createItem.type == ItemID.Torch ) {
//DebugHelpers.Print( "EIEIO", "reason:"+Enum.GetName(typeof(RecipeCraftFailReason), reason)+" ("+(int)reason+") "+string.Join(",",_.Select(idx=>ItemIdentityHelpers.GetQualifiedName(idx)+"@"+idx ) )
//	+ " in "+string.Join(",",ingredients.Select(item=>item.Name+"@"+item.type)), 20 );
//}
				if( reason == 0 ) {
					return true;
				}
			}

			return false;
		}
	}
}
