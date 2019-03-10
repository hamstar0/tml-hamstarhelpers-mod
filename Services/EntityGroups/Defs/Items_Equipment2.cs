using System;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups2( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Equipment",
				new string[] { "Any Weapon", "Any Tool", "Any Accessory", "Any Armor" },
				( item, grps ) => {
					return grps["Any Weapon"].Contains( item.type ) ||
						grps["Any Tool"].Contains( item.type ) ||
						grps["Any Accessory"].Contains( item.type ) ||
						grps["Any Armor"].Contains( item.type );
				}
			);
		}
	}
}
