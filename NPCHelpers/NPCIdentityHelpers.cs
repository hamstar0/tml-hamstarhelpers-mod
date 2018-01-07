using HamstarHelpers.DotNetHelpers.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCIdentityHelpers {
		public static string GetUniqueId( NPC npc ) {
			string id = npc.TypeName;

			if( npc.HasGivenName ) { id = npc.GivenName + " " + id; }
			if( npc.modNPC != null ) { id = npc.modNPC.mod.Name + " " + id; }

			if( id != "" ) { return id; }
			return "" + npc.type;
		}


		[System.Obsolete( "use NPCHelpers.LooselyAssessThreat", true )]
		public static float LooselyAssessThreat( NPC npc ) {
			return NPCHelpers.LooselyAssessThreat( npc );
		}

		////////////////

		public static readonly ReadOnlySet<int> VanillaBloodMoonTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaBloodMoonTypes );
		public static readonly ReadOnlySet<int> VanillaGoblinArmyTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaGoblinArmyTypes );
		public static readonly ReadOnlySet<int> VanillaFrostLegionTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaFrostLegionTypes );
		public static readonly ReadOnlySet<int> VanillaPirateTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaPirateTypes );
		public static readonly ReadOnlySet<int> VanillaMartianTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaMartianTypes );
		public static readonly ReadOnlySet<int> VanillaSolarEclipseTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaSolarEclipseTypes );
		public static readonly ReadOnlySet<int> VanillaPumpkingMoonTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaPumpkingMoonTypes );
		public static readonly ReadOnlySet<int> VanillaFrostMoonTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaFrostMoonTypes );
		public static readonly ReadOnlySet<int> VanillaOldOnesArmyTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaOldOnesArmyTypes );
		public static readonly ReadOnlySet<int> VanillaSolarPillarTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaSolarPillarTypes );
		public static readonly ReadOnlySet<int> VanillaNebulaPillarTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaNebulaPillarTypes );
		public static readonly ReadOnlySet<int> VanillaVortexPillarTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaVortexPillarTypes );
		public static readonly ReadOnlySet<int> VanillaStardustPillarTypes = new ReadOnlySet<int>( NPCIdentityHelpers._VanillaStardustPillarTypes );

		private static ISet<int> _VanillaBloodMoonTypes = new HashSet<int> {
			NPCID.TheGroom, NPCID.TheBride, NPCID.BloodZombie, NPCID.Drippler,
			NPCID.CorruptBunny, NPCID.CorruptGoldfish, NPCID.CorruptPenguin,
			NPCID.CrimsonBunny, NPCID.CrimsonGoldfish, NPCID.CrimsonPenguin,
			NPCID.Clown
		};
		private static ISet<int> _VanillaGoblinArmyTypes = new HashSet<int> {
			NPCID.GoblinPeon, NPCID.GoblinSorcerer, NPCID.GoblinThief, NPCID.GoblinWarrior, NPCID.GoblinArcher, NPCID.GoblinSummoner
		};
		private static ISet<int> _VanillaFrostLegionTypes = new HashSet<int> {
			NPCID.MisterStabby, NPCID.SnowmanGangsta, NPCID.SnowBalla
		};
		private static ISet<int> _VanillaPirateTypes = new HashSet<int> {
			NPCID.PirateCaptain, NPCID.PirateCorsair, NPCID.PirateCrossbower, NPCID.PirateDeadeye, NPCID.PirateDeckhand, NPCID.Parrot,
			NPCID.PirateShip, NPCID.PirateShipCannon
		};
		private static ISet<int> _VanillaSolarEclipseTypes = new HashSet<int> {
			NPCID.Eyezor, NPCID.Frankenstein, NPCID.SwampThing, NPCID.Vampire, NPCID.CreatureFromTheDeep, NPCID.Fritz, NPCID.ThePossessed,
			NPCID.Mothron, NPCID.MothronEgg, NPCID.MothronSpawn, NPCID.Reaper,
			NPCID.Butcher, NPCID.DeadlySphere, NPCID.DrManFly, NPCID.Nailhead, NPCID.Psycho
		};
		private static ISet<int> _VanillaMartianTypes = new HashSet<int> {
			NPCID.BrainScrambler, NPCID.GrayGrunt, NPCID.RayGunner, NPCID.MartianOfficer, NPCID.MartianOfficer, NPCID.MartianEngineer,
			NPCID.GigaZapper, NPCID.MartianTurret, NPCID.MartianDrone, NPCID.MartianWalker, NPCID.Scutlix,
			NPCID.MartianSaucer, NPCID.MartianSaucerCannon, NPCID.MartianSaucerCore, NPCID.MartianSaucerTurret
		};
		private static ISet<int> _VanillaPumpkingMoonTypes = new HashSet<int> {
			NPCID.Scarecrow1, NPCID.Scarecrow2, NPCID.Scarecrow3, NPCID.Scarecrow4, NPCID.Scarecrow5, NPCID.Scarecrow6, NPCID.Scarecrow7,
			NPCID.Scarecrow8, NPCID.Scarecrow9, NPCID.Scarecrow10,
			NPCID.Splinterling, NPCID.Hellhound, NPCID.Poltergeist, NPCID.HeadlessHorseman,
			NPCID.MourningWood, NPCID.Pumpking, NPCID.PumpkingBlade
		};
		private static ISet<int> _VanillaFrostMoonTypes = new HashSet<int> {
			NPCID.PresentMimic, NPCID.Flocko, NPCID.GingerbreadMan, NPCID.Nutcracker, NPCID.NutcrackerSpinning, NPCID.Yeti,
			NPCID.ZombieElf, NPCID.ZombieElfGirl, NPCID.ElfArcher, NPCID.ElfCopter,
			NPCID.Krampus, NPCID.Everscream, NPCID.SantaNK1, NPCID.IceQueen
		};
		private static ISet<int> _VanillaOldOnesArmyTypes = new HashSet<int> {
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
		private static ISet<int> _VanillaSolarPillarTypes = new HashSet<int> {
			NPCID.LunarTowerSolar, NPCID.SolarCrawltipedeBody, NPCID.SolarCrawltipedeTail, NPCID.SolarCrawltipedeHead,
			NPCID.SolarDrakomireRider, NPCID.SolarDrakomire, NPCID.SolarSpearman, NPCID.SolarSolenian, NPCID.SolarSroller,
			NPCID.SolarCorite, NPCID.SolarFlare, NPCID.SolarGoop
		};
		private static ISet<int> _VanillaNebulaPillarTypes = new HashSet<int> {
			NPCID.NebulaBrain, NPCID.NebulaBeast, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier, NPCID.LunarTowerNebula
		};
		private static ISet<int> _VanillaVortexPillarTypes = new HashSet<int> {
			NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexLarva, NPCID.VortexRifleman, NPCID.VortexSoldier, NPCID.LunarTowerVortex
		};
		private static ISet<int> _VanillaStardustPillarTypes = new HashSet<int> {
			NPCID.StardustCellBig, NPCID.StardustCellSmall, NPCID.StardustJellyfishBig, NPCID.StardustJellyfishSmall, NPCID.StardustSoldier,
			NPCID.StardustSpiderBig, NPCID.StardustSpiderSmall, NPCID.StardustWormBody, NPCID.StardustWormHead, NPCID.StardustWormTail,
			NPCID.LunarTowerStardust
		};
	}
}
