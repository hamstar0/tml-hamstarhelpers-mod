using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeIdentityHelpers {
		public static bool Equals( Recipe recipe1, Recipe recipe2 ) {
			if( recipe1.needHoney != recipe2.needHoney ) { return false; }
			if( recipe1.needLava != recipe2.needLava ) { return false; }
			if( recipe1.needSnowBiome != recipe2.needSnowBiome ) { return false; }
			if( recipe1.needWater != recipe2.needWater ) { return false; }

			if( recipe1.alchemy != recipe2.alchemy ) { return false; }
			if( recipe1.anyFragment != recipe2.anyFragment ) { return false; }
			if( recipe1.anySand != recipe2.anySand ) { return false; }
			if( recipe1.anyPressurePlate != recipe2.anyPressurePlate ) { return false; }
			if( recipe1.anyIronBar != recipe2.anyIronBar ) { return false; }
			if( recipe1.anyWood != recipe2.anyWood ) { return false; }

			if( recipe1.createItem.IsNotTheSameAs( recipe2.createItem ) ) { return false; }

			var reqTile1 = new HashSet<int>( recipe1.requiredTile );
			var reqTile2 = new HashSet<int>( recipe2.requiredTile );
			if( !reqTile1.SetEquals( reqTile2 ) ) { return false; }

			var reqItem1 = new HashSet<Item>( recipe1.requiredItem );
			var reqItem2 = new HashSet<Item>( recipe2.requiredItem );
			if( !reqItem1.SetEquals( reqItem2 ) ) { return false; }

			var reqAcceptedGrps1 = new HashSet<int>( recipe1.acceptedGroups );
			var reqAcceptedGrps2 = new HashSet<int>( recipe2.acceptedGroups );
			if( !reqAcceptedGrps1.SetEquals( reqAcceptedGrps2 ) ) { return false; }

			return true;
		}


		////////////////

		public static ISet<int> GetRecipeIndexesOfItem( int itemNetID ) {
			if( itemNetID == 0 ) {
				throw new HamstarException( "Invalid item type" );
			}

			var mymod = ModHelpersMod.Instance;
			IDictionary<int, ISet<int>> recipeIdxLists = mymod.RecipeIdentityHelpers.RecipeIndexesByItemNetID;
			
			lock( RecipeIdentityHelpers.MyLock ) {
				if( recipeIdxLists.Count == 0 ) {
					mymod.RecipeIdentityHelpers.CacheItemRecipes();
				}
				return recipeIdxLists.GetOrDefault( itemNetID )
					?? new HashSet<int>();
			}
		}
		
		public static IList<Recipe> GetRecipesOfItem( int itemNetID ) {
			return RecipeIdentityHelpers.GetRecipeIndexesOfItem( itemNetID )
				.Select( idx=>Main.recipe[idx] )
				.ToList();
		}


		////////////////

		public static ISet<int> GetRecipeIndexesUsingIngredient( int itemNetID ) {
			if( itemNetID == 0 ) {
				throw new HamstarException( "Invalid item type" );
			}

			var mymod = ModHelpersMod.Instance;
			IDictionary<int, ISet<int>> recipeIdxSets = mymod.RecipeIdentityHelpers.RecipeIndexesOfIngredientNetIDs;

			lock( RecipeIdentityHelpers.MyLock ) {
				if( recipeIdxSets.Count == 0 ) {
					mymod.RecipeIdentityHelpers.CacheIngredientRecipes();
				}
				return recipeIdxSets.GetOrDefault( itemNetID )
					?? new HashSet<int>();
			}
		}
	}
}
