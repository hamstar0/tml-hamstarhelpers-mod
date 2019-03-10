using HamstarHelpers.Helpers.DebugHelpers;
using System;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	public partial class EntityGroupDefs {
		internal static void DefineItemWeaponGroups2( Action<string, string[], Matcher> addDef ) {
			// Misc Sub Classes

			addDef( "Any Ranger Misc",
				new string[] { "Any Ranger Gun", "Any Ranger Bow", "Any Ranger Launcher" },
				( item, grps ) => {
					if( !item.ranged ) { return false; }
					bool ranger = grps["Any Ranger Gun"].Contains( item.type );
					bool bow = grps["Any Ranger Bow"].Contains( item.type );
					bool launcher = grps["Any Ranger Launcher"].Contains( item.type );
					return !ranger && !bow && !launcher;
				}
			);

			addDef( "Any Magic Misc",
				new string[] { "Any Magic Staff Or Scepter Or Wand", "Any Magic Rod", "Any Magic Gun", "Any Magic Tome" },
				( item, grps ) => {
					if( !item.magic ) { return false; }
					bool staff = grps["Any Magic Staff Or Scepter Or Wand"].Contains( item.type );
					bool rod = grps["Any Magic Rod"].Contains( item.type );
					bool gun = grps["Any Magic Gun"].Contains( item.type );
					bool tome = grps["Any Magic Tome"].Contains( item.type );
					return !staff && !rod && !gun && !tome;
				}
			);
		}
	}
}
