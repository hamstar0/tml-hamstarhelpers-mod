using Terraria;
using Terraria.GameContent.Events;


namespace HamstarHelpers.NPCHelpers {
	public enum VanillaInvasionType {
		None = -1,
		Goblins = 1,
		FrostLegion = 2,
		Pirates = 3,
		Martians = 4,
		BloodMoon = 1000,
		SlimeRain,
		Sandstorm,
		SolarEclipse,
		PumpkinMoon,
		FrostMoon,
		LunarApocalypse
	}



	public static class NPCInvasionHelpers {
		public static VanillaInvasionType GetInvasionType( int which ) {
			switch( which ) {
			case 1:
				return VanillaInvasionType.Goblins;
			case 2:
				return VanillaInvasionType.FrostLegion;
			case 3:
				return VanillaInvasionType.Pirates;
			case 4:
				return VanillaInvasionType.Martians;
			default:
				return VanillaInvasionType.None;
			}
		}


		public static VanillaInvasionType GetCurrentInvasionType() {
			VanillaInvasionType inv_type = NPCInvasionHelpers.GetInvasionType( Main.invasionType );

			if( inv_type == VanillaInvasionType.None ) {
				if( Main.snowMoon ) { return VanillaInvasionType.FrostMoon; }
				if( Main.pumpkinMoon ) { return VanillaInvasionType.PumpkinMoon; }
				if( Main.eclipse ) { return VanillaInvasionType.SolarEclipse; }
				if( NPC.LunarApocalypseIsUp ) { return VanillaInvasionType.LunarApocalypse; }
				if( Main.bloodMoon ) { return VanillaInvasionType.BloodMoon; }
				if( Main.slimeRain ) { return VanillaInvasionType.SlimeRain; }
				if( Sandstorm.Happening ) { return VanillaInvasionType.Sandstorm; }
			}
			return inv_type;
		}
	}
}
