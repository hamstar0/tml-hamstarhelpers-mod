using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeGroupHelpers {
		public static IDictionary<string, RecipeGroup> Groups {
			get {
				var mymod = ModHelpersMod.Instance;

				if( mymod.RecipeGroupHelpers._Groups == null ) {
					mymod.RecipeGroupHelpers._Groups = RecipeGroupHelpers.CreateRecipeGroups();
				}
				return mymod.RecipeGroupHelpers._Groups;
			}
		}



		////////////////

		private static IDictionary<string, RecipeGroup> CreateRecipeGroups() {
			IDictionary<string, Tuple<string, ISet<int>>> commonItemGrps = ItemIdentityHelpers.GetCommonItemGroups();
			IDictionary<string, RecipeGroup> groups = commonItemGrps.ToDictionary( kv => "HamstarHelpers:" + kv.Key,
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
