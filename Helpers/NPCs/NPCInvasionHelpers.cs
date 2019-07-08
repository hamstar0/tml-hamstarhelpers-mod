using System;
using Terraria;
using Terraria.GameContent.Events;


namespace HamstarHelpers.Helpers.NPCs {
	/// <summary></summary>
	[Flags]
	public enum VanillaEventFlag {
		/// <summary></summary>
		None = 1,
		/// <summary></summary>
		Goblins = 2,
		/// <summary></summary>
		FrostLegion = 4,
		/// <summary></summary>
		Pirates = 8,
		/// <summary></summary>
		Martians = 16,
		/// <summary></summary>
		BloodMoon = 32,
		/// <summary></summary>
		SlimeRain = 64,
		/// <summary></summary>
		Sandstorm = 128,
		/// <summary></summary>
		SolarEclipse = 256,
		/// <summary></summary>
		PumpkinMoon = 512,
		/// <summary></summary>
		FrostMoon = 1024,
		/// <summary></summary>
		LunarApocalypse = 2048
	}




	/// <summary>
	/// Assorted static "helper" functions pertaining to NPC invasions
	/// </summary>
	public static partial class NPCInvasionHelpers {
		/// <summary>
		/// Gets an enum flag type representing a given invasion type (internal code).
		/// </summary>
		/// <param name="which"></param>
		/// <returns></returns>
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

		
		/// <summary>
		/// Gets an OR'd enum flag representing each vanilla event in session.
		/// </summary>
		/// <returns></returns>
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
