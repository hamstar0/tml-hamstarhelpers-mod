using HamstarHelpers.DebugHelpers;


namespace HamstarHelpers.TmlHelpers {
	public partial class LoadHelpers {
		internal int StartupDelay = 0;

		internal bool IsClientPlaying = false;
		internal bool HasServerBegunHavingPlayers = false;



		////////////////

		internal LoadHelpers() { }

		////////////////

		internal void Update() {
			this.StartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
