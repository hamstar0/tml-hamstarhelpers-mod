using HamstarHelpers.Helpers.TmlHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.WorldHelpers {
	public partial class WorldStateHelpers {
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
		
		internal void LoadFromData( ModHelpersMod mymod, int halfDays, string worldId ) {
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			this.HalfDaysElapsed = halfDays;

			myworld.ObsoletedID = worldId;
		}


		////////////////

		internal void Update( ModHelpersMod mymod ) {
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
