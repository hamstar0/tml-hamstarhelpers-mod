using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.RecipeHelpers {
	public partial class RecipeHelpers {
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
				var mymod = HamstarHelpersMod.Instance;

				if( mymod.RecipeHelpers._Groups == null ) {
					mymod.RecipeHelpers._Groups = RecipeHelpers.CreateRecipeGroups();
				}
				return mymod.RecipeHelpers._Groups;
			}
		}


		private IDictionary<string, RecipeGroup> _Groups = null;
	}
}
