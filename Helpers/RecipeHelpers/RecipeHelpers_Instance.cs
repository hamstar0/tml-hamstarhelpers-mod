using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeHelpers {
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


		private IDictionary<int, IList<Recipe>> RecipesByItem = new Dictionary<int, IList<Recipe>>();
	}
}
