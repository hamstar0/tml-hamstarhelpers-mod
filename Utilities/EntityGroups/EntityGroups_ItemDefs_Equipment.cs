using HamstarHelpers.ItemHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemEquipmentGroups1( Action<string, Func<Item, bool>> add_def ) {
			add_def( "Any Weapon", ( Item item ) => {
				return item.damage > 0;
			} );
			add_def( "Any Tool", ( Item item ) => {
				return ItemAttributeHelpers.IsTool( item );
			} );

			add_def( "Any Accessory", ( Item item ) => {
				return item.accessory && !item.vanity;
			} );
			add_def( "Any Armor", ( Item item ) => {
				return ItemAttributeHelpers.IsArmor( item );
			} );
			add_def( "Any Vanity", ( Item item ) => {
				return item.vanity;
			} );
			add_def( "Any Potion", ( Item item ) => {
				return item.potion;
			} );

			// Weapon Classes

			add_def( "Any Ranged Weapon", ( Item item ) => {
				return item.ranged;
			} );
			add_def( "Any Magic Weapon", ( Item item ) => {
				return item.magic;
			} );
			add_def( "Any Melee Weapon", ( Item item ) => {
				return item.melee;
			} );
			add_def( "Any Thrown Weapon", ( Item item ) => {
				return item.thrown;
			} );

			// Melee Sub Classes

			add_def( "Any Swingable", ( Item item ) => {
				return item.melee && item.useStyle == 1;
			} );
			add_def( "Any Thrustable", ( Item item ) => {
				return item.melee && item.useStyle == 5;
			} );
			add_def( "Any Flail", ( Item item ) => {
				if( !item.melee || item.useStyle != 5 ) { return false; }
				if( item.type == ItemID.Anchor ) { return true; }

				var proj_pool = this.GetProjPool();

				switch( proj_pool[ item.shoot ].aiStyle ) {
				case 15:    // Standard
				case 13:    // Chain Knife, Boxing Glove
				case 69:    // Flairon
				case 75:    // Solar Eruption
					return true;
				//case 3:     // Anchor
				}
				return false;
			} );
			add_def( "Any Boomerang", ( Item item ) => {
				if( !item.melee || item.useStyle != 1 ) { return false; }
				if( item.type == ItemID.FlyingKnife ) { return true; }

				var proj_pool = this.GetProjPool();

				switch( proj_pool[ item.shoot ].aiStyle ) {
				case 3:    // Boomerangs
				case 15:    // Thorn Chakram
					return true;
				}
				return false;
			} );
			add_def( "Any Yoyo", ( Item item ) => {
				return ItemAttributeHelpers.IsYoyo( item );
			} );

			// Magic Sub Classes

			add_def( "Any Magic Staff Or Scepter Or Wand", ( Item item ) => {
				if( !item.magic ) { return false; }

				string name = ItemIdentityHelpers.GetQualifiedName( item );
				return name.Contains( "Staff" ) ||
					name.Contains( "Scepter" ) ||
					name.Contains( "Wand" );
			} );
			add_def( "Any Magic Rod", ( Item item ) => {
				if( !item.magic ) { return false; }

				string name = ItemIdentityHelpers.GetQualifiedName( item );
				return name.Contains( "Rod" );
			} );
			add_def( "Any Magic Gun", ( Item item ) => {
				if( !item.magic ) { return false; }

				switch( item.type ) {
				case ItemID.LeafBlower:
					return true;
				}

				string name = ItemIdentityHelpers.GetQualifiedName( item );
				return name.Contains( "Gun" ) ||
					name.Contains( "Rifle" ) ||
					name.Contains( "Ray" ) ||
					name.Contains( "Cannon" ) ||
					name.Contains( "Laser" );
			} );
			add_def( "Any Magic Tome", ( Item item ) => {
				if( !item.magic ) { return false; }

				switch( item.type ) {
				case ItemID.DemonScythe:
				case ItemID.WaterBolt:
				case ItemID.LunarFlareBook:
				case ItemID.MagnetSphere:
				case ItemID.RazorbladeTyphoon:
					return true;
				}

				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.SpellTome }, 1 );
				if( has ) { return true; }

				string name = ItemIdentityHelpers.GetQualifiedName( item );
				return name.Contains( "Book" ) ||
					name.Contains( "Tome" );
			} );

			// Ranged Sub Classes

			add_def( "Any Ranger Gun", ( Item item ) => {
				if( !item.ranged ) { return false; }
				return item.useAmmo == AmmoID.Bullet ||
					item.useAmmo == AmmoID.CandyCorn ||
					item.useAmmo == AmmoID.Coin;
			} );
			add_def( "Any Ranger Bow", ( Item item ) => {
				if( !item.ranged ) { return false; }
				return item.useAmmo == AmmoID.Arrow ||
					item.useAmmo == AmmoID.Stake;
			} );
			add_def( "Any Ranger Launcher", ( Item item ) => {
				if( !item.ranged ) { return false; }
				return item.useAmmo == AmmoID.Rocket ||
					item.useAmmo == AmmoID.StyngerBolt ||
					item.useAmmo == AmmoID.JackOLantern ||
					item.useAmmo == AmmoID.NailFriendly;
			} );

			// Summon Sub Classes

			add_def( "Any Minion Summon Item", ( Item item ) => {
				return item.summon && !item.sentry;
			} );
			add_def( "Any Sentry Summon Item", ( Item item ) => {
				return item.summon && item.sentry;
			} );

			// Accessory Classes

			add_def( "Any Wing Accessory", ( Item item ) => {
				if( !item.accessory || item.vanity ) { return false; }
				return item.wingSlot > 0;
			} );
			add_def( "Any Vanilla Movement Accessory", ( Item item ) => {
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
			} );
			add_def( "Any Vanilla Combat Accessory", ( Item item ) => {
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
			} );
			add_def( "Any Vanilla Yoyo Accessory", ( Item item ) => {
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
			} );
			add_def( "Any Vanilla Health Or Mana Accessory", ( Item item ) => {
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
			} );
			add_def( "Any Vanilla Construction Accessory", ( Item item ) => {
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
			} );
			add_def( "Any Vanilla Information Accessory", ( Item item ) => {
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
			} );
			add_def( "Any Vanilla Fishing Accessory", ( Item item ) => {
				if( !item.accessory || item.vanity ) { return false; }
				switch( item.type ) {
				case ItemID.HighTestFishingLine:
				case ItemID.AnglerEarring:
				case ItemID.TackleBox:
				case ItemID.AnglerTackleBag:
					return true;
				}
				return false;
			} );
			add_def( "Any Vanilla Music Box Accessory", ( Item item ) => {
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
			} );
			add_def( "Any Vanilla Expert Accessory", ( Item item ) => {
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
			} );

			// Vanity Classes
			add_def( "Any Vanity Accessory", ( Item item ) => {
				if( !item.vanity ) { return false; }
				return item.accessory;
			} );
			add_def( "Any Vanity Garment", ( Item item ) => {
				if( !item.vanity ) { return false; }
				return !item.accessory;
			} );
		}


		private void DefineItemEquipmentGroups2( Action<string, Func<Item, bool>> add_def ) {
			var placeables = EntityGroups.ItemGroups["Any Placeables"];

			// Misc Sub Classes

			add_def( "Any Ranger Misc", ( Item item ) => {
				if( !item.ranged ) { return false; }
				return !EntityGroups.ItemGroups["Any Ranger Gun"].Contains( item.type ) &&
						!EntityGroups.ItemGroups["Any Ranger Bow"].Contains( item.type ) &&
						!EntityGroups.ItemGroups["Any Ranger Launcher"].Contains( item.type );
			} );

			add_def( "Any Magic Misc", ( Item item ) => {
				if( !item.magic ) { return false; }
				return !EntityGroups.ItemGroups["Any Magic Staff Or Scepter Or Wand"].Contains( item.type ) &&
						!EntityGroups.ItemGroups["Any Magic Rod"].Contains( item.type ) &&
						!EntityGroups.ItemGroups["Any Magic Gun"].Contains( item.type ) &&
						!EntityGroups.ItemGroups["Any Magic Tome"].Contains( item.type );
			} );
			
			// Equipment Tiers

			add_def( "Any Tiki Equipment", ( Item item ) => {
				string name = ItemIdentityHelpers.GetQualifiedName( item );
				if( !name.Contains( "Tiki" ) ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Plain Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Wood }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Boreal Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.BorealWood }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Palm Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.PalmWood }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Rich Mahogany Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.RichMahogany }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Ebonwood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Ebonwood }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Shadewood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Shadewood }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Pearlwood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Pearlwood }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Dynasty Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.DynastyWood }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Spooky Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.SpookyWood }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Tin Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.TinBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Copper Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.CopperBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Iron Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.IronBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Lead Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.LeadBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Silver Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.SilverBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Tungsten Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.TungstenBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Gold Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.GoldBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Platinum Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.PlatinumBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Meteor Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.MeteoriteBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Demonite Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.DemoniteBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Crimtane Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.CrimtaneBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Jungle Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.JungleSpores }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Bee Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.BeeWax }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Bone Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Bone }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Hellstone Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.HellstoneBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Cobalt Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.CobaltBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Palladium Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.PalladiumBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Mythril Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.MythrilBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Orichalcum Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.OrichalcumBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Adamantite Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.AdamantiteBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Titanium Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.TitaniumBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Frost Core Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FrostCore }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Forbidden Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.AncientBattleArmorMaterial }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Hallow Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.HallowedBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Chlorophyte Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.ChlorophyteBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Shroomite Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.ShroomiteBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Spectre Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.SpectreBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Shell Equipment", ( Item item ) => {
				var has1 = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.BeetleShell }, 1 );
				var has2 = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.TurtleShell }, 1 );
				if( !has1 && !has2 ) { return false; }
				return !placeables.Contains( item.type );
			} );

			add_def( "Any Nebula Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FragmentNebula }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Vortex Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FragmentVortex }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Solar Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FragmentSolar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Stardust Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FragmentStardust }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Luminite Ore Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.LunarBar }, 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
		}


		private void DefineItemEquipmentGroups3( Action<string, Func<Item, bool>> add_def ) {
			var placeables = EntityGroups.ItemGroups["Any Placeables"];

			add_def( "Any Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, EntityGroups.ItemGroups["Any Wood"], 1 );
				if( !has ) { return false; }
				return !placeables.Contains( item.type );
			} );
			add_def( "Any Copper Or Tin Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Copper Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Tin Equipment"].Contains( item.type );
			} );
			add_def( "Any Iron Or Lead Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Iron Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Lead Equipment"].Contains( item.type );
			} );
			add_def( "Any Silver Or Tungsten Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Silver Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Tungsten Equipment"].Contains( item.type );
			} );
			add_def( "Any Gold Or Platinum Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Gold Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Platinum Equipment"].Contains( item.type );
			} );
			add_def( "Any Demonite Or Crimtane Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Demonite Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Crimtane Equipment"].Contains( item.type );
			} );
			add_def( "Any Meteor Or Jungle Or Bone Or Bee Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Meteor Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Jungle Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Bone Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Bee Equipment"].Contains( item.type );
			} );
			add_def( "Any Cobalt Or Palladium Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Cobalt Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Palladium Equipment"].Contains( item.type );
			} );
			add_def( "Any Mythril Or Orichalcum Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Mythril Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Orichalcum Equipment"].Contains( item.type );
			} );
			add_def( "Any Adamantite Or Titanium Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Adamantite Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Titanium Equipment"].Contains( item.type );
			} );
			add_def( "Any Frost Core Or Forbidden Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Frost Core Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Forbidden Equipment"].Contains( item.type );
			} );
			add_def( "Any Chlorophyte Or Shroomite Or Spectre Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Chlorophyte Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Shroomite Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Spectre Equipment"].Contains( item.type );
			} );
			add_def( "Any Celestial Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Nebula Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Vortex Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Solar Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Stardust Equipment"].Contains( item.type );
			} );
		}
	}
}
