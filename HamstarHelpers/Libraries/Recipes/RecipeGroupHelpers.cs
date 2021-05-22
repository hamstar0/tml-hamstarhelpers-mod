using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.Items;


namespace HamstarHelpers.Libraries.Recipes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to common recipe groups.
	/// </summary>
	public partial class RecipeGroupLibraries {
		/// <summary>
		/// Recipe group of items of an "evil" biome boss's drops (Shadow Scale and Tissue Sample).
		/// </summary>
		public static RecipeGroup EvilBiomeBossDrops => RecipeGroupLibraries.Groups["ModHelpers:EvilBiomeBossDrops"];
		/// <summary>
		/// Recipe group of light pet items of an "evil" biome (Shadow Orb and Crimson Heart).
		/// </summary>
		public static RecipeGroup EvilBiomeLightPet => RecipeGroupLibraries.Groups["ModHelpers:EvilBossDropsEvilBiomeLightPet"];
		/// <summary></summary>
		public static RecipeGroup VanillaButterfly => RecipeGroupLibraries.Groups["ModHelpers:EvilBossDropsVanillaButterfly"];
		/// <summary></summary>
		public static RecipeGroup VanillaGoldCritter => RecipeGroupLibraries.Groups["ModHelpers:EvilBossDropsVanillaGoldCritter"];
		/// <summary></summary>
		public static RecipeGroup PressurePlates => RecipeGroupLibraries.Groups["ModHelpers:EvilBossDropsPressurePlates"];
		/// <summary></summary>
		public static RecipeGroup WeightedPressurePlates => RecipeGroupLibraries.Groups["ModHelpers:EvilBossDropsWeightedPressurePlates"];
		/// <summary></summary>
		public static RecipeGroup ConveyorBelts => RecipeGroupLibraries.Groups["ModHelpers:EvilBossDropsConveyorBelts"];
		/// <summary></summary>
		public static RecipeGroup NpcBanners => RecipeGroupLibraries.Groups["ModHelpers:EvilBossDropsNpcBanners"];
		/// <summary></summary>
		public static RecipeGroup RecordedMusicBoxes => RecipeGroupLibraries.Groups["ModHelpers:EvilBossDropsRecordedMusicBoxes"];


		////

		/// <summary>
		/// A map of common recipe groups to their internal names.
		/// </summary>
		public static IDictionary<string, RecipeGroup> Groups {
			get {
				var mymod = ModHelpersMod.Instance;

				if( mymod.RecipeGroupHelpers._Groups == null ) {
					mymod.RecipeGroupHelpers._Groups = RecipeGroupLibraries.CreateRecipeGroups();
				}
				return mymod.RecipeGroupHelpers._Groups;
			}
		}



		////////////////

		private static IDictionary<string, RecipeGroup> CreateRecipeGroups() {
			IDictionary<string, ItemGroupDefinition> commonItemGrps = ItemGroupIdentityLibraries.GetCommonItemGroups();

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
