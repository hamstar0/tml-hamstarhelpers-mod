using HamstarHelpers.Helpers.TModLoader;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.World {
	/// @private
	public partial class WorldStateHelpers {
		private bool IsDay;
		private int HalfDaysElapsed;
		private long TicksElapsed;



		////////////////

		internal void Load( TagCompound tags ) {
			var myworld = ModContent.GetInstance<ModHelpersWorld>();

			if( tags.ContainsKey( "world_id" ) ) {
				this.HalfDaysElapsed = tags.GetInt( "half_days_elapsed_" + myworld.ObsoleteId );
			}
		}

		internal void Save( TagCompound tags ) {
			var myworld = ModContent.GetInstance<ModHelpersWorld>();

			tags["half_days_elapsed_" + myworld.ObsoleteId] = (int)this.HalfDaysElapsed;
		}

		////////////////
		
		internal void LoadFromData( int halfDays, string oldWorldId ) {
			var myworld = ModContent.GetInstance<ModHelpersWorld>();

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
				}

				this.IsDay = Main.dayTime;
			}

			this.TicksElapsed++;
		}
	}
}
