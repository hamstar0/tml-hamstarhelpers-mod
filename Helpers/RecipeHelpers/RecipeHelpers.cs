using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeHelpers {
		public static bool ItemHasIngredients( int item_type, ISet<int> ingredients, int min_stack ) {
			for( int i = 0; i < Main.recipe.Length; i++ ) {
				Recipe recipe = Main.recipe[i];
				if( recipe.createItem.type != item_type ) { continue; }

				for( int j = 0; j < recipe.requiredItem.Length; j++ ) {
					Item reqitem = recipe.requiredItem[j];
					if( reqitem.stack < min_stack ) { continue; }
					if( ingredients.Contains( reqitem.type ) ) {
						return true;
					}
				}
			}
			return false;
		}

		[Obsolete( "use ItemHasIngredients(int, ISet<int>, int)" )]
		public static bool ItemHasIngredients( Item item, ISet<int> ingredients, int min_stack ) {
			return RecipeHelpers.ItemHasIngredients( item.type, ingredients, min_stack );
		}


		////////////////

		private static IDictionary<string, RecipeGroup> CreateRecipeGroups() {
			IDictionary<string, Tuple<string, ISet<int>>> dict = ItemIdentityHelpers.GetCommonItemGroups();
			IDictionary<string, RecipeGroup> groups = dict.ToDictionary( kv => "HamstarHelpers:"+kv.Key,
				kv => {
					string grp_name = kv.Value.Item1;
					ISet<int> item_ids = kv.Value.Item2;
					return new RecipeGroup( () => Lang.misc[37].ToString() + " " + grp_name, item_ids.ToArray() );
				}
			);
			
			return groups;
		}



		////////////////

		public static IDictionary<string, RecipeGroup> Groups {
			get {
				var mymod = ModHelpersMod.Instance;

				if( mymod.RecipeHelpers._Groups == null ) {
					mymod.RecipeHelpers._Groups = RecipeHelpers.CreateRecipeGroups();
				}
				return mymod.RecipeHelpers._Groups;
			}
		}


		private IDictionary<string, RecipeGroup> _Groups = null;
	}
}
