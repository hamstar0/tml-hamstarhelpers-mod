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
			var eventTypes = new List<VanillaInvasionType>();
			var invType = (VanillaInvasionType)NPCInvasionHelpers.GetEventTypeOfInvasionType( Main.invasionType );

			if( invType != VanillaInvasionType.None ) {
				eventTypes.Add( invType );
			}

			if( Sandstorm.Happening ) { eventTypes.Add( VanillaInvasionType.Sandstorm ); }
			if( Main.bloodMoon ) { eventTypes.Add( VanillaInvasionType.BloodMoon ); }
			if( Main.slimeRain ) { eventTypes.Add( VanillaInvasionType.SlimeRain ); }
			if( Main.eclipse ) { eventTypes.Add( VanillaInvasionType.SolarEclipse ); }
			if( Main.snowMoon ) { eventTypes.Add( VanillaInvasionType.FrostMoon ); }
			if( Main.pumpkinMoon ) { eventTypes.Add( VanillaInvasionType.PumpkinMoon ); }
			if( NPC.LunarApocalypseIsUp ) { eventTypes.Add( VanillaInvasionType.LunarApocalypse ); }

			return eventTypes;
		}
	}
}
