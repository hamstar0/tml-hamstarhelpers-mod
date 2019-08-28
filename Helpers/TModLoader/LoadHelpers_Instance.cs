using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;


namespace HamstarHelpers.Helpers.TModLoader {
	/// @private
	public partial class LoadHelpers {
		internal uint WorldStartupDelay = 0;

		internal bool IsClientPlaying_Hackish = false;
		internal bool HasServerBegunHavingPlayers_Hackish = false;



		////////////////

		internal LoadHelpers() {
			LoadHooks.AddWorldLoadEachHook( () => {
				this.WorldStartupDelay = 0;
			} );
			LoadHooks.AddWorldUnloadEachHook( () => {
				this.WorldStartupDelay = 0;
				this.IsClientPlaying_Hackish = false;
			} );
			LoadHooks.AddPostWorldUnloadEachHook( () => { // Redundant?
				this.WorldStartupDelay = 0;
				this.IsClientPlaying_Hackish = false;
			} );
		}

		////////////////

		internal void UpdateUponWorldBeingPlayed() {
			this.WorldStartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
