using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.TmlHelpers;


namespace HamstarHelpers.Internals.Logic {
	partial class WorldLogic {
		private void PreUpdateShared( HamstarHelpersMod mymod ) {
			if( LoadHelpers.IsWorldLoaded() ) {
				mymod.Promises.FulfillWorldLoadPromises();
			}

			if( LoadHelpers.IsWorldBeingPlayed() ) {
				mymod.LoadHelpers.Update();
				mymod.WorldHelpers.Update( mymod );
			}

			if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
				this.UpdateSafelyLoaded( mymod );
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

#pragma warning disable 612, 618
			AltProjectileInfo.UpdateAll();
			AltNPCInfo.UpdateAll();
#pragma warning restore 612, 618
		}
	}
}
