using Terraria;


namespace HamstarHelpers.MiscHelpers {
	public static partial class InfoHelpers {
		public static string GetVanillaProgress() {
			if( NPC.downedMoonlord ) {
				return "Post Moon Lord";
			}
			if( NPC.LunarApocalypseIsUp ) {
				return "Lunar apocalypse happening";
			}
			if( NPC.downedGolemBoss ) {
				return "Post Golem";
			}
			if( NPC.downedPlantBoss ) {
				return "Post Plantera";
			}
			if( NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 ) {
				return "Post Mech Bosses";
			}
			if( Main.hardMode ) {
				return "Hard mode";
			}
			if( NPC.downedBoss2 ) {
				if( WorldGen.crimson ) {
					return "Post Eater of Worlds";
				} else {
					return "Post Brain of Cthuluhu";
				}
			}
			if( NPC.downedBoss3 ) {
				return "Post Skeletron";
			}
			if( NPC.downedBoss1 || NPC.downedSlimeKing ) {
				return "Boss killing begun";
			}
			if( NPC.downedGoblins ) {
				return "Goblins killed";
			}
			return "Start";
		}
	}
}
