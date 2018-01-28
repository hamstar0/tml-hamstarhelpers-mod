using HamstarHelpers.DotNetHelpers.DataStructures;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.NPCHelpers {
	public class NPCIdentityHelpers {
		public static string GetUniqueId( NPC npc ) {
			string id = npc.TypeName;

			if( npc.HasGivenName ) { id = npc.GivenName + " " + id; }
			if( npc.modNPC != null ) { id = npc.modNPC.mod.Name + " " + id; }

			if( id != "" ) { return id; }
			return "" + npc.type;
		}
		
		// TODO: GetVanillaSnapshotHash


		////////////////

		public static readonly ReadOnlySet<int> VanillaBloodMoonTypes;
		public static readonly ReadOnlySet<int> VanillaGoblinArmyTypes;
		public static readonly ReadOnlySet<int> VanillaFrostLegionTypes;
		public static readonly ReadOnlySet<int> VanillaPirateTypes;
		public static readonly ReadOnlySet<int> VanillaMartianTypes;
		public static readonly ReadOnlySet<int> VanillaSolarEclipseTypes;
		public static readonly ReadOnlySet<int> VanillaPumpkingMoonTypes;
		public static readonly ReadOnlySet<int> VanillaFrostMoonTypes;
		public static readonly ReadOnlySet<int> VanillaOldOnesArmyTypes;
		public static readonly ReadOnlySet<int> VanillaSolarPillarTypes;
		public static readonly ReadOnlySet<int> VanillaNebulaPillarTypes;
		public static readonly ReadOnlySet<int> VanillaVortexPillarTypes;
		public static readonly ReadOnlySet<int> VanillaStardustPillarTypes;



		static NPCIdentityHelpers() {
			var blood_moon = new HashSet<int> {
				NPCID.TheGroom, NPCID.TheBride, NPCID.BloodZombie, NPCID.Drippler,
				NPCID.CorruptBunny, NPCID.CorruptGoldfish, NPCID.CorruptPenguin,
				NPCID.CrimsonBunny, NPCID.CrimsonGoldfish, NPCID.CrimsonPenguin,
				NPCID.Clown
			};
			var goblins = new HashSet<int> {
				NPCID.GoblinPeon, NPCID.GoblinSorcerer, NPCID.GoblinThief, NPCID.GoblinWarrior, NPCID.GoblinArcher, NPCID.GoblinSummoner
			};
			var frost_legion = new HashSet<int> {
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
			var solar_eclipse = new HashSet<int> {
				NPCID.Eyezor, NPCID.Frankenstein, NPCID.SwampThing, NPCID.Vampire, NPCID.CreatureFromTheDeep, NPCID.Fritz, NPCID.ThePossessed,
				NPCID.Mothron, NPCID.MothronEgg, NPCID.MothronSpawn, NPCID.Reaper,
				NPCID.Butcher, NPCID.DeadlySphere, NPCID.DrManFly, NPCID.Nailhead, NPCID.Psycho
			};
			var pumpkin_moon = new HashSet<int> {
				NPCID.Scarecrow1, NPCID.Scarecrow2, NPCID.Scarecrow3, NPCID.Scarecrow4, NPCID.Scarecrow5, NPCID.Scarecrow6, NPCID.Scarecrow7,
				NPCID.Scarecrow8, NPCID.Scarecrow9, NPCID.Scarecrow10,
				NPCID.Splinterling, NPCID.Hellhound, NPCID.Poltergeist, NPCID.HeadlessHorseman,
				NPCID.MourningWood, NPCID.Pumpking, NPCID.PumpkingBlade
			};
			var frost_moon = new HashSet<int> {
				NPCID.PresentMimic, NPCID.Flocko, NPCID.GingerbreadMan, NPCID.Nutcracker, NPCID.NutcrackerSpinning, NPCID.Yeti,
				NPCID.ZombieElf, NPCID.ZombieElfGirl, NPCID.ElfArcher, NPCID.ElfCopter,
				NPCID.Krampus, NPCID.Everscream, NPCID.SantaNK1, NPCID.IceQueen
			};
			var old_ones_army = new HashSet<int> {
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
			var solar_pillar = new HashSet<int> {
				NPCID.LunarTowerSolar, NPCID.SolarCrawltipedeBody, NPCID.SolarCrawltipedeTail, NPCID.SolarCrawltipedeHead,
				NPCID.SolarDrakomireRider, NPCID.SolarDrakomire, NPCID.SolarSpearman, NPCID.SolarSolenian, NPCID.SolarSroller,
				NPCID.SolarCorite, NPCID.SolarFlare, NPCID.SolarGoop
			};
			var nebula_pillar = new HashSet<int> {
				NPCID.NebulaBrain, NPCID.NebulaBeast, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier, NPCID.LunarTowerNebula
			};
			var vortex_pillar = new HashSet<int> {
				NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexLarva, NPCID.VortexRifleman, NPCID.VortexSoldier, NPCID.LunarTowerVortex
			};
			var stardust_pillar = new HashSet<int> {
				NPCID.StardustCellBig, NPCID.StardustCellSmall, NPCID.StardustJellyfishBig, NPCID.StardustJellyfishSmall, NPCID.StardustSoldier,
				NPCID.StardustSpiderBig, NPCID.StardustSpiderSmall, NPCID.StardustWormBody, NPCID.StardustWormHead, NPCID.StardustWormTail,
				NPCID.LunarTowerStardust
			};

			NPCIdentityHelpers.VanillaBloodMoonTypes = new ReadOnlySet<int>( blood_moon );
			NPCIdentityHelpers.VanillaGoblinArmyTypes = new ReadOnlySet<int>( goblins );
			NPCIdentityHelpers.VanillaFrostLegionTypes = new ReadOnlySet<int>( frost_legion );
			NPCIdentityHelpers.VanillaPirateTypes = new ReadOnlySet<int>( pirates );
			NPCIdentityHelpers.VanillaMartianTypes = new ReadOnlySet<int>( martians );
			NPCIdentityHelpers.VanillaSolarEclipseTypes = new ReadOnlySet<int>( solar_eclipse );
			NPCIdentityHelpers.VanillaPumpkingMoonTypes = new ReadOnlySet<int>( pumpkin_moon );
			NPCIdentityHelpers.VanillaFrostMoonTypes = new ReadOnlySet<int>( frost_moon );
			NPCIdentityHelpers.VanillaOldOnesArmyTypes = new ReadOnlySet<int>( old_ones_army );
			NPCIdentityHelpers.VanillaSolarPillarTypes = new ReadOnlySet<int>( solar_pillar );
			NPCIdentityHelpers.VanillaNebulaPillarTypes = new ReadOnlySet<int>( nebula_pillar );
			NPCIdentityHelpers.VanillaVortexPillarTypes = new ReadOnlySet<int>( vortex_pillar );
			NPCIdentityHelpers.VanillaStardustPillarTypes = new ReadOnlySet<int>( stardust_pillar );
		}


		////////////////

		public static IReadOnlyDictionary<string, int> NamesToIds {
			get { return HamstarHelpersMod.Instance.NPCIdentityHelpers._NamesToIds; }
		}



		////////////////

		private IDictionary<string, int> __namesToIds = new Dictionary<string, int>();
		private IReadOnlyDictionary<string, int> _NamesToIds = null;


		////////////////

		internal void OnPostSetupContent() {
			for( int i = 1; i < NPCLoader.NPCCount; i++ ) {
				string name = Lang.GetNPCNameValue( i );
				this.__namesToIds[name] = i;
			}

			this._NamesToIds = new ReadOnlyDictionary<string, int>( this.__namesToIds );
		}


		////////////////

		[System.Obsolete( "use NPCHelpers.LooselyAssessThreat", true )]
		public static float LooselyAssessThreat( NPC npc ) {
			return NPCHelpers.LooselyAssessThreat( npc );
		}
	}
}
