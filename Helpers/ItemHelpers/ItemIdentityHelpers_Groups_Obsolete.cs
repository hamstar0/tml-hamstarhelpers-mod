using System;
using Terraria.ID;


namespace HamstarHelpers.ItemHelpers {
	public partial class ItemIdentityHelpers {
		[Obsolete( "Use EvilBiomeBossChunks", false )]
		public static int[] EvilBiomeBossChunkTypes { get { return new int[] { ItemID.ShadowScale, ItemID.TissueSample }; } }
		[Obsolete( "Use EvilBiomeLightPets", false )]
		public static int[] EvilBiomeLightPetTypes { get { return new int[] { ItemID.CrimsonHeart, ItemID.ShadowOrb }; } }

		[Obsolete( "Use MagicMirrors", false )]
		public static int[] MagicMirrorTypes { get { return new int[] { ItemID.MagicMirror, ItemID.IceMirror }; } }


		[Obsolete( "Use VanillaAnimals", false )]
		public static int[] VanillaAnimalTypes { get { return new int[] { ItemID.Bird, ItemID.BlueJay, ItemID.Cardinal,
			ItemID.Duck, ItemID.MallardDuck, ItemID.Penguin,
			ItemID.Bunny, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.Frog, ItemID.Mouse,
			ItemID.Goldfish,
			ItemID.GoldBunny, ItemID.GoldBird, ItemID.GoldFrog, ItemID.GoldMouse, ItemID.SquirrelGold };
		} }

		[Obsolete( "Use VanillaBugs", false )]
		public static int[] VanillaBugTypes { get { return new int[] { ItemID.Firefly, ItemID.LightningBug,
			ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly,
			ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly,
			ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly,
			ItemID.Scorpion, ItemID.BlackScorpion, ItemID.Grasshopper,
			ItemID.EnchantedNightcrawler, ItemID.Worm,
			ItemID.GlowingSnail, ItemID.Grubby, ItemID.Sluggy, ItemID.Snail,
			ItemID.TruffleWorm,
			ItemID.GoldGrasshopper, ItemID.GoldWorm, ItemID.GoldButterfly };
		} }

		[Obsolete( "Use VanillaButterflies", false )]
		public static int[] VanillaButterflyTypes { get { return new int[] {
			ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly,
			ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly,
			ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly, ItemID.GoldButterfly };
		} }

		[Obsolete( "Use VanillaGoldCritters", false )]
		public static int[] VanillaGoldCritterTypes { get { return new int[] {
			ItemID.GoldBunny, ItemID.GoldMouse, ItemID.SquirrelGold, ItemID.GoldBird, ItemID.GoldFrog,
			ItemID.GoldGrasshopper, ItemID.GoldWorm, ItemID.GoldButterfly };
		} }

		[Obsolete( "Use AlchemyHerbss", false )]
		public static int[] AlchemyHerbsTypes { get { return new int[] {
			ItemID.Daybloom, ItemID.Blinkroot, ItemID.Moonglow, ItemID.Deathweed, ItemID.Fireblossom, ItemID.Shiverthorn };
		 } }
		[Obsolete( "Use StrangePlants", false )]
		public static int[] StrangePlantTypes { get { return new int[] {
			ItemID.StrangePlant1, ItemID.StrangePlant2, ItemID.StrangePlant3, ItemID.StrangePlant4 };
		} }

		 [Obsolete("Use PressurePlateTypes", true)]
		 public static int[] PressurePlates { get { return ItemIdentityHelpers.PressurePlateTypes; } }
		[Obsolete( "Use AllPressurePlates", false )]
		public static int[] PressurePlateTypes { get { return new int[] {
			ItemID.BluePressurePlate, ItemID.BrownPressurePlate, ItemID.GrayPressurePlate, ItemID.GreenPressurePlate,
			ItemID.LihzahrdPressurePlate, ItemID.RedPressurePlate, ItemID.YellowPressurePlate,
			ItemID.WeightedPressurePlateCyan, ItemID.WeightedPressurePlateOrange, ItemID.WeightedPressurePlatePink,
			ItemID.WeightedPressurePlatePurple, ItemID.ProjectilePressurePad };
		} }
		 [Obsolete( "Use WeightedPressurePlatesTypes", true )]
		 public static int[] WeightedPressurePlates { get { return ItemIdentityHelpers.WeightedPressurePlateTypes; } }
		[Obsolete( "Use WeightPressurePlates", false )]
		public static int[] WeightedPressurePlateTypes { get { return new int[] {
			ItemID.WeightedPressurePlateCyan, ItemID.WeightedPressurePlateOrange, ItemID.WeightedPressurePlatePink,
			ItemID.WeightedPressurePlatePurple };
		} }
		 [Obsolete( "Use ConveyorBeltTypes", true )]
		 public static int[] ConveyorBelts { get { return ItemIdentityHelpers.ConveyorBeltTypes; } }
		[Obsolete( "Use ConveyorBelts", false )]
		public static int[] ConveyorBeltTypes { get { return new int[] {
			ItemID.ConveyorBeltLeft, ItemID.ConveyorBeltRight };
		} }
	}
}
