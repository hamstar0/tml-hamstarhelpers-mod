using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.PromisedHooks;


namespace HamstarHelpers.Helpers.TModLoader {
	/// @private
	public partial class LoadHelpers {
		internal int StartupDelay = 0;

		internal bool IsClientPlaying_Hackish = false;
		internal bool HasServerBegunHavingPlayers_Hackish = false;



		////////////////

		internal LoadHelpers() {
			PromisedHooks.AddWorldLoadEachPromise( () => {
				this.StartupDelay = 0;
			} );
			PromisedHooks.AddWorldUnloadEachPromise( () => {
				this.StartupDelay = 0;
				this.IsClientPlaying_Hackish = false;
			} );
			PromisedHooks.AddPostWorldUnloadEachPromise( () => { // Redundant?
				this.StartupDelay = 0;
				this.IsClientPlaying_Hackish = false;
			} );
		}

		////////////////

		internal void UpdateUponWorldBeingPlayed() {
			this.StartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
