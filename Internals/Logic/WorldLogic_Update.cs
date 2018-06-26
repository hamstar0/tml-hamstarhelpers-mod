using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.TmlHelpers;


namespace HamstarHelpers.Internals.Logic {
	partial class WorldLogic {
		private void PreUpdate( HamstarHelpersMod mymod ) {
			if( LoadHelpers.IsWorldLoaded() ) {
				mymod.Promises.FulfillWorldLoadPromises();
			}

			if( LoadHelpers.IsWorldBeingPlayed() ) {
				mymod.LoadHelpers.PostWorldLoadUpdate();
				mymod.WorldHelpers.Update( mymod );

				// Simply idle until ready (seems needed)
				if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
					this.UpdateSafelyLoaded( mymod );
				}
			}
		}

		////

		private void PreUpdatePlayer( HamstarHelpersMod mymod ) {
			this.PreUpdate( mymod );

			mymod.AnimatedColors.Update();

			UIControlPanel.UpdateModList( mymod );
		}


		public void PreUpdateSingle( HamstarHelpersMod mymod ) {
			this.PreUpdatePlayer( mymod );
		}

		public void PreUpdateClient( HamstarHelpersMod mymod ) {
			this.PreUpdatePlayer( mymod );
		}
		
		public void PreUpdateServer( HamstarHelpersMod mymod ) {
			this.PreUpdate( mymod );
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
