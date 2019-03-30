using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
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
				out int[] missingTile, out int[] missingItem, IEnumerable<Item> availableIngredients = null
			) {
			RecipeCraftFailReason reason = 0;
			var missingTileList = new List<int>();
			var missingItemList = new List<int>();

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
			IDictionary<int, int> availItemInfo = new Dictionary<int, int>( availableIngredients.Count() );
			foreach( Item item in availableIngredients ) {
				if( availItemInfo.ContainsKey( item.netID) ) {
					availItemInfo[ item.netID ] += item.stack;
				} else {
					availItemInfo[ item.netID ] = item.stack;
				}
			}
			//availItems.ToDictionary( kv => kv.netID, kv => kv.stack );

			// Tiles
			for( int i=0; i < Recipe.maxRequirements; i++ ) {
				int reqTileType = recipe.requiredTile[i];
				if( reqTileType == -1 ) { break; }

				if( !player.adjTile[ reqTileType ] ) {
					missingTileList.Add( reqTileType );
					reason |= RecipeCraftFailReason.MissingTile;
				}
			}

			// Items
			for( int i = 0; i < Recipe.maxRequirements; i++ ) {
				Item reqItem = recipe.requiredItem[i];
				if( reqItem.type == 0 ) { break; }

				int reqStack = reqItem.stack;
				bool hasCheckedGroups = false;

				foreach( var kv in availItemInfo ) {
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
				if( !hasCheckedGroups && availItemInfo.ContainsKey(reqItem.netID) ) {
					reqStack -= availItemInfo[ reqItem.netID ];
				}

				// Account for missing ingredients:
				if( reqStack > 0 ) {
					missingItemList.Add( reqItem.netID );
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
			missingItem = missingItemList.ToArray();
			return reason;
		}


		////////////////

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

			if( !recipe1.createItem.IsNotTheSameAs(recipe2.createItem) ) { return false; }

			var reqTile1 = new HashSet<int>( recipe1.requiredTile );
			var reqTile2 = new HashSet<int>( recipe2.requiredTile );
			if( !reqTile1.Equals(reqTile2) ) { return false; }

			var reqItem1 = new HashSet<Item>( recipe1.requiredItem );
			var reqItem2 = new HashSet<Item>( recipe2.requiredItem );
			if( !reqItem1.Equals( reqItem2 ) ) { return false; }

			var reqAcceptedGrps1 = new HashSet<int>( recipe1.acceptedGroups );
			var reqAcceptedGrps2 = new HashSet<int>( recipe2.acceptedGroups );
			if( !reqAcceptedGrps1.Equals( reqAcceptedGrps2 ) ) { return false; }

			return true;
		}


		public static IList<Recipe> GetRecipesOfItem( int itemType ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.RecipeHelpers.RecipesByItem.Count > 0 ) {
				if( mymod.RecipeHelpers.RecipesByItem.ContainsKey(itemType) ) {
					return mymod.RecipeHelpers.RecipesByItem[ itemType ];
				}
				return new List<Recipe>();
			}

			for( int i = 0; i < Main.recipe.Length; i++ ) {
				Recipe recipe = Main.recipe[i];
				int recipeItemType = recipe.createItem.type;

				if( !mymod.RecipeHelpers.RecipesByItem.ContainsKey(recipeItemType) ) {
					mymod.RecipeHelpers.RecipesByItem[ recipeItemType ] = new List<Recipe>();
				}
				mymod.RecipeHelpers.RecipesByItem[ recipeItemType ].Add( recipe );
			}

			return RecipeHelpers.GetRecipesOfItem( itemType );
		}


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

		[Obsolete( "use ItemHasIngredients(int, ISet<int>, int)" )]
		public static bool ItemHasIngredients( Item item, ISet<int> ingredients, int minStack ) {
			return RecipeHelpers.ItemHasIngredients( item.type, ingredients, minStack );
		}


		////////////////

		private static IDictionary<string, RecipeGroup> CreateRecipeGroups() {
			IDictionary<string, Tuple<string, ISet<int>>> dict = ItemIdentityHelpers.GetCommonItemGroups();
			IDictionary<string, RecipeGroup> groups = dict.ToDictionary( kv => "HamstarHelpers:"+kv.Key,
				kv => {
					string grpName = kv.Value.Item1;
					ISet<int> itemIds = kv.Value.Item2;
					return new RecipeGroup( () => Lang.misc[37].ToString() + " " + grpName, itemIds.ToArray() );
				}
			);
			
			return groups;
		}
	}
}
