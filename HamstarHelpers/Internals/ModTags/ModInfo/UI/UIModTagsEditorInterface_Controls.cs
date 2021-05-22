using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditorInterface : UIModTagsInterface {
		public override void RefreshControls() {
			this.ResetButton.RefreshEnableState( this.EditButton.IsEditMode );  //was base.RefreshControls();

			this.RefreshMode();
		}


		////////////////

		private void RefreshMode() {
			string modName = this.Manager.CurrentModName;

			// No mod?
			if( string.IsNullOrEmpty( modName ) ) {
				this.SetReadOnlyMode( false );
				return;
			}

			// Skip mods already set with tags
			if( this.Manager.IsCurrentModRecentlyTagged() ) {
				this.Manager.SetInfoTextDefault( "Tags already set." );
				this.SetReadOnlyMode( false );
				return;
			}

			if( this.EditButton.IsEditMode ) {
				this.RefreshEditMode();
			} else {
				this.RefreshReadOnlyMode();
			}
		}

		private void RefreshReadOnlyMode() {
			if( this.Manager.GetTagsWithGivenState( 1 ).Count > 0 ) {
				this.Manager.SetInfoTextDefault( "Do these tags look incorrect? If so, modify them." );
				this.SetReadOnlyMode( true );
				return;
			} else {
				this.Manager.SetInfoTextDefault( "No tags set for this mod. Why not add some?" );
				this.SetEditMode( false );
				return;
			}
		}

		private void RefreshEditMode() {
			string modName = this.Manager.CurrentModName;
			ISet<string> onTags = this.Manager.GetTagsWithGivenState( 1 );

			if( this.Manager.AllModTagsSnapshot == null ) {
				LogLibraries.Alert( "AllModTagsSnapshot == null" );
			} else if( this.Manager.AllModTagsSnapshot.ContainsKey( modName ) == true ) {
				// No changes?
				if( onTags.SetEquals( this.Manager.AllModTagsSnapshot[modName] ) ) {
					this.SetEditMode( false );
					return;
				}
			}

			// Non-zero tags?
			if( onTags.Count >= 2 ) {
				this.EditButton.Enable();
			} else {
				this.EditButton.Disable();
			}
		}


		////////////////

		public void SetReadOnlyMode( bool enableModeChangeButton ) {
			foreach( UITagMenuButton tagButton in this.TagButtons.Values ) {
				tagButton.Disable();
			}

			this.EditButton.SetReadOnlyModeForButton();

			if( enableModeChangeButton ) {
				this.EditButton.Enable();
			} else {
				this.EditButton.Disable();
			}
		}


		public void SetEditMode( bool enableSubmitButton ) {
			foreach( UITagMenuButton tagButton in this.TagButtons.Values ) {
				tagButton.Enable();
			}

			this.EditButton.SetEditModeForButton();

			if( enableSubmitButton ) {
				this.EditButton.Enable();
			} else {
				this.EditButton.Disable();
			}

			this.ResetButton.RefreshEnableState( true );
		}
	}
}
