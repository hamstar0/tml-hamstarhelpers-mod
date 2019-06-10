using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Recipes {
	[Flags]
	public enum RecipeCraftFailReason {
		NeedsNearbyWater=1,
		NeedsNearbyHoney=2,
		NeedsNearbyLava=4,
		NeedsNearbySnowBiome=8,
		MissingTile=16,
		MissingItem=32
	}




	public partial class RecipeHelpers {
		public static RecipeCraftFailReason GetRecipeFailReasons( Player player, Recipe recipe,
				out int[] missingTile, out IDictionary<int, int> missingItemTypesStacks, IEnumerable<Item> availableIngredients = null
			) {
			RecipeCraftFailReason reason = 0;
			var missingTileList = new List<int>();
			missingItemTypesStacks = new Dictionary<int, int>();

			// Get available item ingredients
			if( availableIngredients == null ) {
				availableIngredients = player.inventory
					.Take( 58 )
					.Where( item => !item.IsAir );

				bool? _;
				Item[] chest = PlayerItemHelpers.GetCurrentlyOpenChest( player, out _ );
				if( chest != null ) {
					availableIngredients = availableIngredients.Concat( chest );
				}
			}

			// Process ingredients list into id + stack map
			IDictionary<int, int> availIngredientInfo = new Dictionary<int, int>( availableIngredients.Count() );
			foreach( Item item in availableIngredients ) {
				if( availIngredientInfo.ContainsKey( item.netID) ) {
					availIngredientInfo[ item.netID ] += item.stack;
				} else {
					availIngredientInfo[ item.netID ] = item.stack;
				}
			}

			// Tiles
			for( int i=0; i < recipe.requiredTile.Length; i++ ) {
				int reqTileType = recipe.requiredTile[i];
				if( reqTileType == -1 ) { break; }

				if( !player.adjTile[ reqTileType ] ) {
					missingTileList.Add( reqTileType );
					reason |= RecipeCraftFailReason.MissingTile;
				}
			}

			// Items
			for( int i = 0; i < recipe.requiredItem.Length; i++ ) {
				Item reqItem = recipe.requiredItem[i];
				if( reqItem == null || reqItem.type == 0 ) { break; }

				int reqStack = reqItem.stack;
				bool hasCheckedGroups = false;

				foreach( var kv in availIngredientInfo ) {
					int itemType = kv.Key;
					int itemStack = kv.Value;

					if( recipe.useWood( itemType, reqItem.type )
							|| recipe.useSand( itemType, reqItem.type )
							|| recipe.useIronBar( itemType, reqItem.type )
							|| recipe.useFragment( itemType, reqItem.type )
							|| recipe.usePressurePlate( itemType, reqItem.type )
							|| recipe.AcceptedByItemGroups( itemType, reqItem.type ) ) {
						reqStack -= itemStack;
						hasCheckedGroups = true;
					}
				}
				if( !hasCheckedGroups && availIngredientInfo.ContainsKey(reqItem.netID) ) {
					reqStack -= availIngredientInfo[ reqItem.netID ];
				}

				// Account for missing ingredients:
				if( reqStack > 0 ) {
					missingItemTypesStacks[ reqItem.netID ] = reqStack;
					reason |= RecipeCraftFailReason.MissingItem;
				}
			}
			
			if( recipe.needWater && !player.adjWater && !player.adjTile[172] ) {
				reason |= RecipeCraftFailReason.NeedsNearbyWater;
			}
			if( recipe.needHoney && !player.adjHoney ) {
				reason |= RecipeCraftFailReason.NeedsNearbyHoney;
			}
			if( recipe.needLava && !player.adjLava ) {
				reason |= RecipeCraftFailReason.NeedsNearbyLava;
			}
			if( recipe.needSnowBiome && !player.ZoneSnow ) {
				reason |= RecipeCraftFailReason.NeedsNearbySnowBiome;
			}

			missingTile = missingTileList.ToArray();
			return reason;
		}
		
		////////////////

		public static IList<int> GetAvailableRecipesOfIngredients( Player player, IEnumerable<Item> ingredients ) {
			int[] _;
			IDictionary<int, int> __;
			IList<int> addedRecipeIndexes = new List<int>();
			ISet<int> possibleRecipeIdxs = new HashSet<int>();

			foreach( Item ingredient in ingredients ) {
				IEnumerable<int> ingredientRecipeIdxs = RecipeIdentityHelpers.GetRecipeIndexesOfItem( ingredient.netID );

				foreach( int recipeIdx in ingredientRecipeIdxs ) {
					possibleRecipeIdxs.Add( recipeIdx );
				}
			}

			foreach( int recipeIdx in possibleRecipeIdxs ) {
				Recipe recipe = Main.recipe[recipeIdx];
				if( recipe.createItem.type == 0 ) { continue; } // Just in case?

				if( RecipeHelpers.GetRecipeFailReasons( player, recipe, out _, out __, ingredients ) == 0 ) {
					addedRecipeIndexes.Add( recipeIdx );
				}
			}

			return addedRecipeIndexes;
		}


		////////////////

		public static bool ItemHasIngredients( int itemType, ISet<int> ingredients, int minStack ) {
			for( int i = 0; i < Main.recipe.Length; i++ ) {
				Recipe recipe = Main.recipe[i];
				if( recipe.createItem.type != itemType ) { continue; }

				for( int j = 0; j < recipe.requiredItem.Length; j++ ) {
					Item reqitem = recipe.requiredItem[j];
					if( reqitem.stack < minStack ) { continue; }
					if( ingredients.Contains( reqitem.type ) ) {
						return true;
					}
				}
			}
			return false;
		}
	}
}
