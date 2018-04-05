using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Events;


namespace HamstarHelpers.NPCHelpers {
	public enum VanillaInvasionType {
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


		[System.Obsolete( "use NPCInvasionHelpers.GetCurrentEventTypes", true )]
		public static VanillaInvasionType GetCurrentInvasionType() {
			VanillaInvasionType inv_type = NPCInvasionHelpers.GetInvasionType( Main.invasionType );

			if( inv_type == VanillaInvasionType.None ) {
				if( Main.snowMoon ) { return VanillaInvasionType.FrostMoon; }
				if( Main.pumpkinMoon ) { return VanillaInvasionType.PumpkinMoon; }
				if( Main.eclipse ) { return VanillaInvasionType.SolarEclipse; }
				if( Main.slimeRain ) { return VanillaInvasionType.SlimeRain; }
				if( NPC.LunarApocalypseIsUp ) { return VanillaInvasionType.LunarApocalypse; }
				if( Main.bloodMoon ) { return VanillaInvasionType.BloodMoon; }
				if( Sandstorm.Happening ) { return VanillaInvasionType.Sandstorm; }
			}
			return inv_type;
		}


		public static IList<VanillaInvasionType> GetCurrentEventTypes() {
			var event_types = new List<VanillaInvasionType>();
			var inv_type = NPCInvasionHelpers.GetInvasionType( Main.invasionType );

			if( inv_type != VanillaInvasionType.None ) {
				event_types.Add( inv_type );
			}

			if( Sandstorm.Happening ) { event_types.Add( VanillaInvasionType.Sandstorm ); }
			if( Main.bloodMoon ) { event_types.Add( VanillaInvasionType.BloodMoon ); }
			if( Main.slimeRain ) { event_types.Add( VanillaInvasionType.SlimeRain ); }
			if( Main.eclipse ) { event_types.Add( VanillaInvasionType.SolarEclipse ); }
			if( Main.snowMoon ) { event_types.Add( VanillaInvasionType.FrostMoon ); }
			if( Main.pumpkinMoon ) { event_types.Add( VanillaInvasionType.PumpkinMoon ); }
			if( NPC.LunarApocalypseIsUp ) { event_types.Add( VanillaInvasionType.LunarApocalypse ); }

			return event_types;
		}
	}
}
