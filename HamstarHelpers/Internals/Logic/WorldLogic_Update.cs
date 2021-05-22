using HamstarHelpers.Libraries.TModLoader;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;


namespace HamstarHelpers.Internals.Logic {
	/// @private
	partial class WorldLogic {
		private void PreUpdateShared() {
			var mymod = ModHelpersMod.Instance;

			if( LoadLibraries.IsWorldLoaded() ) {
				mymod.LoadHooks.FulfillWorldLoadHooks();
			}

			if( LoadLibraries.IsWorldBeingPlayed() ) {
				mymod.LoadHooks.FulfillWorldInPlayHooks();
				mymod.LoadHelpers.UpdateUponWorldBeingPlayed();
				mymod.WorldStateHelpers.UpdateUponWorldBeingPlayed();
			}

			if( LoadLibraries.IsWorldSafelyBeingPlayed() ) {
				this.UpdateSafelyLoaded();
				mymod.LoadHooks.FulfillSafeWorldLoadHook();
			}
		}

		private void PreUpdateLocal() {
			var mymod = ModHelpersMod.Instance;
			mymod.AnimatedColors.Update();

			UIModControlPanelTab.UpdateModList();
		}


		////
		
		public void PreUpdateSingle() {
			this.PreUpdateShared();
			this.PreUpdateLocal();
		}

		public void PreUpdateClient() {
			this.PreUpdateShared();
			this.PreUpdateLocal();
		}
		
		public void PreUpdateServer() {
			this.PreUpdateShared();
		}


		////////////////

		private void UpdateSafelyLoaded() {
			var mymod = ModHelpersMod.Instance;
			mymod.ModLock.Update();
		}
	}
}
