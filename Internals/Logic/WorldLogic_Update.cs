using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.Helpers.TmlHelpers;


namespace HamstarHelpers.Internals.Logic {
	partial class WorldLogic {
		private void PreUpdateShared( HamstarHelpersMod mymod ) {
			if( LoadHelpers.IsWorldLoaded() ) {
				mymod.Promises.FulfillWorldLoadPromises();
			}

			if( LoadHelpers.IsWorldBeingPlayed() ) {
				mymod.LoadHelpers.Update();
				mymod.WorldHelpers.Update( mymod );

				// Simply idle until ready (seems needed)
				if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
					this.UpdateSafelyLoaded( mymod );
				}
			}
		}

		private void PreUpdateLocal( HamstarHelpersMod mymod ) {
			mymod.AnimatedColors.Update();

			UIControlPanel.UpdateModList( mymod );
		}

		////


		public void PreUpdateSingle( HamstarHelpersMod mymod ) {
			this.PreUpdateShared( mymod );
			this.PreUpdateLocal( mymod );
		}

		public void PreUpdateClient( HamstarHelpersMod mymod ) {
			this.PreUpdateShared( mymod );
			this.PreUpdateLocal( mymod );
		}
		
		public void PreUpdateServer( HamstarHelpersMod mymod ) {
			this.PreUpdateShared( mymod );
		}


		////////////////
		
		private void UpdateSafelyLoaded( HamstarHelpersMod mymod ) {
			mymod.ModLockHelpers.Update();
		}
	}
}
