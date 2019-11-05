using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public static readonly string AnyWingAccessory = "Any Wing Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaMovementAccessory = "Any Vanilla Movement Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaCombatAccessory = "Any Vanilla Combat Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaYoyoAccessory = "Any Vanilla Yoyo Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaHealthOrManaAccessory = "Any Vanilla Health Or Mana Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaConstructionAccessory = "Any Vanilla Construction Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaInformationAccessory = "Any Vanilla Information Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaFishingAccessory = "Any Vanilla Fishing Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaMusicBoxAccessory = "Any Vanilla Music Box Accessory";
		/// <summary></summary>
		public static readonly string AnyVanillaExpertAccessory = "Any Vanilla Expert Accessory";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemAccessoriesGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			// Accessory Classes
			
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyWingAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					return item.wingSlot > 0;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaMovementAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.Aglet:
					case ItemID.BalloonHorseshoeHoney:
					case ItemID.AnkletoftheWind:
					case ItemID.ArcticDivingGear:
					case ItemID.BalloonPufferfish:
					case ItemID.BlizzardinaBalloon:
					case ItemID.BlizzardinaBottle:
					case ItemID.BlueHorseshoeBalloon:
					case ItemID.BundleofBalloons:
					case ItemID.ClimbingClaws:
					case ItemID.CloudinaBalloon:
					case ItemID.CloudinaBottle:
					case ItemID.DivingGear:
					case ItemID.FartInABalloon:
					case ItemID.FartinaJar:
					case ItemID.Flipper:
					case ItemID.FlurryBoots:
					case ItemID.FlyingCarpet:
					case ItemID.FrogLeg:
					case ItemID.FrostsparkBoots:
					case ItemID.BalloonHorseshoeFart:
					case ItemID.HermesBoots:
					case ItemID.HoneyBalloon:
					case ItemID.IceSkates:
					case ItemID.JellyfishDivingGear:
					case ItemID.LavaCharm:
					case ItemID.LavaWaders:
					case ItemID.LightningBoots:
					case ItemID.LuckyHorseshoe:
					case ItemID.MasterNinjaGear:
					case ItemID.ObsidianHorseshoe:
					case ItemID.ObsidianWaterWalkingBoots:
					case ItemID.BalloonHorseshoeSharkron:
					case ItemID.RocketBoots:
					case ItemID.SailfishBoots:
					case ItemID.SandstorminaBalloon:
					case ItemID.SandstorminaBottle:
					case ItemID.SharkronBalloon:
					case ItemID.ShinyRedBalloon:
					case ItemID.ShoeSpikes:
					case ItemID.SpectreBoots:
					case ItemID.Tabi:
					case ItemID.TigerClimbingGear:
					case ItemID.TsunamiInABottle:
					case ItemID.WaterWalkingBoots:
					case ItemID.WhiteHorseshoeBalloon:
					case ItemID.YellowHorseshoeBalloon:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaCombatAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.AdhesiveBandage:
					case ItemID.AnkhCharm:
					case ItemID.AnkhShield:
					case ItemID.ArmorBracing:
					case ItemID.ArmorPolish:
					case ItemID.AvengerEmblem:
					case ItemID.BeeCloak:
					case ItemID.Bezoar:
					case ItemID.BlackBelt:
					case ItemID.Blindfold:
					case ItemID.CelestialEmblem:
					case ItemID.MoonCharm:
					case ItemID.MoonShell:
					case ItemID.CelestialStone:
					case ItemID.CelestialShell:
					case ItemID.CobaltShield:
					case ItemID.CountercurseMantra:
					case ItemID.CrossNecklace:
					case ItemID.DestroyerEmblem:
					case ItemID.EyeoftheGolem:
					case ItemID.FastClock:
					case ItemID.FeralClaws:
					case ItemID.FireGauntlet:
					case ItemID.FleshKnuckles:
					case ItemID.FrozenTurtleShell:
					case ItemID.HandWarmer:
					case ItemID.HoneyComb:
					case ItemID.MagicQuiver:
					case ItemID.MagmaStone:
					case ItemID.MechanicalGlove:
					case ItemID.MedicatedBandage:
					case ItemID.Megaphone:
					case ItemID.MoonStone:
					case ItemID.Nazar:
					case ItemID.ObsidianRose:
					case ItemID.ObsidianShield:
					case ItemID.ObsidianSkull:
					case ItemID.PaladinsShield:
					case ItemID.PanicNecklace:
					case ItemID.PocketMirror:
					case ItemID.PowerGlove:
					case ItemID.PutridScent:
					case ItemID.RangerEmblem:
					case ItemID.RifleScope:
					case ItemID.Shackle:
					case ItemID.SharkToothNecklace:
					case ItemID.SniperScope:
					case ItemID.SorcererEmblem:
					case ItemID.StarCloak:
					case ItemID.StarVeil:
					case ItemID.SummonerEmblem:
					case ItemID.SunStone:
					case ItemID.SweetheartNecklace:
					case ItemID.ThePlan:
					case ItemID.TitanGlove:
					case ItemID.TrifoldMap:
					case ItemID.Vitamins:
					case ItemID.WarriorEmblem:
					case ItemID.ApprenticeScarf:
					case ItemID.SquireShield:
					case ItemID.HuntressBuckler:
					case ItemID.MonkBelt:
					case ItemID.HerculesBeetle:
					case ItemID.NecromanticScroll:
					case ItemID.PapyrusScarab:
					case ItemID.PygmyNecklace:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaYoyoAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.YoyoBag:
					case ItemID.YoYoGlove:
					case ItemID.RedString:
					case ItemID.OrangeString:
					case ItemID.YellowString:
					case ItemID.LimeString:
					case ItemID.GreenString:
					case ItemID.TealString:
					case ItemID.CyanString:
					case ItemID.SkyBlueString:
					case ItemID.BlueString:
					case ItemID.PurpleString:
					case ItemID.VioletString:
					case ItemID.PinkString:
					case ItemID.BrownString:
					case ItemID.WhiteString:
					case ItemID.RainbowString:
					case ItemID.BlackString:
					case ItemID.BlackCounterweight:
					case ItemID.YellowCounterweight:
					case ItemID.BlueCounterweight:
					case ItemID.RedCounterweight:
					case ItemID.PurpleCounterweight:
					case ItemID.GreenCounterweight:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaHealthOrManaAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.BandofRegeneration:
					case ItemID.BandofStarpower:
					case ItemID.CelestialCuffs:
					case ItemID.CelestialMagnet:
					case ItemID.CelestialEmblem:
					case ItemID.CharmofMyths:
					case ItemID.MagicCuffs:
					case ItemID.ManaFlower:
					case ItemID.ManaRegenerationBand:
					case ItemID.NaturesGift:
					case ItemID.PhilosophersStone:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaConstructionAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.Toolbelt:
					case ItemID.Toolbox:
					case ItemID.PaintSprayer:
					case ItemID.ExtendoGrip:
					case ItemID.PortableCementMixer:
					case ItemID.BrickLayer:
					case ItemID.ArchitectGizmoPack:
					case ItemID.ActuationAccessory:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaInformationAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.CopperWatch:
					case ItemID.TinWatch:
					case ItemID.SilverWatch:
					case ItemID.TungstenWatch:
					case ItemID.GoldWatch:
					case ItemID.PlatinumWatch:
					case ItemID.DepthMeter:
					case ItemID.Compass:
					case ItemID.Ruler:
					case ItemID.LaserRuler:
					case ItemID.GPS:
					case ItemID.FishermansGuide:
					case ItemID.WeatherRadio:
					case ItemID.Sextant:
					case ItemID.FishFinder:
					case ItemID.MetalDetector:
					case ItemID.Stopwatch:
					case ItemID.DPSMeter:
					case ItemID.GoblinTech:
					case ItemID.TallyCounter:
					case ItemID.LifeformAnalyzer:
					case ItemID.Radar:
					case ItemID.REK:
					case ItemID.PDA:
					case ItemID.MechanicalLens:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaFishingAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.HighTestFishingLine:
					case ItemID.AnglerEarring:
					case ItemID.TackleBox:
					case ItemID.AnglerTackleBag:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaMusicBoxAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.MusicBox:
					case ItemID.MusicBoxOverworldDay:
					case ItemID.MusicBoxAltOverworldDay:
					case ItemID.MusicBoxNight:
					case ItemID.MusicBoxRain:
					case ItemID.MusicBoxSnow:
					case ItemID.MusicBoxIce:
					case ItemID.MusicBoxDesert:
					case ItemID.MusicBoxOcean:
					case ItemID.MusicBoxSpace:
					case ItemID.MusicBoxUnderground:
					case ItemID.MusicBoxAltUnderground:
					case ItemID.MusicBoxMushrooms:
					case ItemID.MusicBoxJungle:
					case ItemID.MusicBoxCorruption:
					case ItemID.MusicBoxUndergroundCorruption:
					case ItemID.MusicBoxCrimson:
					case ItemID.MusicBoxUndergroundCrimson:
					case ItemID.MusicBoxTheHallow:
					case ItemID.MusicBoxUndergroundHallow:
					case ItemID.MusicBoxHell:
					case ItemID.MusicBoxDungeon:
					case ItemID.MusicBoxTemple:
					case ItemID.MusicBoxBoss1:
					case ItemID.MusicBoxBoss2:
					case ItemID.MusicBoxBoss3:
					case ItemID.MusicBoxBoss4:
					case ItemID.MusicBoxBoss5:
					case ItemID.MusicBoxPlantera:
					case ItemID.MusicBoxEerie:
					case ItemID.MusicBoxEclipse:
					case ItemID.MusicBoxGoblins:
					case ItemID.MusicBoxPirates:
					case ItemID.MusicBoxMartians:
					case ItemID.MusicBoxPumpkinMoon:
					case ItemID.MusicBoxFrostMoon:
					case ItemID.MusicBoxTowers:
					case ItemID.MusicBoxLunarBoss:
					case ItemID.MusicBoxSandstorm:
					case ItemID.MusicBoxDD2:
					case ItemID.MusicBoxTitle:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaExpertAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.accessory || item.vanity ) { return false; }
					switch( item.type ) {
					case ItemID.RoyalGel:
					case ItemID.EoCShield:
					case ItemID.WormScarf:
					case ItemID.BrainOfConfusion:
					case ItemID.HiveBackpack:
					case ItemID.SporeSac:
					case ItemID.ShinyStone:
					case ItemID.GravityGlobe:
						return true;
					}
					return false;
				} )
			) );
		}
	}
}
