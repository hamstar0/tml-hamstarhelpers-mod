using HamstarHelpers.Helpers;
using HamstarHelpers.NetProtocols;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.WorldHelpers {
	public partial class WorldHelpers {
		public static string GetUniqueId() {
			return WorldHelpers.GetUniqueId( false );
		}

		public static string GetUniqueId( bool as_file_name ) {
			if( as_file_name ) {
				return FileHelpers.SanitizePath( Main.worldName ) + "@" + Main.worldID;
			} else {
				return FileHelpers.SanitizePath( Main.worldName ) + ":" + Main.worldID;
			}
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
			return (int)(HamstarHelpersMod.Instance.WorldHelpers.TicksElapsed / 60);
		}

		public static int GetElapsedHalfDays() {
			return HamstarHelpersMod.Instance.WorldHelpers.HalfDaysElapsed;
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



		////////////////

		private bool IsDay;
		private int HalfDaysElapsed;
		private long TicksElapsed;

		internal IDictionary<string, Action> DayHooks = new Dictionary<string, Action>();
		internal IDictionary<string, Action> NightHooks = new Dictionary<string, Action>();


		////////////////

		internal void Load( HamstarHelpersMod mymod, TagCompound tags ) {
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( tags.ContainsKey( "world_id" ) ) {
				this.HalfDaysElapsed = tags.GetInt( "half_days_elapsed_" + myworld.ObsoleteID );
			}
		}

		internal void Save( HamstarHelpersMod mymod, TagCompound tags ) {
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();

			tags.Set( "half_days_elapsed_" + myworld.ObsoleteID, (int)this.HalfDaysElapsed );
		}

		////////////////

		internal void SaveForNetwork( HHModDataProtocol protocol ) {
			protocol.HalfDays = this.HalfDaysElapsed;
		}

		internal void LoadFromNetwork( int half_days ) {
			this.HalfDaysElapsed = half_days;
		}


		////////////////

		internal void Update( HamstarHelpersMod mymod ) {
			if( !TmlLoadHelpers.IsWorldSafelyBeingPlayed() ) {
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
