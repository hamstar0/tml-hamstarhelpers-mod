using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class EntityGroupIDs {
	}




	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups2( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Equipment",
				new string[] { "Any Weapon", "Any Tool", "Any Accessory", "Any Armor" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Weapon"].Contains( item.type ) ||
						grps["Any Tool"].Contains( item.type ) ||
						grps["Any Accessory"].Contains( item.type ) ||
						grps["Any Armor"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Heavy Armor",
				new string[] { "Any Armor" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Armor"].Contains( item.type ) &&
						item.defense >= 4;
				} )
			) );
		}
	}
}
