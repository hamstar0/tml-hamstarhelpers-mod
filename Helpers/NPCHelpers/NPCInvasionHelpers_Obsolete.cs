using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Events;


namespace HamstarHelpers.Helpers.NPCHelpers {
	[Obsolete( "use VanillaEventType" )]
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




	public static partial class NPCInvasionHelpers {
		[Obsolete( "use GetEventTypeOfInvasionType(int)", true )]
		public static VanillaInvasionType GetInvasionType( int which ) {
			return (VanillaInvasionType)NPCInvasionHelpers.GetEventTypeOfInvasionType( which );
		}


		[Obsolete( "use GetCurrentEventTypeSet()", true )]
		public static IList<VanillaInvasionType> GetCurrentEventTypes() {
			var event_types = new List<VanillaInvasionType>();
			var inv_type = (VanillaInvasionType)NPCInvasionHelpers.GetEventTypeOfInvasionType( Main.invasionType );

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
