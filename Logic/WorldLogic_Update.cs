using HamstarHelpers.TmlHelpers;
using Terraria;


namespace HamstarHelpers.Logic {
	partial class WorldLogic {
		public void Update( HamstarHelpersMod mymod ) {
			if( !TmlWorldHelpers.IsGameLoaded() ) {
				return;
			}

			if( this.IsPlaying() ) {
				mymod.ControlPanel.UpdateModList();
			}

			mymod.AnimatedColors.Update();

			// Simply idle until ready (seems needed)
			if( !this.IsFullyReady() ) {
				this.IsDay = Main.dayTime;
				return;
			} else {
				this.UpdateLoaded( mymod );
			}
		}


		private void UpdateLoaded( HamstarHelpersMod mymod ) {
			this.UpdateDay( mymod );

			mymod.ModLockHelpers.Update();
			
			AltProjectileInfo.UpdateAll();
			AltNPCInfo.UpdateAll();
		}

		////////////////

		private void UpdateDay( HamstarHelpersMod mymod ) {
			if( this.IsDay != Main.dayTime ) {
				this.HalfDaysElapsed++;

				if( !this.IsDay ) {
					foreach( var kv in mymod.WorldHelpers.DayHooks ) { kv.Value(); }
				} else {
					foreach( var kv in mymod.WorldHelpers.NightHooks ) { kv.Value(); }
				}
			}
			this.IsDay = Main.dayTime;
		}
	}
}
