﻿using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeIdentityHelpers {
		private static object MyLock = new object();



		////////////////
		
		private IDictionary<int, ISet<int>> RecipeIndexesByItemNetID = new Dictionary<int, ISet<int>>();
		private IDictionary<int, ISet<int>> RecipeIndexesOfIngredientNetIDs = new Dictionary<int, ISet<int>>();



		////////////////

		private void CacheItemRecipes() {
			lock( RecipeIdentityHelpers.MyLock ) {
				for( int i = 0; i < Main.recipe.Length; i++ ) {
					Recipe recipe = Main.recipe[i];
					int recipeItemType = recipe.createItem.type;
					if( recipeItemType == 0 ) {
						break;
					}
					
					this.RecipeIndexesByItemNetID.Append2D( recipeItemType, i );
				}
			}
		}


		private void CacheIngredientRecipes() {
			lock( RecipeIdentityHelpers.MyLock ) {
				for( int i = 0; i < Main.recipe.Length; i++ ) {
					Recipe recipe = Main.recipe[i];
					if( recipe.createItem.type == 0 ) {
						break;
					}

					for( int j=0; j<recipe.requiredItem.Length; j++ ) {
						Item item = recipe.requiredItem[j];
						if( item == null || item.IsAir ) {
							break;
						}

						this.RecipeIndexesOfIngredientNetIDs.Append2D( item.netID, i );
					}
				}
			}
		}
	}
}
