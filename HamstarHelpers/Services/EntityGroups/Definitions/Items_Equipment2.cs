using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Helpers.Items.Attributes;


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
				grpName: ItemGroupIDs.AnyEquipment,
				grpDeps: new string[] {
					ItemGroupIDs.AnyWeapon,
					ItemGroupIDs.AnyTool,
					ItemGroupIDs.AnyAccessory,
					ItemGroupIDs.AnyArmor },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return grps[ItemGroupIDs.AnyWeapon].Contains( item.type )
						|| grps[ItemGroupIDs.AnyTool].Contains( item.type )
						|| grps[ItemGroupIDs.AnyAccessory].Contains( item.type )
						|| grps[ItemGroupIDs.AnyArmor].Contains( item.type )
						|| ItemAttributeHelpers.IsGrapple( item );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyHeavyArmor,
				grpDeps: new string[] { ItemGroupIDs.AnyArmor },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return grps[ItemGroupIDs.AnyArmor].Contains( item.type ) &&
						item.defense >= 4;
				} )
			) );
		}
	}
}
