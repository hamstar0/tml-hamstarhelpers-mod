using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		//"Any Ranger Misc",
		//"Any Magic Misc",
	}




	partial class EntityGroupDefs {
		internal static void DefineItemWeaponGroups2( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			// Misc Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Ranger Misc",
				grpDeps: new string[] { "Any Ranger Gun", "Any Ranger Bow", "Any Ranger Launcher" },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.ranged ) { return false; }
					bool ranger = grps["Any Ranger Gun"].Contains( item.type );
					bool bow = grps["Any Ranger Bow"].Contains( item.type );
					bool launcher = grps["Any Ranger Launcher"].Contains( item.type );
					return !ranger && !bow && !launcher;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Magic Misc",
				grpDeps: new string[] {
					"Any Magic Staff Or Scepter Or Wand",
					"Any Magic Rod",
					"Any Magic Gun",
					"Any Magic Tome" },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }
					bool staff = grps["Any Magic Staff Or Scepter Or Wand"].Contains( item.type );
					bool rod = grps["Any Magic Rod"].Contains( item.type );
					bool gun = grps["Any Magic Gun"].Contains( item.type );
					bool tome = grps["Any Magic Tome"].Contains( item.type );
					return !staff && !rod && !gun && !tome;
				} )
			) );
		}
	}
}
