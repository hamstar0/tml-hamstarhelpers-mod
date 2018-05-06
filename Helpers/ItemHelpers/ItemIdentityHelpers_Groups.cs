using System;
using Terraria.ID;


namespace HamstarHelpers.ItemHelpers {
	public partial class ItemIdentityHelpers {
		public static readonly Tuple<string, int[]> EvilBiomeBossChunks = Tuple.Create(
			"Evil Biome Boss Chunk",
			new int[] { ItemID.ShadowScale, ItemID.TissueSample }
		);

		public static readonly Tuple<string, int[]> EvilBiomeLightPets = Tuple.Create(
			"Evil Biome Light Pet",
			new int[] { ItemID.CrimsonHeart, ItemID.ShadowOrb }
		);

		public static readonly Tuple<string, int[]> MagicMirrors = Tuple.Create(
			"Magic Mirrors",
			new int[] { ItemID.MagicMirror, ItemID.IceMirror }
		);

		
		public static readonly Tuple<string, int[]> VanillaAnimals = Tuple.Create(
			"Live Animal (vanilla)",
			new int[] {
				ItemID.Bird, ItemID.BlueJay, ItemID.Cardinal,
				ItemID.Duck, ItemID.MallardDuck, ItemID.Penguin,
				ItemID.Bunny, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.Frog, ItemID.Mouse,
				ItemID.Goldfish,
				ItemID.GoldBunny, ItemID.GoldBird, ItemID.GoldFrog, ItemID.GoldMouse, ItemID.SquirrelGold
			}
		);
		
		public static readonly Tuple<string, int[]> VanillaBugs = Tuple.Create(
			"Live Bug (vanilla)",
			new int[] {
				ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly,
				ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly,
				ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly,
				ItemID.Scorpion, ItemID.BlackScorpion, ItemID.Grasshopper,
				ItemID.EnchantedNightcrawler, ItemID.Worm,
				ItemID.GlowingSnail, ItemID.Grubby, ItemID.Sluggy, ItemID.Snail,
				ItemID.TruffleWorm,
				ItemID.GoldGrasshopper, ItemID.GoldWorm, ItemID.GoldButterfly
			}
		);
		
		public static readonly Tuple<string, int[]> VanillaButterflies = Tuple.Create(
			"Butterflies (vanilla)",
			new int[] {
				ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly,
				ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly,
				ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly, ItemID.GoldButterfly
			}
		);
		
		public static readonly Tuple<string, int[]> VanillaGoldCritters = Tuple.Create(
			"Gold Critters (vanilla)",
			new int[] {
				ItemID.GoldBunny, ItemID.GoldMouse, ItemID.SquirrelGold, ItemID.GoldBird, ItemID.GoldFrog,
				ItemID.GoldGrasshopper, ItemID.GoldWorm, ItemID.GoldButterfly
			}
		);
		
		public static readonly Tuple<string, int[]> AlchemyHerbs = Tuple.Create(
			"Alchemy Herbs (vanilla)",
			new int[] {
				ItemID.Daybloom, ItemID.Blinkroot, ItemID.Moonglow, ItemID.Deathweed, ItemID.Fireblossom, ItemID.Shiverthorn
			}
		);
		public static readonly Tuple<string, int[]> StrangePlants = Tuple.Create(
			"Strange Plant",
			new int[] { ItemID.StrangePlant1, ItemID.StrangePlant2, ItemID.StrangePlant3, ItemID.StrangePlant4 }
		);
		
		public static readonly Tuple<string, int[]> AllPressurePlates = Tuple.Create(
			"Pressure Plates",
			new int[] {
				ItemID.BluePressurePlate, ItemID.BrownPressurePlate, ItemID.GrayPressurePlate, ItemID.GreenPressurePlate,
			ItemID.LihzahrdPressurePlate, ItemID.RedPressurePlate, ItemID.YellowPressurePlate,
			ItemID.WeightedPressurePlateCyan, ItemID.WeightedPressurePlateOrange, ItemID.WeightedPressurePlatePink,
			ItemID.WeightedPressurePlatePurple, ItemID.ProjectilePressurePad
			}
		);
		public static readonly Tuple<string, int[]> WeightPressurePlates = Tuple.Create(
			"Weighted Pressure Plates",
			new int[] {
				ItemID.WeightedPressurePlateCyan, ItemID.WeightedPressurePlateOrange, ItemID.WeightedPressurePlatePink,
				ItemID.WeightedPressurePlatePurple
			}
		);
		public static readonly Tuple<string, int[]> ConveyorBeltPair = Tuple.Create(
			"Conveyor Belts",
			new int[] { ItemID.ConveyorBeltLeft, ItemID.ConveyorBeltRight }
		);
		
		public static readonly Tuple<string, int[]> Paints = Tuple.Create(
			"Paints",
			new int[] {
				ItemID.BlackPaint, ItemID.BluePaint, ItemID.BrownPaint, ItemID.CyanPaint,
				ItemID.DeepBluePaint, ItemID.DeepCyanPaint, ItemID.DeepGreenPaint, ItemID.DeepLimePaint,
				ItemID.DeepOrangePaint, ItemID.DeepPinkPaint, ItemID.DeepPurplePaint, ItemID.DeepRedPaint,
				ItemID.DeepSkyBluePaint, ItemID.DeepTealPaint, ItemID.DeepVioletPaint, ItemID.DeepYellowPaint,
				ItemID.GrayPaint, ItemID.GreenPaint, ItemID.LimePaint, ItemID.NegativePaint, ItemID.OrangePaint,
				ItemID.PinkPaint, ItemID.PurplePaint, ItemID.RedPaint, ItemID.ShadowPaint, ItemID.SkyBluePaint,
				ItemID.TealPaint, ItemID.VioletPaint, ItemID.WhitePaint, ItemID.YellowPaint
			}
		);
		public static readonly Tuple<string, int[]> VanillaDyes = Tuple.Create(
			"Dyes (vanilla)",
			new int[] {
				ItemID.RedDye, ItemID.RedDye, ItemID.OrangeDye, ItemID.YellowDye, ItemID.LimeDye, ItemID.BrightTealDye, ItemID.BrightCyanDye,
				ItemID.BrightSkyBlueDye, ItemID.BrightBlueDye, ItemID.BrightPurpleDye, ItemID.BrightVioletDye, ItemID.BrightPinkDye,
				ItemID.BlackDye, ItemID.RedandSilverDye, ItemID.OrangeandSilverDye, ItemID.YellowandSilverDye, ItemID.LimeandSilverDye,
				ItemID.GreenandSilverDye, ItemID.TealandSilverDye, ItemID.CyanandSilverDye, ItemID.SkyBlueandSilverDye, ItemID.BlueandSilverDye,
				ItemID.PurpleandSilverDye, ItemID.VioletandSilverDye, ItemID.PinkandSilverDye, ItemID.IntenseFlameDye,
				ItemID.IntenseGreenFlameDye, ItemID.IntenseBlueFlameDye, ItemID.RainbowDye, ItemID.IntenseRainbowDye, ItemID.YellowGradientDye,
				ItemID.CyanGradientDye, ItemID.BrightGreenDye, ItemID.BrightLimeDye, ItemID.BrightYellowDye, ItemID.BrightOrangeDye,
				ItemID.GreenDye, ItemID.TealDye, ItemID.CyanDye, ItemID.SkyBlueDye, ItemID.BlueDye, ItemID.PurpleDye, ItemID.VioletDye,
				ItemID.PinkDye, ItemID.RedandBlackDye, ItemID.OrangeandBlackDye, ItemID.YellowandBlackDye, ItemID.LimeandBlackDye,
				ItemID.GreenandBlackDye, ItemID.OrichalcumRepeater, ItemID.TealandBlackDye, ItemID.SkyBlueandBlackDye, ItemID.BlueandBlackDye,
				ItemID.PurpleandBlackDye, ItemID.VioletandBlackDye, ItemID.PinkandBlackDye, ItemID.FlameDye, ItemID.FlameAndBlackDye,
				ItemID.GreenFlameDye, ItemID.GreenFlameAndBlackDye, ItemID.BlueFlameDye, ItemID.BlueFlameAndBlackDye, ItemID.SilverDye,
				ItemID.BrightRedDye, ItemID.CyanandBlackDye, ItemID.VioletGradientDye, ItemID.TeamDye, ItemID.MartianHairDye,
				ItemID.MartianArmorDye, ItemID.LivingFlameDye, ItemID.LivingRainbowDye, ItemID.ShadowDye, ItemID.NegativeDye,
				ItemID.LivingOceanDye, ItemID.PumpkinSink, ItemID.BrownDye, ItemID.DevDye, ItemID.PurpleOozeDye, ItemID.ReflectiveSilverDye,
				ItemID.ReflectiveGoldDye, ItemID.BlueAcidDye, ItemID.HadesDye, ItemID.TwilightDye, ItemID.AcidDye, ItemID.MushroomDye,
				ItemID.PhaseDye, ItemID.ReflectiveDye, ItemID.TwilightHairDye, ItemID.SolarDye, ItemID.NebulaDye, ItemID.VortexDye,
				ItemID.StardustDye, ItemID.VoidDye, ItemID.ShiftingSandsDye, ItemID.MirageDye, ItemID.ShiftingPearlSandsDye,
				ItemID.FlameAndSilverDye, ItemID.GreenFlameAndSilverDye, ItemID.BlueFlameAndSilverDye, ItemID.ReflectiveCopperDye,
				ItemID.ReflectiveObsidianDye, ItemID.ReflectiveMetalDye, ItemID.MidnightRainbowDye, ItemID.BlackAndWhiteDye,
				ItemID.BrightSilverDye, ItemID.SilverAndBlackDye, ItemID.RedAcidDye, ItemID.GelDye, ItemID.PinkGelDye, ItemID.BurningHadesDye,
				ItemID.GrimDye, ItemID.LokisDye, ItemID.ShadowflameHadesDye
			}
		);
	}
}
