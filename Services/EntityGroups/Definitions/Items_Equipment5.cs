using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyOreEquipment = "Any Ore Equipment";
		/// <summary></summary>
		public const string AnyNonOreCraftedEquipment = "Any Non Ore Crafted Equipment";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups5( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyOreEquipment,
				grpDeps: new string[] {
					ItemGroupIDs.AnyCopperOrTinEquipment,
					ItemGroupIDs.AnyIronOrLeadEquipment,
					ItemGroupIDs.AnySilverOrTungstenEquipment,
					ItemGroupIDs.AnyGoldOrPlatinumEquipment,
					ItemGroupIDs.AnyMeteorEquipment,
					ItemGroupIDs.AnyDemoniteOrCrimtaneEquipment,
					ItemGroupIDs.AnyHellstoneEquipment,
					ItemGroupIDs.AnyCobaltOrPalladiumEquipment,
					ItemGroupIDs.AnyMythrilOrOrichalcumEquipment,
					ItemGroupIDs.AnyAdamantiteOrTitaniumEquipment,
					ItemGroupIDs.AnyHallowEquipment,
					ItemGroupIDs.AnyFrostCoreEquipment,
					ItemGroupIDs.AnyShellEquipment,
					ItemGroupIDs.AnyChlorophyteOrShroomiteOrSpectreEquipment,
					ItemGroupIDs.AnyLuminiteOreEquipment
				},
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return grps[ItemGroupIDs.AnyCopperOrTinEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyIronOrLeadEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnySilverOrTungstenEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyGoldOrPlatinumEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyMeteorEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyDemoniteOrCrimtaneEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyHellstoneEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyCobaltOrPalladiumEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyMythrilOrOrichalcumEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyAdamantiteOrTitaniumEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyHallowEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyFrostCoreEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyShellEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyChlorophyteOrShroomiteOrSpectreEquipment].Contains( item.type ) ||
						grps[ItemGroupIDs.AnyLuminiteOreEquipment].Contains( item.type );
				} )
			) );
			
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyNonOreCraftedEquipment,
				grpDeps: new string[] {
					ItemGroupIDs.AnyCactusEquipment,
					ItemGroupIDs.AnyWoodEquipment,
					ItemGroupIDs.AnyJungleEquipment,
					ItemGroupIDs.AnyBeeEquipment,
					ItemGroupIDs.AnyBoneEquipment,
					ItemGroupIDs.AnyMeteorEquipment,
					ItemGroupIDs.AnyCelestialEquipment
				},
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return grps[ItemGroupIDs.AnyCactusEquipment].Contains( item.type )
						|| grps[ItemGroupIDs.AnyWoodEquipment].Contains( item.type )
						|| grps[ItemGroupIDs.AnyJungleEquipment].Contains( item.type )
						|| grps[ItemGroupIDs.AnyBeeEquipment].Contains( item.type )
						|| grps[ItemGroupIDs.AnyBoneEquipment].Contains( item.type )
						|| grps[ItemGroupIDs.AnyMeteorEquipment].Contains( item.type )
						|| grps[ItemGroupIDs.AnyCelestialEquipment].Contains( item.type )
						|| ItemAttributeHelpers.IsGrapple(item);
				} )
			) );
		}
	}
}
