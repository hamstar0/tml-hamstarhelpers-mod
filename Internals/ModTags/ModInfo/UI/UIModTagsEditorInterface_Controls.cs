using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditorInterface : UIModTagsInterface {
		public override void RefreshButtonEnableStates() {
			base.RefreshButtonEnableStates();
			this.FinishButton.RefreshEnableState();
		}


		////////////////

		public void EnableSubmitOption() {
			this.FinishButton.SetModeSubmit();
		}

		public void DisableSubmitOption() {
			this.FinishButton.SetModeReadOnly();
		}

		////////////////

		public void LockFinishButton() {
			this.FinishButton.Lock();
		}

		public void UnlockFinishButton() {
			this.FinishButton.Unlock();
		}


		////////////////

		public void ResetUIState( bool isRecentlyTagged ) {
			if( !isRecentlyTagged ) {
				this.UnlockFinishButton();
			} else {
				this.LockFinishButton();
			}

			this.ResetTagButtons( true );
		}
	}
}
