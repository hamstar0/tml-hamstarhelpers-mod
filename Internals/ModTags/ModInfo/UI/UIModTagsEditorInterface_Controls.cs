using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditorInterface : UIModTagsInterface {
		public override void RefreshButtonEnableStates() {
			base.RefreshButtonEnableStates();

			if( this.EditButton.IsEditMode ) {
				this.EditButton.RefreshEnableState();
			} else {
				if( this.Manager.GetTagsWithGivenState( 1 ).Count > 0 ) {
					this.Manager.SetInfoTextDefault( "Do these tags look incorrect? If so, modify them." );
					this.UnlockEditButton();
				} else {
					this.Manager.SetInfoTextDefault( "No tags set for this mod. Why not add some?" );
					this.EditButton.SetReadOnlyModeForButton();
					this.LockEditButton();
				}
			}
		}


		////////////////

		public void LockEditButton() {
			this.EditButton.LockForButton();
		}

		public void UnlockEditButton() {
			this.EditButton.UnlockForButton();
		}


		////////////////

		public void ResetUIState( bool isRecentlyTagged ) {
			if( !isRecentlyTagged ) {
				this.UnlockEditButton();
			} else {
				this.LockEditButton();
			}

			this.ResetTagButtons( true );
		}
	}
}
