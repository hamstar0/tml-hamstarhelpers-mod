using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.RecipeHelpers {
	public partial class RecipeHelpers {
		private static IDictionary<string, RecipeGroup> CreateRecipeGroups() {
			IDictionary<string, Tuple<string, int[]>> tuples = ItemIdentityHelpers.GetCommonItemGroups();
			IDictionary<string, RecipeGroup> groups = tuples.ToDictionary( kv => "HamstarHelpers:"+kv.Key,
				kv => {
					string grp_name = kv.Value.Item1;
					int[] item_ids = kv.Value.Item2;
					return new RecipeGroup( () => Lang.misc[37].ToString() + " " + grp_name, item_ids );
				}
			);
			
			return groups;
		}



		////////////////

		private IDictionary<string, RecipeGroup> _Groups = null;
		public static IDictionary<string, RecipeGroup> Groups {
			get {
				var mymod = HamstarHelpersMod.Instance;
				if( mymod.RecipeHelpers._Groups == null ) {
					mymod.RecipeHelpers._Groups = RecipeHelpers.CreateRecipeGroups();
				}
				return mymod.RecipeHelpers._Groups;
			}
		}
	}
}
