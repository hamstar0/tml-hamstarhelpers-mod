using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.ItemHelpers {
	public class ItemIdentityHelpers {
		public const int HighestVanillaRarity = 11;
		public const int JunkRarity = -1;
		public const int QuestItemRarity = -11;


		////////////////

		public static readonly int[] EvilBiomeBossChunkTypes = new int[] { ItemID.ShadowScale, ItemID.TissueSample };
		public static readonly int[] EvilBiomeLightPetTypes = new int[] { ItemID.CrimsonHeart, ItemID.ShadowOrb };

		public static readonly int[] MagicMirrorTypes = new int[] { ItemID.MagicMirror, ItemID.IceMirror };


		public static readonly int[] VanillaAnimalTypes = new int[] { ItemID.Bird, ItemID.BlueJay, ItemID.Cardinal,
			ItemID.Duck, ItemID.MallardDuck, ItemID.Penguin,
			ItemID.Bunny, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.Frog, ItemID.Mouse,
			ItemID.Goldfish,
			ItemID.GoldBunny, ItemID.GoldBird, ItemID.GoldFrog, ItemID.GoldMouse, ItemID.SquirrelGold };

		public static readonly int[] VanillaBugTypes = new int[] { ItemID.Firefly, ItemID.LightningBug,
			ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly,
				ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly,
				ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly,
			ItemID.Scorpion, ItemID.BlackScorpion, ItemID.Grasshopper,
			ItemID.EnchantedNightcrawler, ItemID.Worm,
			ItemID.GlowingSnail, ItemID.Grubby, ItemID.Sluggy, ItemID.Snail,
			ItemID.TruffleWorm,
			ItemID.GoldGrasshopper, ItemID.GoldWorm, ItemID.GoldButterfly };

		public static readonly int[] VanillaButterflyTypes = new int[] {
			ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly,
			ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly,
			ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly, ItemID.GoldButterfly };

		public static readonly int[] VanillaGoldCritterTypes = new int[] {
			ItemID.GoldBunny, ItemID.GoldMouse, ItemID.SquirrelGold, ItemID.GoldBird, ItemID.GoldFrog,
			ItemID.GoldGrasshopper, ItemID.GoldWorm, ItemID.GoldButterfly };
		
		public static readonly int[] AlchemyHerbsTypes = new int[] {
			ItemID.Daybloom, ItemID.Blinkroot, ItemID.Moonglow, ItemID.Deathweed, ItemID.Fireblossom, ItemID.Shiverthorn };
		public static readonly int[] StrangePlantTypes = new int[] {
			ItemID.StrangePlant1, ItemID.StrangePlant2, ItemID.StrangePlant3, ItemID.StrangePlant4 };

		public static readonly int[] PressurePlates = new int[] {
			ItemID.BluePressurePlate, ItemID.BrownPressurePlate, ItemID.GrayPressurePlate, ItemID.GreenPressurePlate,
			ItemID.LihzahrdPressurePlate, ItemID.RedPressurePlate, ItemID.YellowPressurePlate,
			ItemID.WeightedPressurePlateCyan, ItemID.WeightedPressurePlateOrange, ItemID.WeightedPressurePlatePink,
			ItemID.WeightedPressurePlatePurple, ItemID.ProjectilePressurePad };
		public static readonly int[] WeightedPressurePlates = new int[] {
			ItemID.WeightedPressurePlateCyan, ItemID.WeightedPressurePlateOrange, ItemID.WeightedPressurePlatePink,
			ItemID.WeightedPressurePlatePurple };
		public static readonly int[] ConveyorBelts = new int[] { ItemID.ConveyorBeltLeft, ItemID.ConveyorBeltRight };


		////////////////

		public static IReadOnlyDictionary<string, int> NamesToIds {
			get { return HamstarHelpersMod.Instance.ItemIdentityHelpers._NamesToIds; }
		}



		////////////////

		private IDictionary<string, int> __namesToIds = new Dictionary<string, int>();
		private IReadOnlyDictionary<string, int> _NamesToIds = null;


		////////////////
		
		internal void OnPostSetupContent() {
			this._NamesToIds = new ReadOnlyDictionary<string, int>( this.__namesToIds );

			for( int i = 1; i < Main.itemTexture.Length; i++ ) {
				Item item = new Item();
				item.SetDefaults( i );

				this.__namesToIds[item.Name] = i;
			}
		}


		////////////////

		private static IDictionary<int, int> ProjPene = new Dictionary<int, int>();

		public static bool IsPenetrator( Item item ) {
			if( item.shoot <= 0 ) { return false; }

			if( !ItemIdentityHelpers.ProjPene.Keys.Contains( item.shoot ) ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				ItemIdentityHelpers.ProjPene[item.shoot] = proj.penetrate;
			}

			return ItemIdentityHelpers.ProjPene[item.shoot] == -1 || ItemIdentityHelpers.ProjPene[item.shoot] >= 3;   // 3 seems fair?
		}


		public static bool IsTool( Item item ) {
			return (item.useStyle > 0 ||
					item.damage > 0 ||
					item.crit > 0 ||
					item.knockBack > 0 ||
					item.melee ||
					item.magic ||
					item.ranged ||
					item.thrown ||
					item.summon ||
					item.pick > 0 ||
					item.hammer > 0 ||
					item.axe > 0) &&
				!item.accessory &&
				!item.potion &&
				!item.consumable &&
				!item.vanity &&
				item.type != 849;   // Actuators are not consumable, apparently
		}


		public static bool IsArmor( Item item ) {
			return (item.headSlot != -1 ||
					item.bodySlot != -1 ||
					item.legSlot != -1) &&
				!item.accessory &&
				!item.potion &&
				!item.consumable &&
				!item.vanity;
		}


		public static bool IsGrapple( Item item ) {
			return Main.projHook[item.shoot];
		}


		public static bool IsYoyo( Item item ) {
			if( item.shoot > 0 && item.useStyle == 5 && item.melee && item.channel ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				return proj.aiStyle == 99;
			}
			return false;
		}

		
		public static bool IsGameplayRelevant( Item item, bool toys_relevant=false, bool junk_relevant=false ) {
			if( !toys_relevant ) {
				switch( item.type ) {
				case ItemID.WaterGun:
				case ItemID.SlimeGun:
				case ItemID.BeachBall:
					return false;
				}
			}
			if( junk_relevant && item.rare < 0 ) { return false; }
			return !item.vanity && item.dye <= 0 && item.hairDye <= 0 && item.paint > 0 && !Main.vanityPet[ item.buffType ];
		}


		public static float LooselyAppraise( Item item ) {
			float appraisal = item.rare;
			if( item.value > 0 ) {
				float value = (float)item.value / 8000f;
				appraisal = ((appraisal * 4f) + value) / 5f;
			}
			return appraisal;
		}
	}
}
