using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeHelpers {
		public static bool CanRecipeBeCrafted( Player player, Recipe recipe ) {

		}


		////////////////

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
