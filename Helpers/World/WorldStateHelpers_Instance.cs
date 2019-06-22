using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.World {
	/** @private */
	public partial class WorldStateHelpers {
		private bool IsDay;
		private int HalfDaysElapsed;
		private long TicksElapsed;

		internal IDictionary<string, Action> DayHooks = new Dictionary<string, Action>();
		internal IDictionary<string, Action> NightHooks = new Dictionary<string, Action>();



		////////////////

		internal void Load( TagCompound tags ) {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			if( tags.ContainsKey( "world_id" ) ) {
				this.HalfDaysElapsed = tags.GetInt( "half_days_elapsed_" + myworld.ObsoleteId );
			}
		}

		internal void Save( TagCompound tags ) {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			tags["half_days_elapsed_" + myworld.ObsoleteId] = (int)this.HalfDaysElapsed;
		}

		////////////////
		
		internal void LoadFromData( int halfDays, string oldWorldId ) {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			this.HalfDaysElapsed = halfDays;

			myworld.ObsoleteId = oldWorldId;
		}


		////////////////

		internal void UpdateUponWorldBeingPlayed() {
			var mymod = ModHelpersMod.Instance;

			if( !LoadHelpers.IsWorldSafelyBeingPlayed() ) {
				this.IsDay = Main.dayTime;
			} else {
				if( this.IsDay != Main.dayTime ) {
					this.HalfDaysElapsed++;

					if( !this.IsDay ) {
						foreach( var kv in mymod.WorldStateHelpers.DayHooks ) { kv.Value(); }
					} else {
						foreach( var kv in mymod.WorldStateHelpers.NightHooks ) { kv.Value(); }
					}
				}

				this.IsDay = Main.dayTime;
			}

			this.TicksElapsed++;
		}
	}
}
