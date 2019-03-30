using System;
using Terraria;
using Terraria.GameContent.Events;


namespace HamstarHelpers.Helpers.NPCHelpers {
	[Flags]
	public enum VanillaEventFlag {
		None = 1,
		Goblins = 2,
		FrostLegion = 4,
		Pirates = 8,
		Martians = 16,
		BloodMoon = 32,
		SlimeRain = 64,
		Sandstorm = 128,
		SolarEclipse = 256,
		PumpkinMoon = 512,
		FrostMoon = 1024,
		LunarApocalypse = 2048
	}




	public static partial class NPCInvasionHelpers {
		public static VanillaEventFlag GetEventTypeOfInvasionType( int which ) {
			switch( which ) {
			case 1:
				return VanillaEventFlag.Goblins;
			case 2:
				return VanillaEventFlag.FrostLegion;
			case 3:
				return VanillaEventFlag.Pirates;
			case 4:
				return VanillaEventFlag.Martians;
			default:
				return VanillaEventFlag.None;
			}
		}

		
		public static VanillaEventFlag GetCurrentEventTypeSet() {
			int flags = 0;
			int invasionEventType = (int)NPCInvasionHelpers.GetEventTypeOfInvasionType( Main.invasionType );

			if( ((VanillaEventFlag)invasionEventType & VanillaEventFlag.None) == 0 ) {
				flags |= invasionEventType;
			}

			if( Sandstorm.Happening ) { flags |= (int)VanillaEventFlag.Sandstorm; }
			if( Main.bloodMoon ) { flags |= (int)VanillaEventFlag.BloodMoon; }
			if( Main.slimeRain ) { flags |= (int)VanillaEventFlag.SlimeRain; }
			if( Main.eclipse ) { flags |= (int)VanillaEventFlag.SolarEclipse; }
			if( Main.snowMoon ) { flags |= (int)VanillaEventFlag.FrostMoon; }
			if( Main.pumpkinMoon ) { flags |= (int)VanillaEventFlag.PumpkinMoon; }
			if( NPC.LunarApocalypseIsUp ) { flags |= (int)VanillaEventFlag.LunarApocalypse; }

			return (VanillaEventFlag)flags;
		}
	}
}
