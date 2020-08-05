using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Recipes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to common recipe groups.
	/// </summary>
	public partial class RecipeGroupHelpers {
		/// <summary>
		/// Recipe group of items of an "evil" biome boss's drops (Shadow Scale and Tissue Sample).
		/// </summary>
		public static RecipeGroup EvilBiomeBossDrops => RecipeGroupHelpers.Groups["ModHelpers:EvilBiomeBossDrops"];
		/// <summary>
		/// Recipe group of light pet items of an "evil" biome (Shadow Orb and Crimson Heart).
		/// </summary>
		public static RecipeGroup EvilBiomeLightPet => RecipeGroupHelpers.Groups["ModHelpers:EvilBossDropsEvilBiomeLightPet"];
		/// <summary></summary>
		public static RecipeGroup VanillaButterfly => RecipeGroupHelpers.Groups["ModHelpers:EvilBossDropsVanillaButterfly"];
		/// <summary></summary>
		public static RecipeGroup VanillaGoldCritter => RecipeGroupHelpers.Groups["ModHelpers:EvilBossDropsVanillaGoldCritter"];
		/// <summary></summary>
		public static RecipeGroup PressurePlates => RecipeGroupHelpers.Groups["ModHelpers:EvilBossDropsPressurePlates"];
		/// <summary></summary>
		public static RecipeGroup WeightedPressurePlates => RecipeGroupHelpers.Groups["ModHelpers:EvilBossDropsWeightedPressurePlates"];
		/// <summary></summary>
		public static RecipeGroup ConveyorBelts => RecipeGroupHelpers.Groups["ModHelpers:EvilBossDropsConveyorBelts"];
		/// <summary></summary>
		public static RecipeGroup NpcBanners => RecipeGroupHelpers.Groups["ModHelpers:EvilBossDropsNpcBanners"];
		/// <summary></summary>
		public static RecipeGroup RecordedMusicBoxes => RecipeGroupHelpers.Groups["ModHelpers:EvilBossDropsRecordedMusicBoxes"];


		////

		/// <summary>
		/// A map of common recipe groups to their internal names.
		/// </summary>
		public static IDictionary<string, RecipeGroup> Groups {
			get {
				var mymod = ModHelpersMod.Instance;

				if( mymod.RecipeGroupHelpers._Groups == null ) {
					mymod.RecipeGroupHelpers._Groups = RecipeGroupHelpers.CreateRecipeGroups();
				}
				return mymod.RecipeGroupHelpers._Groups;
			}
		}



		////////////////

		private static IDictionary<string, RecipeGroup> CreateRecipeGroups() {
			IDictionary<string, ItemGroupDefinition> commonItemGrps = ItemGroupIdentityHelpers.GetCommonItemGroups();

			IDictionary<string, RecipeGroup> groups = commonItemGrps.ToDictionary(
				kv => {
					string internalGrpName = kv.Key;
					return "ModHelpers:" + internalGrpName;
				},
				kv => {
					string grpName = kv.Value.GroupName;
					ISet<int> itemIds = kv.Value.Group;

					return new RecipeGroup(
						() => Lang.misc[37].ToString() + " " + grpName,
						itemIds.ToArray()
					);
				}
			);

			return groups;
		}
	}
}
