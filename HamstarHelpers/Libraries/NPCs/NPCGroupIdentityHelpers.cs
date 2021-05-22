using HamstarHelpers.Classes.DataStructures;
using System.Collections.Generic;
using Terraria.ID;


namespace HamstarHelpers.Libraries.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to identifying common groups of NPCs.
	/// </summary>
	public partial class NPCGroupIdentityLibraries {
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaBloodMoonTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaGoblinArmyTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaFrostLegionTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaPirateTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaMartianTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaSolarEclipseTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaPumpkingMoonTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaFrostMoonTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaOldOnesArmyTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaSolarPillarTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaNebulaPillarTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaVortexPillarTypes;
		/// <summary></summary>
		public static readonly ReadOnlySet<int> VanillaStardustPillarTypes;



		static NPCGroupIdentityLibraries() {
			var bloodMoon = new HashSet<int> {
				NPCID.TheGroom, NPCID.TheBride, NPCID.BloodZombie, NPCID.Drippler,
				NPCID.CorruptBunny, NPCID.CorruptGoldfish, NPCID.CorruptPenguin,
				NPCID.CrimsonBunny, NPCID.CrimsonGoldfish, NPCID.CrimsonPenguin,
				NPCID.Clown
			};
			var goblins = new HashSet<int> {
				NPCID.GoblinPeon, NPCID.GoblinSorcerer, NPCID.GoblinThief, NPCID.GoblinWarrior, NPCID.GoblinArcher, NPCID.GoblinSummoner
			};
			var frostLegion = new HashSet<int> {
				NPCID.MisterStabby, NPCID.SnowmanGangsta, NPCID.SnowBalla
			};
			var pirates = new HashSet<int> {
				NPCID.PirateCaptain, NPCID.PirateCorsair, NPCID.PirateCrossbower, NPCID.PirateDeadeye, NPCID.PirateDeckhand, NPCID.Parrot,
				NPCID.PirateShip, NPCID.PirateShipCannon
			};
			var martians = new HashSet<int> {
				NPCID.BrainScrambler, NPCID.GrayGrunt, NPCID.RayGunner, NPCID.MartianOfficer, NPCID.MartianOfficer, NPCID.MartianEngineer,
				NPCID.GigaZapper, NPCID.MartianTurret, NPCID.MartianDrone, NPCID.MartianWalker, NPCID.Scutlix,
				NPCID.MartianSaucer, NPCID.MartianSaucerCannon, NPCID.MartianSaucerCore, NPCID.MartianSaucerTurret
			};
			var solarEclipse = new HashSet<int> {
				NPCID.Eyezor, NPCID.Frankenstein, NPCID.SwampThing, NPCID.Vampire, NPCID.CreatureFromTheDeep, NPCID.Fritz, NPCID.ThePossessed,
				NPCID.Mothron, NPCID.MothronEgg, NPCID.MothronSpawn, NPCID.Reaper,
				NPCID.Butcher, NPCID.DeadlySphere, NPCID.DrManFly, NPCID.Nailhead, NPCID.Psycho
			};
			var pumpkinMoon = new HashSet<int> {
				NPCID.Scarecrow1, NPCID.Scarecrow2, NPCID.Scarecrow3, NPCID.Scarecrow4, NPCID.Scarecrow5, NPCID.Scarecrow6, NPCID.Scarecrow7,
				NPCID.Scarecrow8, NPCID.Scarecrow9, NPCID.Scarecrow10,
				NPCID.Splinterling, NPCID.Hellhound, NPCID.Poltergeist, NPCID.HeadlessHorseman,
				NPCID.MourningWood, NPCID.Pumpking, NPCID.PumpkingBlade
			};
			var frostMoon = new HashSet<int> {
				NPCID.PresentMimic, NPCID.Flocko, NPCID.GingerbreadMan, NPCID.Nutcracker, NPCID.NutcrackerSpinning, NPCID.Yeti,
				NPCID.ZombieElf, NPCID.ZombieElfGirl, NPCID.ElfArcher, NPCID.ElfCopter,
				NPCID.Krampus, NPCID.Everscream, NPCID.SantaNK1, NPCID.IceQueen
			};
			var oldOnesArmy = new HashSet<int> {
				NPCID.DD2LanePortal, NPCID.DD2EterniaCrystal,
				NPCID.DD2GoblinT1, NPCID.DD2GoblinT2, NPCID.DD2GoblinT3,
				NPCID.DD2GoblinBomberT1, NPCID.DD2GoblinBomberT2, NPCID.DD2GoblinBomberT3,
				NPCID.DD2JavelinstT1, NPCID.DD2JavelinstT2, NPCID.DD2JavelinstT3,
				NPCID.DD2KoboldFlyerT2, NPCID.DD2KoboldFlyerT3, NPCID.DD2KoboldWalkerT2, NPCID.DD2KoboldWalkerT3,
				NPCID.DD2LightningBugT3, NPCID.DD2OgreT2, NPCID.DD2OgreT3, NPCID.DD2SkeletonT1, NPCID.DD2SkeletonT3,
				NPCID.DD2WitherBeastT2, NPCID.DD2WitherBeastT3, NPCID.DD2WyvernT1, NPCID.DD2WyvernT2, NPCID.DD2WyvernT3,
				NPCID.DD2DrakinT2, NPCID.DD2DrakinT3, NPCID.DD2DarkMageT1, NPCID.DD2DarkMageT3, NPCID.DD2AttackerTest,
				NPCID.DD2Betsy
			};
			var solarPillar = new HashSet<int> {
				NPCID.LunarTowerSolar, NPCID.SolarCrawltipedeBody, NPCID.SolarCrawltipedeTail, NPCID.SolarCrawltipedeHead,
				NPCID.SolarDrakomireRider, NPCID.SolarDrakomire, NPCID.SolarSpearman, NPCID.SolarSolenian, NPCID.SolarSroller,
				NPCID.SolarCorite, NPCID.SolarFlare, NPCID.SolarGoop
			};
			var nebulaPillar = new HashSet<int> {
				NPCID.NebulaBrain, NPCID.NebulaBeast, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier, NPCID.LunarTowerNebula
			};
			var vortexPillar = new HashSet<int> {
				NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexLarva, NPCID.VortexRifleman, NPCID.VortexSoldier, NPCID.LunarTowerVortex
			};
			var stardustPillar = new HashSet<int> {
				NPCID.StardustCellBig, NPCID.StardustCellSmall, NPCID.StardustJellyfishBig, NPCID.StardustJellyfishSmall, NPCID.StardustSoldier,
				NPCID.StardustSpiderBig, NPCID.StardustSpiderSmall, NPCID.StardustWormBody, NPCID.StardustWormHead, NPCID.StardustWormTail,
				NPCID.LunarTowerStardust
			};

			NPCGroupIdentityLibraries.VanillaBloodMoonTypes = new ReadOnlySet<int>( bloodMoon );
			NPCGroupIdentityLibraries.VanillaGoblinArmyTypes = new ReadOnlySet<int>( goblins );
			NPCGroupIdentityLibraries.VanillaFrostLegionTypes = new ReadOnlySet<int>( frostLegion );
			NPCGroupIdentityLibraries.VanillaPirateTypes = new ReadOnlySet<int>( pirates );
			NPCGroupIdentityLibraries.VanillaMartianTypes = new ReadOnlySet<int>( martians );
			NPCGroupIdentityLibraries.VanillaSolarEclipseTypes = new ReadOnlySet<int>( solarEclipse );
			NPCGroupIdentityLibraries.VanillaPumpkingMoonTypes = new ReadOnlySet<int>( pumpkinMoon );
			NPCGroupIdentityLibraries.VanillaFrostMoonTypes = new ReadOnlySet<int>( frostMoon );
			NPCGroupIdentityLibraries.VanillaOldOnesArmyTypes = new ReadOnlySet<int>( oldOnesArmy );
			NPCGroupIdentityLibraries.VanillaSolarPillarTypes = new ReadOnlySet<int>( solarPillar );
			NPCGroupIdentityLibraries.VanillaNebulaPillarTypes = new ReadOnlySet<int>( nebulaPillar );
			NPCGroupIdentityLibraries.VanillaVortexPillarTypes = new ReadOnlySet<int>( vortexPillar );
			NPCGroupIdentityLibraries.VanillaStardustPillarTypes = new ReadOnlySet<int>( stardustPillar );
		}
	}
}
