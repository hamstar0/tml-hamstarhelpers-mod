using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.RecipeHelpers {
	public partial class RecipeHelpers {
		private static IDictionary<string, RecipeGroup> CreateRecipeGroups() {
			IDictionary<string, RecipeGroup> groups = null;

			try {
				IEnumerable<FieldInfo> item_grp_fields = typeof( ItemIdentityHelpers ).GetFields( BindingFlags.Static | BindingFlags.Public );
				item_grp_fields = item_grp_fields.Where( field => {
					return field.FieldType == typeof( Tuple<string, int[]> );
				} );

				var tuples = item_grp_fields.ToDictionary(
					field => field.Name,
					field => (Tuple<string, int[]>)field.GetValue( null )
				);

				groups = tuples.ToDictionary( kv => "HamstarHelpers:"+kv.Key,
					kv => {
						string grp_name = kv.Value.Item1;
						int[] item_ids = kv.Value.Item2;
						return new RecipeGroup( () => Lang.misc[37].ToString() + " " + grp_name, item_ids );
					}
				);
			} catch( Exception e ) {
				LogHelpers.Log( "RecipeHelpers.CreateRecipeGroups - " + e.ToString() );
			}
			
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
