using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.WorldHelpers {
	public partial class WorldHelpers {
		public static string GetUniqueIdWithSeed() {
			return FileHelpers.SanitizePath( Main.worldName ) + "@" + Main.worldID + "." + Main.ActiveWorldFileData.Seed;
		}


		////////////////

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

		public static int GetElapsedPlayTime() {
			return (int)(ModHelpersMod.Instance.WorldHelpers.TicksElapsed / 60);
		}

		public static int GetElapsedHalfDays() {
			return ModHelpersMod.Instance.WorldHelpers.HalfDaysElapsed;
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
			ModHelpersMod.Instance.WorldHelpers.DayHooks[name] = callback;
		}

		public static void AddNightHook( string name, Action callback ) {
			ModHelpersMod.Instance.WorldHelpers.NightHooks[name] = callback;
		}



		////////////////

		private bool IsDay;
		private int HalfDaysElapsed;
		private long TicksElapsed;

		internal IDictionary<string, Action> DayHooks = new Dictionary<string, Action>();
		internal IDictionary<string, Action> NightHooks = new Dictionary<string, Action>();



		////////////////

		internal void Load( ModHelpersMod mymod, TagCompound tags ) {
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			if( tags.ContainsKey( "world_id" ) ) {
				this.HalfDaysElapsed = tags.GetInt( "half_days_elapsed_" + myworld.ObsoletedID );
			}
		}

		internal void Save( ModHelpersMod mymod, TagCompound tags ) {
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			tags.Set( "half_days_elapsed_" + myworld.ObsoletedID, (int)this.HalfDaysElapsed );
		}

		////////////////
		
		internal void LoadFromData( ModHelpersMod mymod, int half_days, string world_id ) {
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			this.HalfDaysElapsed = half_days;

			myworld.ObsoletedID = world_id;
		}


		////////////////

		internal void Update( ModHelpersMod mymod ) {
			if( !LoadHelpers.IsWorldSafelyBeingPlayed() ) {
				this.IsDay = Main.dayTime;
			} else {
				if( this.IsDay != Main.dayTime ) {
					this.HalfDaysElapsed++;

					if( !this.IsDay ) {
						foreach( var kv in mymod.WorldHelpers.DayHooks ) { kv.Value(); }
					} else {
						foreach( var kv in mymod.WorldHelpers.NightHooks ) { kv.Value(); }
					}
				}

				this.IsDay = Main.dayTime;
			}

			this.TicksElapsed++;
		}
	}
}
