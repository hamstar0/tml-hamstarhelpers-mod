using HamstarHelpers.ControlPanel;
using HamstarHelpers.TmlHelpers;


namespace HamstarHelpers.Logic {
	partial class WorldLogic {
		private void PreUpdate( HamstarHelpersMod mymod ) {
			if( TmlLoadHelpers.IsWorldLoaded() ) {
				mymod.TmlLoadHelpers.FulfillWorldLoadPromises();
			}

			if( !TmlLoadHelpers.IsWorldBeingPlayed() ) {
				return;
			}

			mymod.TmlLoadHelpers.PostWorldLoadUpdate();
			mymod.WorldHelpers.Update( mymod );

			// Simply idle until ready (seems needed)
			if( TmlLoadHelpers.IsWorldSafelyBeingPlayed() ) {
				this.UpdateLoaded( mymod );
			}
		}


		public void PreUpdateSingle( HamstarHelpersMod mymod ) {
			this.PreUpdate( mymod );

			mymod.AnimatedColors.Update();

			UIControlPanel.UpdateModList( mymod );
		}


		public void PreUpdateServer( HamstarHelpersMod mymod ) {
			this.PreUpdate( mymod );
		}


		////////////////
		
		private void UpdateLoaded( HamstarHelpersMod mymod ) {
			mymod.ModLockHelpers.Update();

#pragma warning disable 612, 618
			AltProjectileInfo.UpdateAll();
			AltNPCInfo.UpdateAll();
#pragma warning restore 612, 618
		}
	}
}
