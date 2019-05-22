using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.RecipeHelpers {
	public partial class RecipeGroupHelpers {
		public static RecipeGroup EvilBossDrops => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDrops"];
		public static RecipeGroup EvilLightPet => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsEvilLightPet"];
		public static RecipeGroup EvilBiomeBossDrops => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsEvilBiomeBossDrops"];
		public static RecipeGroup EvilBiomeLightPet => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsEvilBiomeLightPet"];
		public static RecipeGroup VanillaButterfly => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsVanillaButterfly"];
		public static RecipeGroup VanillaGoldCritter => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsVanillaGoldCritter"];
		public static RecipeGroup PressurePlates => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsPressurePlates"];
		public static RecipeGroup WeightedPressurePlates => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsWeightedPressurePlates"];
		public static RecipeGroup ConveyorBelts => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsConveyorBelts"];
		public static RecipeGroup NpcBanners => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsNpcBanners"];
		public static RecipeGroup RecordedMusicBoxes => RecipeGroupHelpers.Groups["HamstarHelpers:EvilBossDropsRecordedMusicBoxes"];


		////

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
			IDictionary<string, Tuple<string, ISet<int>>> commonItemGrps = ItemIdentityHelpers.GetCommonItemGroups();
			IDictionary<string, RecipeGroup> groups = commonItemGrps.ToDictionary(
				kv => "HamstarHelpers:" + kv.Key,
				kv => {
					string grpName = kv.Value.Item1;
					ISet<int> itemIds = kv.Value.Item2;

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
