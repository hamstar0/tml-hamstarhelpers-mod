using HamstarHelpers.Helpers.TmlHelpers;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	partial class UIModControlPanelTab : UIControlPanelTab {
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

			UIModControlPanelTab self = this;
			string issueTitle = this.IssueTitleInput.Text;
			string issueBody = this.IssueBodyInput.Text;
			if( string.IsNullOrEmpty( issueTitle ) || string.IsNullOrEmpty( issueBody ) ) { return; }

			this.AwaitingReport = true;
			this.DisableIssueInput();

			self.Logic.ReportIssue( self.CurrentModListItem.Mod, issueTitle, issueBody, delegate {
				self.AwaitingReport = false;
				self.ResetIssueInput = true;
				self.RequestClose = true;
			} );
		}
	}
}
