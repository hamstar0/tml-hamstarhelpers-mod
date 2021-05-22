using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;


namespace HamstarHelpers.Libraries.TModLoader {
	/// @private
	public partial class LoadLibraries {
		internal uint WorldStartupDelay = 0;

		internal bool IsLocalPlayerInGame_Hackish = false;
		internal bool HasServerBegunHavingPlayers_Hackish = false;



		////////////////

		internal LoadLibraries() {
			LoadHooks.AddWorldLoadEachHook( () => {
				this.WorldStartupDelay = 0;
			} );
			LoadHooks.AddWorldUnloadEachHook( () => {
				this.WorldStartupDelay = 0;
				this.IsLocalPlayerInGame_Hackish = false;
			} );
			LoadHooks.AddPostWorldUnloadEachHook( () => { // Redundant?
				this.WorldStartupDelay = 0;
				this.IsLocalPlayerInGame_Hackish = false;
			} );
		}

		////////////////

		internal void UpdateUponWorldBeingPlayed() {
			this.WorldStartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
