using HamstarHelpers.ItemHelpers;
using HamstarHelpers.NPCHelpers;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.RecipeHelpers {
	public static class RecipeHelpers {
		private static KeyValuePair<string, RecipeGroup> _EvilBossDrops;
		public static KeyValuePair<string, RecipeGroup> EvilBossDrops { get {
			if( string.IsNullOrEmpty(RecipeHelpers._EvilBossDrops.Key) ) {
				RecipeHelpers._EvilBossDrops = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:EvilBiomeBossDrops",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Evil Biome Boss Chunk",
						ItemIdentityHelpers.EvilBiomeBossChunkTypes )
				);
			}
			return RecipeHelpers._EvilBossDrops;
		} }
		private static KeyValuePair<string, RecipeGroup> _EvilLightPet;
		public static KeyValuePair<string, RecipeGroup> EvilLightPet { get {
			if( string.IsNullOrEmpty( RecipeHelpers._EvilLightPet.Key ) ) {
				RecipeHelpers._EvilLightPet = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:EvilBiomeLightPet",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Evil Biome Light Pet",
						ItemIdentityHelpers.EvilBiomeLightPetTypes )
				);
			}
			return RecipeHelpers._EvilLightPet;
		} }
		private static KeyValuePair<string, RecipeGroup> _MagicMirrors;
		public static KeyValuePair<string, RecipeGroup> MagicMirrors { get {
			if( string.IsNullOrEmpty( RecipeHelpers._MagicMirrors.Key ) ) {
				RecipeHelpers._MagicMirrors = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:MagicMirrors",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Magic Mirrors",
						ItemIdentityHelpers.MagicMirrorTypes )
				);
			}
			return RecipeHelpers._MagicMirrors;
		} }

		private static KeyValuePair<string, RecipeGroup> _VanillaAnimals;
		public static KeyValuePair<string, RecipeGroup> VanillaAnimals { get {
			if( string.IsNullOrEmpty( RecipeHelpers._VanillaAnimals.Key ) ) {
				RecipeHelpers._VanillaAnimals = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:VanillaAnimals",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Live Animal (vanilla)",
						ItemIdentityHelpers.VanillaAnimalTypes )
				);
			}
			return RecipeHelpers._VanillaAnimals;
		} }
		private static KeyValuePair<string, RecipeGroup> _VanillaBugs;
		public static KeyValuePair<string, RecipeGroup> VanillaBugs { get {
			if( string.IsNullOrEmpty( RecipeHelpers._VanillaBugs.Key ) ) {
				RecipeHelpers._VanillaBugs = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:VanillaBugs",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Live Bug (vanilla)",
						ItemIdentityHelpers.VanillaBugTypes )
				);
			}
			return RecipeHelpers._VanillaBugs;
		} }
		private static KeyValuePair<string, RecipeGroup> _VanillaButterfly;
		public static KeyValuePair<string, RecipeGroup> VanillaButterfly { get {
			if( string.IsNullOrEmpty( RecipeHelpers._VanillaButterfly.Key ) ) {
				RecipeHelpers._VanillaButterfly = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:VanillaButterflies",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Butterflies (vanilla)",
						ItemIdentityHelpers.VanillaButterflyTypes )
				);
			}
			return RecipeHelpers._VanillaButterfly;
		} }
		private static KeyValuePair<string, RecipeGroup> _VanillaGoldCritter;
		public static KeyValuePair<string, RecipeGroup> VanillaGoldCritter { get {
			if( string.IsNullOrEmpty( RecipeHelpers._VanillaGoldCritter.Key ) ) {
				RecipeHelpers._VanillaGoldCritter = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:GoldCritter",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Gold Critters (vanilla)",
						ItemIdentityHelpers.VanillaGoldCritterTypes )
				);
			}
			return RecipeHelpers._VanillaGoldCritter;
		} }

		private static KeyValuePair<string, RecipeGroup> _MobBanners;
		public static KeyValuePair<string, RecipeGroup> MobBanners { get {
			if( string.IsNullOrEmpty( RecipeHelpers._MobBanners.Key ) ) {
				RecipeHelpers._MobBanners = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:NpcBanners",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Mob Banner",
						NPCBannerHelpers.GetBannerItemTypes().ToArray() )
				);
			}
			return RecipeHelpers._MobBanners;
		} }
		private static KeyValuePair<string, RecipeGroup> _RecordedMusicBox;
		public static KeyValuePair<string, RecipeGroup> RecordedMusicBox { get {
			if( string.IsNullOrEmpty( RecipeHelpers._RecordedMusicBox.Key ) ) {
				RecipeHelpers._RecordedMusicBox = new KeyValuePair<string, RecipeGroup>(
					"HamstarHelpers:RecordedMusicBoxes",
					new RecipeGroup( () => Lang.misc[37].ToString() + " Recorded Music Box (vanilla)",
						ItemMusicBoxHelpers.GetVanillaMusicBoxes().ToArray() )
				);
			}
			return RecipeHelpers._RecordedMusicBox;
		} }

		
		////////////////

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

			return groups;
		}
	}
}
