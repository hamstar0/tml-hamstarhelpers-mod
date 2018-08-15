using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;


namespace HamstarHelpers.Helpers.TmlHelpers {
	public partial class LoadHelpers {
		internal int StartupDelay = 0;

		internal bool IsClientPlaying_Hackish = false;
		internal bool HasServerBegunHavingPlayers_Hackish = false;



		////////////////

		internal LoadHelpers() {
			Promises.AddWorldLoadEachPromise( () => {
				this.StartupDelay = 0;
			} );
			Promises.AddWorldUnloadEachPromise( () => {
				this.StartupDelay = 0;
				this.IsClientPlaying_Hackish = false;
			} );
			Promises.AddPostWorldUnloadEachPromise( () => {
				this.StartupDelay = 0;
				this.IsClientPlaying_Hackish = false;
			} );
		}

		////////////////

		internal void Update() {
			this.StartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
