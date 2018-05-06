using HamstarHelpers.ItemHelpers;
using HamstarHelpers.NPCHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.RecipeHelpers {
	public partial class RecipeHelpers {
		[Obsolete( "Use ItemIdentityHelpers.Groups", true )]
		public static IDictionary<string, RecipeGroup> GetRecipeGroups() {
			return RecipeHelpers.Groups;
		}


		private static KeyValuePair<string, RecipeGroup> GetPair( ref KeyValuePair<string, RecipeGroup> source, string name, string desc, int[] item_ids ) {
			if( string.IsNullOrEmpty( source.Key ) ) {
				var rg = new RecipeGroup( () => Lang.misc[37].ToString() + " " + desc, item_ids );
				source = new KeyValuePair<string, RecipeGroup>( "HamstarHelpers:" + name, rg );
			}
			return source;
		}



		////////////////

		private KeyValuePair<string, RecipeGroup> _EvilBossDrops;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"EvilBossDrops\"]", true )]
		public static KeyValuePair<string, RecipeGroup> EvilBossDrops { get {
			int[] item_ids = ItemIdentityHelpers.EvilBiomeBossChunkTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._EvilBossDrops, "EvilBiomeBossDrops", "Evil Biome Boss Chunk", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _EvilLightPet;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"EvilLightPet\"]", true )]
		public static KeyValuePair<string, RecipeGroup> EvilLightPet { get {
			int[] item_ids = ItemIdentityHelpers.EvilBiomeLightPetTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._EvilLightPet, "EvilBiomeLightPet", "Evil Biome Light Pet", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _MagicMirrors;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"MagicMirrors\"]", true )]
		public static KeyValuePair<string, RecipeGroup> MagicMirrors { get {
			int[] item_ids = ItemIdentityHelpers.MagicMirrorTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._MagicMirrors, "MagicMirrors", "Magic Mirrors", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _VanillaAnimals;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"VanillaAnimals\"]", true )]
		public static KeyValuePair<string, RecipeGroup> VanillaAnimals { get {
			int[] item_ids = ItemIdentityHelpers.VanillaAnimalTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._VanillaAnimals, "VanillaAnimals", "Live Animal (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _VanillaBugs;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"VanillaBugs\"]", true )]
		public static KeyValuePair<string, RecipeGroup> VanillaBugs { get {
			int[] item_ids = ItemIdentityHelpers.VanillaBugTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._VanillaBugs, "VanillaBugs", "Live Bug (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _VanillaButterfly;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"VanillaButterfly\"]", true )]
		public static KeyValuePair<string, RecipeGroup> VanillaButterfly { get {
			int[] item_ids = ItemIdentityHelpers.VanillaButterflyTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._VanillaButterfly, "VanillaButterflies", "Butterflies (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _VanillaGoldCritter;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"VanillaGoldCritter\"]", true )]
		public static KeyValuePair<string, RecipeGroup> VanillaGoldCritter { get {
			int[] item_ids = ItemIdentityHelpers.VanillaGoldCritterTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._VanillaGoldCritter, "GoldCritter", "Gold Critters (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _MobBanners;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"MobBanners\"]", true )]
		public static KeyValuePair<string, RecipeGroup> MobBanners { get {
			int[] item_ids = NPCBannerHelpers.GetBannerItemTypes().ToArray();
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._MobBanners, "NpcBanners", "Mob Banner", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _RecordedMusicBox;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"RecordedMusicBox\"]", true )]
		public static KeyValuePair<string, RecipeGroup> RecordedMusicBox { get {
			int[] item_ids = ItemMusicBoxHelpers.GetVanillaMusicBoxes().ToArray();
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._RecordedMusicBox, "RecordedMusicBoxes", "Recorded Music Box (vanilla)", item_ids );
		} }


		 private KeyValuePair<string, RecipeGroup> _AlchemyHerbs;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"AlchemyHerbs\"]", true )]
		public static KeyValuePair<string, RecipeGroup> AlchemyHerbs { get {
			int[] item_ids = ItemIdentityHelpers.AlchemyHerbsTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._AlchemyHerbs, "AlchemyHerbs", "Alchemy Herbs (vanilla)", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _StrangePlants;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"StrangePlants\"]", true )]
		public static KeyValuePair<string, RecipeGroup> StrangePlants { get {
			int[] item_ids = ItemIdentityHelpers.StrangePlantTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._StrangePlants, "StrangePlants", "Strange Plant", item_ids );
		} }

		 private KeyValuePair<string, RecipeGroup> _PressurePlates;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"PressurePlates\"]", true )]
		public static KeyValuePair<string, RecipeGroup> PressurePlates { get {
			int[] item_ids = ItemIdentityHelpers.PressurePlateTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._PressurePlates, "PressurePlates", "Pressure Plates", item_ids );
		} }
		 private KeyValuePair<string, RecipeGroup> _WeightedPressurePlates;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"WeightedPressurePlates\"]", true )]
		public static KeyValuePair<string, RecipeGroup> WeightedPressurePlates { get {
			int[] item_ids = ItemIdentityHelpers.WeightedPressurePlateTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._WeightedPressurePlates, "WeightedPressurePlates", "Weighted Pressure Plates", item_ids );
		} }
		 private KeyValuePair<string, RecipeGroup> _ConveyorBelts;
		[Obsolete( "Use ItemIdentityHelpers.Groups[\"ConveyorBelts\"]", true )]
		public static KeyValuePair<string, RecipeGroup> ConveyorBelts { get {
			int[] item_ids = ItemIdentityHelpers.ConveyorBeltTypes;
			return RecipeHelpers.GetPair( ref HamstarHelpersMod.Instance.RecipeHelpers._ConveyorBelts, "ConveyorBelts", "Conveyor Belts", item_ids );
		} }
	}
}
