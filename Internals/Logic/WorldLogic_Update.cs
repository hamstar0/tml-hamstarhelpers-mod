using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.Helpers.TmlHelpers;


namespace HamstarHelpers.Internals.Logic {
	partial class WorldLogic {
		private void PreUpdateShared( ModHelpersMod mymod ) {
			if( LoadHelpers.IsWorldLoaded() ) {
				mymod.Promises.FulfillWorldLoadPromises();
			}

			if( LoadHelpers.IsWorldBeingPlayed() ) {
				mymod.Promises.FulfillWorldInPlayPromises();
				mymod.LoadHelpers.Update();
				mymod.WorldHelpers.Update( mymod );
			}

			if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
				this.UpdateSafelyLoaded( mymod );
				mymod.Promises.FulfillSafeWorldLoadPromises();
			}
		}

		private void PreUpdateLocal( ModHelpersMod mymod ) {
			mymod.AnimatedColors.Update();

			UIControlPanel.UpdateModList( mymod );
		}


		////
		
		public void PreUpdateSingle( ModHelpersMod mymod ) {
			this.PreUpdateShared( mymod );
			this.PreUpdateLocal( mymod );
		}

		public void PreUpdateClient( ModHelpersMod mymod ) {
			this.PreUpdateShared( mymod );
			this.PreUpdateLocal( mymod );
		}
		
		public void PreUpdateServer( ModHelpersMod mymod ) {
			this.PreUpdateShared( mymod );
		}


		////////////////
		
		private void UpdateSafelyLoaded( ModHelpersMod mymod ) {
			mymod.ModLockHelpers.Update();
		}
	}
}
