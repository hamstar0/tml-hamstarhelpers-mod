using HamstarHelpers.ItemHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public class ItemGroupDefinition : EntityGroupDefinition<Item> {
		public static IList<Item> GetItemGroup( string query_json ) {
			var def = JsonConvert.DeserializeObject<ItemGroupDefinition>( query_json );
			return def.GetGroup();
		}

		
		////////////////

		public ISet<string> UsesItemIngredients { get; private set; }

		private readonly Item[] MyPool = null;

		public override Item[] GetPool() {
			if( this.MyPool == null ) {
				for( int i = 0; i < Main.itemTexture.Length; i++ ) {
					this.MyPool[i] = new Item();
					this.MyPool[i].SetDefaults( i, true );
				}
			}
			return this.MyPool;
		}


		public override IList<Item> GetGroup() {
			var items = base.GetGroup();
			Item[] item_arr = items.ToArray();

			for( int i=0; i<item_arr.Length; i++ ) {
				Item item = item_arr[i];
				bool missing_recipe = true;

				foreach( Recipe recipe in Main.recipe ) {
					if( recipe.createItem.type != item.type ) { continue; }

					bool missing_ingredients = true;

					foreach( string ing_item_name in this.UsesItemIngredients ) {
						bool ingredient_found = false;

						foreach( Item req_item in recipe.requiredItem ) {
							if( ItemIdentityHelpers.GetQualifiedName(req_item) == ing_item_name ) {
								ingredient_found = true;
								break;
							}
						}

						if( !ingredient_found ) {
							missing_ingredients = true;
							break;
						}
					}

					if( missing_ingredients ) {
						items.RemoveAt( i );
					}

					missing_recipe = false;
					break;
				}

				if( missing_recipe ) {
					items.RemoveAt( i );
				}
			}

			return items;
		}
	}
}
