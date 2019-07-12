using System.Collections.Generic;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to music box items.
	/// </summary>
	public class MusicBoxHelpers {
		/// <summary>
		/// Gets a set of all vanilla music box item ids.
		/// </summary>
		/// <returns></returns>
		public static ISet<int> GetVanillaMusicBoxItemIds() {
			return new HashSet<int> { ItemID.MusicBoxAltOverworldDay,
				ItemID.MusicBoxAltUnderground,
				ItemID.MusicBoxBoss1,
				ItemID.MusicBoxBoss2,
				ItemID.MusicBoxBoss3,
				ItemID.MusicBoxBoss4,
				ItemID.MusicBoxBoss5,
				ItemID.MusicBoxCorruption,
				ItemID.MusicBoxCrimson,
				ItemID.MusicBoxDD2,
				ItemID.MusicBoxDesert,
				ItemID.MusicBoxDungeon,
				ItemID.MusicBoxEclipse,
				ItemID.MusicBoxEerie,
				ItemID.MusicBoxFrostMoon,
				ItemID.MusicBoxGoblins,
				ItemID.MusicBoxHell,
				ItemID.MusicBoxIce,
				ItemID.MusicBoxJungle,
				ItemID.MusicBoxLunarBoss,
				ItemID.MusicBoxMartians,
				ItemID.MusicBoxMushrooms,
				ItemID.MusicBoxNight,
				ItemID.MusicBoxOcean,
				ItemID.MusicBoxOverworldDay,
				ItemID.MusicBoxPirates,
				ItemID.MusicBoxPlantera,
				ItemID.MusicBoxPumpkinMoon,
				ItemID.MusicBoxRain,
				ItemID.MusicBoxSandstorm,
				ItemID.MusicBoxSnow,
				ItemID.MusicBoxSpace,
				ItemID.MusicBoxTemple,
				ItemID.MusicBoxTheHallow,
				ItemID.MusicBoxTitle,
				ItemID.MusicBoxTowers,
				ItemID.MusicBoxUnderground,
				ItemID.MusicBoxUndergroundCorruption,
				ItemID.MusicBoxUndergroundCrimson,
				ItemID.MusicBoxUndergroundHallow };
		}

		/// <summary>
		/// Gets the "music type" (internal identifier) of a given vanilla music box by its item id.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static int GetMusicTypeOfVanillaMusicBox( int itemType ) {
			switch( itemType ) {
			case ItemID.MusicBoxOverworldDay:
				return 1;
			case ItemID.MusicBoxEerie:
				return 2;
			case ItemID.MusicBoxNight:
				return 3;
			case ItemID.MusicBoxUnderground:
				return 4;
			case ItemID.MusicBoxBoss1:
				return 5;
			case ItemID.MusicBoxTitle:
				return 6;
			case ItemID.MusicBoxJungle:
				return 7;
			case ItemID.MusicBoxCorruption:
				return 8;
			case ItemID.MusicBoxTheHallow:
				return 9;
			case ItemID.MusicBoxUndergroundCorruption:
				return 10;
			case ItemID.MusicBoxUndergroundHallow:
				return 11;
			case ItemID.MusicBoxBoss2:
				return 12;
			case ItemID.MusicBoxBoss3:
				return 13;
			case ItemID.MusicBoxSnow:
				return 14;
			case ItemID.MusicBoxSpace:
				return 15;
			case ItemID.MusicBoxCrimson:
				return 16;
			case ItemID.MusicBoxBoss4:
				return 17;
			case ItemID.MusicBoxAltOverworldDay:
				return 18;
			case ItemID.MusicBoxRain:
				return 19;
			case ItemID.MusicBoxIce:
				return 20;
			case ItemID.MusicBoxDesert:
				return 21;
			case ItemID.MusicBoxOcean:
				return 22;
			case ItemID.MusicBoxDungeon:
				return 23;
			case ItemID.MusicBoxPlantera:
				return 24;
			case ItemID.MusicBoxBoss5:
				return 25;
			case ItemID.MusicBoxTemple:
				return 26;
			case ItemID.MusicBoxEclipse:
				return 27;
			//return 28; Pumpkin moon?
			case ItemID.MusicBoxMushrooms:
				return 29;
			case ItemID.MusicBoxPumpkinMoon:
				return 30;
			case ItemID.MusicBoxAltUnderground:
				return 31;
			case ItemID.MusicBoxFrostMoon:
				return 32;
			case ItemID.MusicBoxUndergroundCrimson:
				return 33;
			case ItemID.MusicBoxTowers:
				return 34;
			case ItemID.MusicBoxPirates:
				return 35;
			case ItemID.MusicBoxHell:
				return 36;
			case ItemID.MusicBoxMartians:
				return 37;
			case ItemID.MusicBoxLunarBoss:
				return 38;
			case ItemID.MusicBoxGoblins:
				return 39;
			case ItemID.MusicBoxSandstorm:
				return 40;
			case ItemID.MusicBoxDD2:
				return 41;
			default:
				return 0;
			}
		}
	}
}
