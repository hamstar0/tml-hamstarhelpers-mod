using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyEquipment = "Any Equipment";
		/// <summary></summary>
		public const string AnyHeavyArmor = "Any Heavy Armor";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups2( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyEquipment,
				new string[] { ItemGroupIDs.AnyWeapon, ItemGroupIDs.AnyTool, ItemGroupIDs.AnyAccessory, ItemGroupIDs.AnyArmor },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps[ItemGroupIDs.AnyWeapon].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyTool].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyAccessory].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyArmor].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyHeavyArmor,
				new string[] { ItemGroupIDs.AnyArmor },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps[ItemGroupIDs.AnyArmor].Contains( item.type ) &&
						item.defense >= 4;
				} )
			) );
		}
	}
}
