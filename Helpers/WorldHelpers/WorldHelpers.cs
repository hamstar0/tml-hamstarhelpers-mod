using HamstarHelpers.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.WorldHelpers {
	public enum VanillaBiomes {
		Forest, Space, Ocean, Cave, Hell,
		Desert, Cold, Mushroom, Jungle, Corruption, Crimson, Hallow,
		Granite, Marble, SpiderNest, Dungeon, Temple
	}
	public enum VanillaSectionalBiomes {
		Forest, Space, Ocean, Cave, Hell
	}
	public enum VanillaSurfaceBiomes {
		Forest, Desert, Cold, Mushroom, Jungle, Ocean, Space, Corruption, Crimson, Hallow
	}
	public enum VanillaUndergroundBiomes {
		Cave, Desert, Cold, Mushroom, Granite, Marble, SpiderNest, Dungeon, Jungle, Temple, Corruption, Crimson, Hallow, Hell
	}
	public enum VanillaHardModeSurfaceBiomes {
		Corruption, Crimson, Hallow
	}
	public enum VanillaHardModeUndergroundBiomes {
		Temple, Corruption, Crimson, Hallow
	}
	public enum VanillaHardModeConvertibleBiomes {
		Cave, Desert, Cold
	}


	public enum WorldSize {
		SubSmall,
		Small,
		Medium,
		Large,
		SuperLarge
	}



	public class WorldHelpers {
		public readonly static int VanillaDayDuration = 54000;
		public readonly static int VanillaNightDuration = 32400;


		////////////////

		internal IDictionary<string, Action> DayHooks = new Dictionary<string, Action>();
		internal IDictionary<string, Action> NightHooks = new Dictionary<string, Action>();

		
		////////////////
		
		public static string GetUniqueId() {
			return FileHelpers.SanitizePath( Main.worldName) + ":" + Main.worldID;
		}


		public static WorldSize GetSize() {
			int size = Main.maxTilesX * Main.maxTilesY;

			if( size <= ( 4200 * 1200 ) / 2 ) {
				return WorldSize.SubSmall;
			} else if( size <= 4200 * 1200 + 1000 ) {
				return WorldSize.Small;
			} else if( size <= 6400 * 1800 + 1000 ) {
				return WorldSize.Medium;
			} else if( size <= 8400 * 2400 + 1000 ) {
				return WorldSize.Large;
			} else {
				return WorldSize.SuperLarge;
			}
		}


		////////////////

		public static bool IsBeingInvaded() {
			return Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0;
		}

		////////////////

		public static int GetElapsedHalfDays() {
			var myworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();
			return myworld.WorldLogic.HalfDaysElapsed;
		}

		public static double GetDayOrNightPercentDone() {
			if( Main.dayTime ) {
				return Main.time / (double)WorldHelpers.VanillaDayDuration;
			} else {
				return Main.time / (double)WorldHelpers.VanillaNightDuration;
			}
		}

		////////////////

		public static bool IsAboveWorldSurface( Vector2 world_pos ) {
			return world_pos.Y < (Main.worldSurface * 16);
		}

		public static bool IsWithinUnderworld( Vector2 world_pos ) {
			return world_pos.Y > ((Main.maxTilesY - 200) * 16);
		}


		////////////////
		
		public static void AddDayHook( string name, Action callback ) {
			HamstarHelpersMod.Instance.WorldHelpers.DayHooks[name] = callback;
		}

		public static void AddNightHook( string name, Action callback ) {
			HamstarHelpersMod.Instance.WorldHelpers.NightHooks[name] = callback;
		}
	}
}
