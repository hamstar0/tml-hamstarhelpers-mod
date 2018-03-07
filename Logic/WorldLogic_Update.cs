using HamstarHelpers.ControlPanel;
using HamstarHelpers.TmlHelpers;
using Terraria;


namespace HamstarHelpers.Logic {
	partial class WorldLogic {
		public void PreUpdateNotServer( HamstarHelpersMod mymod ) {
			this.PreUpdateServer( mymod );

			mymod.AnimatedColors.Update();

			ControlPanelUI.UpdateModList( mymod );
		}


		public void PreUpdateServer( HamstarHelpersMod mymod ) {
			if( !TmlLoadHelpers.IsWorldBeingPlayed() ) {
				return;
			}

			mymod.TmlLoadHelpers.Update();
			mymod.WorldHelpers.Update( mymod );

			// Simply idle until ready (seems needed)
			if( TmlLoadHelpers.IsWorldSafelyBeingPlayed() ) {
				this.UpdateLoaded( mymod );
			}
		}


		////////////////
		
		private void UpdateLoaded( HamstarHelpersMod mymod ) {
			mymod.ModLockHelpers.Update();
			
			AltProjectileInfo.UpdateAll();
			AltNPCInfo.UpdateAll();
		}
	}
}
