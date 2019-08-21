using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditorInterface : UIModTagsInterface {
		public override void RefreshControlsStates() {
			base.RefreshControlsStates();

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

			if( this.Manager.IsCurrentModRecentlyTagged() ) {
				this.Manager.SetInfoTextDefault( "Tags already set." );
				this.SetReadOnlyMode( false );
				return;
			}

			if( !this.EditButton.IsEditMode ) {
				if( this.Manager.GetTagsWithGivenState( 1 ).Count > 0 ) {
					this.Manager.SetInfoTextDefault( "Do these tags look incorrect? If so, modify them." );
					this.SetReadOnlyMode( true );
				} else {
					this.Manager.SetInfoTextDefault( "No tags set for this mod. Why not add some?" );
					this.SetEditMode( false );
				}
				return;
			}

			ISet<string> tags = this.Manager.GetTagsWithGivenState( 1 );

			if( this.Manager.AllModTagsSnapshot?.ContainsKey( modName ) == true ) {
				// No changes?
				if( tags.SetEquals( this.Manager.AllModTagsSnapshot[modName] ) ) {
					this.SetEditMode( false );
					return;
				}
			}

			if( this.EditButton.IsEditMode ) {
				// Non-zero tags?
				if( tags.Count >= 2 ) {
					this.EditButton.Enable();
					return;
				} else {
					this.EditButton.Disable();
					return;
				}
			}
		}


		////////////////

		private void SetReadOnlyMode( bool enableModeChangeButton ) {
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


		private void SetEditMode( bool enableSubmitButton ) {
			foreach( UITagMenuButton tagButton in this.TagButtons.Values ) {
				tagButton.Enable();
			}

			this.EditButton.SetEditModeForButton();

			if( enableSubmitButton ) {
				this.EditButton.Enable();
			} else {
				this.EditButton.Disable();
			}
		}
	}
}
