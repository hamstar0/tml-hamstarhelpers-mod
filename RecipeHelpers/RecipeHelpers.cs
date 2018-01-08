using HamstarHelpers.ItemHelpers;
using HamstarHelpers.NPCHelpers;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.RecipeHelpers {
	public class RecipeHelpers {
		public static IDictionary<string, RecipeGroup> GetRecipeGroups() {
			IDictionary<string, RecipeGroup> groups = new Dictionary<string, RecipeGroup>();
			
			groups[RecipeHelpers.EvilBossDrops.Key] = RecipeHelpers.EvilBossDrops.Value;
			groups[RecipeHelpers.EvilLightPet.Key] = RecipeHelpers.EvilLightPet.Value;
			groups[RecipeHelpers.MagicMirrors.Key] = RecipeHelpers.MagicMirrors.Value;

			groups[RecipeHelpers.VanillaAnimals.Key] = RecipeHelpers.VanillaAnimals.Value;
			groups[RecipeHelpers.VanillaBugs.Key] = RecipeHelpers.VanillaBugs.Value;
			groups[RecipeHelpers.VanillaButterfly.Key] = RecipeHelpers.VanillaButterfly.Value;
			groups[RecipeHelpers.VanillaGoldCritter.Key] = RecipeHelpers.VanillaGoldCritter.Value;

			groups[RecipeHelpers.MobBanners.Key] = RecipeHelpers.MobBanners.Value;
			groups[RecipeHelpers.RecordedMusicBox.Key] = RecipeHelpers.RecordedMusicBox.Value;

			groups[RecipeHelpers.AlchemyHerbs.Key] = RecipeHelpers.AlchemyHerbs.Value;
			groups[RecipeHelpers.StrangePlants.Key] = RecipeHelpers.StrangePlants.Value;

			groups[RecipeHelpers.PressurePlates.Key] = RecipeHelpers.PressurePlates.Value;
			groups[RecipeHelpers.WeightedPressurePlates.Key] = RecipeHelpers.WeightedPressurePlates.Value;
			groups[RecipeHelpers.ConveyorBelts.Key] = RecipeHelpers.ConveyorBelts.Value;

			return groups;
		}


		////////////////

		private static KeyValuePair<string, RecipeGroup> GetPair( ref KeyValuePair<string, RecipeGroup> source, string name, string desc, int[] item_ids ) {
			if( string.IsNullOrEmpty( source.Key ) ) {
				var rg = new RecipeGroup( () => Lang.misc[37].ToString() + " " + desc, item_ids );
				source = new KeyValuePair<string, RecipeGroup>( "HamstarHelpers:"+name, rg );
			}
			return source;
		}



		////////////////

		private KeyValuePair<string, RecipeGroup> _EvilBossDrops;
		public static KeyValuePair<string, RecipeGroup> EvilBossDrops { get {
			int[] item_ids = ItemIdentityHelpers.EvilBiomeBossChunkTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._EvilBossDrops, "EvilBiomeBossDrops", "Evil Biome Boss Chunk", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _EvilLightPet;
		public static KeyValuePair<string, RecipeGroup> EvilLightPet { get {
			int[] item_ids = ItemIdentityHelpers.EvilBiomeLightPetTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._EvilLightPet, "EvilBiomeLightPet", "Evil Biome Light Pet", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _MagicMirrors;
		public static KeyValuePair<string, RecipeGroup> MagicMirrors { get {
			int[] item_ids = ItemIdentityHelpers.MagicMirrorTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._MagicMirrors, "MagicMirrors", "Magic Mirrors", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _VanillaAnimals;
		public static KeyValuePair<string, RecipeGroup> VanillaAnimals { get {
			int[] item_ids = ItemIdentityHelpers.VanillaAnimalTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._VanillaAnimals, "VanillaAnimals", "Live Animal (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _VanillaBugs;
		public static KeyValuePair<string, RecipeGroup> VanillaBugs { get {
			int[] item_ids = ItemIdentityHelpers.VanillaBugTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._VanillaBugs, "VanillaBugs", "Live Bug (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _VanillaButterfly;
		public static KeyValuePair<string, RecipeGroup> VanillaButterfly { get {
			int[] item_ids = ItemIdentityHelpers.VanillaButterflyTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._VanillaButterfly, "VanillaButterflies", "Butterflies (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _VanillaGoldCritter;
		public static KeyValuePair<string, RecipeGroup> VanillaGoldCritter { get {
			int[] item_ids = ItemIdentityHelpers.VanillaGoldCritterTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._VanillaGoldCritter, "GoldCritter", "Gold Critters (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _MobBanners;
		public static KeyValuePair<string, RecipeGroup> MobBanners { get {
			int[] item_ids = NPCBannerHelpers.GetBannerItemTypes().ToArray();
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._MobBanners, "NpcBanners", "Mob Banner", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _RecordedMusicBox;
		public static KeyValuePair<string, RecipeGroup> RecordedMusicBox { get {
			int[] item_ids = ItemMusicBoxHelpers.GetVanillaMusicBoxes().ToArray();
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._RecordedMusicBox, "RecordedMusicBoxes", "Recorded Music Box (vanilla)", item_ids );
		} }


		 private KeyValuePair<string, RecipeGroup> _AlchemyHerbs;
		public static KeyValuePair<string, RecipeGroup> AlchemyHerbs { get {
			int[] item_ids = ItemIdentityHelpers.AlchemyHerbsTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._AlchemyHerbs, "AlchemyHerbs", "Alchemy Herbs (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _StrangePlants;
		public static KeyValuePair<string, RecipeGroup> StrangePlants { get {
			int[] item_ids = ItemIdentityHelpers.StrangePlantTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._StrangePlants, "StrangePlants", "Strange Plant", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _PressurePlates;
		public static KeyValuePair<string, RecipeGroup> PressurePlates { get {
			int[] item_ids = ItemIdentityHelpers.PressurePlates;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._PressurePlates, "PressurePlates", "Pressure Plates", item_ids );
		} }
		 private KeyValuePair<string, RecipeGroup> _WeightedPressurePlates;
		public static KeyValuePair<string, RecipeGroup> WeightedPressurePlates { get {
			int[] item_ids = ItemIdentityHelpers.WeightedPressurePlates;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._WeightedPressurePlates, "WeightedPressurePlates", "Weighted Pressure Plates", item_ids );
		} }
		 private KeyValuePair<string, RecipeGroup> _ConveyorBelts;
		public static KeyValuePair<string, RecipeGroup> ConveyorBelts { get {
			int[] item_ids = ItemIdentityHelpers.ConveyorBelts;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._ConveyorBelts, "ConveyorBelts", "Conveyor Belts", item_ids );
		} }
	}
}
