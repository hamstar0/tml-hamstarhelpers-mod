using HamstarHelpers.Helpers.TmlHelpers;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	partial class UIControlPanel : UIState {
		public void EnableIssueInput() {
			if( !this.IssueTitleInput.IsEnabled ) {
				this.IssueTitleInput.Enable();
			}
			if( !this.IssueBodyInput.IsEnabled ) {
				this.IssueBodyInput.Enable();
			}
			this.RefreshIssueSubmitButton();
		}

		public void DisableIssueInput() {
			if( this.IssueTitleInput.IsEnabled ) {
				this.IssueTitleInput.Disable();
			}
			if( this.IssueBodyInput.IsEnabled ) {
				this.IssueBodyInput.Disable();
			}
			if( this.IssueSubmitButton.IsEnabled ) {
				this.IssueSubmitButton.Disable();
			}
		}


		public void RefreshIssueSubmitButton() {
			if( this.IssueSubmitButton == null || this.IssueTitleInput == null || this.IssueBodyInput == null ) {
				return;
			}

			if( this.IssueTitleInput.Text.Length < 4 || this.IssueBodyInput.Text.Length < 4 ) {
				if( this.IssueSubmitButton.IsEnabled ) {
					this.IssueSubmitButton.Disable();
				}
			} else {
				if( !this.IssueSubmitButton.IsEnabled ) {
					this.IssueSubmitButton.Enable();
				}
			}
		}


		////////////////

		private void SubmitIssue() {
			if( this.CurrentModListItem == null ) { return; }
			if( !ModMetaDataManager.HasGithub( this.CurrentModListItem.Mod ) ) { return; }

			UIControlPanel self = this;
			string issue_title = this.IssueTitleInput.Text;
			string issue_body = this.IssueBodyInput.Text;
			if( string.IsNullOrEmpty( issue_title ) || string.IsNullOrEmpty( issue_body ) ) { return; }

			this.AwaitingReport = true;
			this.DisableIssueInput();

			self.Logic.ReportIssue( self.CurrentModListItem.Mod, issue_title, issue_body, delegate {
				self.AwaitingReport = false;
				self.ResetIssueInput = true;
				self.SetDialogToClose = true;
			} );
		}
	}
}
