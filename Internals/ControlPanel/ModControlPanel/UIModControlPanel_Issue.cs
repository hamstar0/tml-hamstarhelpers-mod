using HamstarHelpers.Helpers.ModHelpers;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	partial class UIModControlPanelTab : UIControlPanelTab {
		public void EnableIssueInput() {
			if( !this.IssueTitleInput.IsInteractive ) {
				this.IssueTitleInput.Enable();
			}
			if( !this.IssueBodyInput.IsInteractive ) {
				this.IssueBodyInput.Enable();
			}
			this.RefreshIssueSubmitButton();
		}

		public void DisableIssueInput() {
			if( this.IssueTitleInput.IsInteractive ) {
				this.IssueTitleInput.Disable();
			}
			if( this.IssueBodyInput.IsInteractive ) {
				this.IssueBodyInput.Disable();
			}
			if( this.IssueSubmitButton.IsInteractive ) {
				this.IssueSubmitButton.Disable();
			}
		}


		public void RefreshIssueSubmitButton() {
			if( this.IssueSubmitButton == null || this.IssueTitleInput == null || this.IssueBodyInput == null ) {
				return;
			}

			if( this.IssueTitleInput.Text.Length < 4 || this.IssueBodyInput.Text.Length < 4 ) {
				if( this.IssueSubmitButton.IsInteractive ) {
					this.IssueSubmitButton.Disable();
				}
			} else {
				if( !this.IssueSubmitButton.IsInteractive ) {
					this.IssueSubmitButton.Enable();
				}
			}
		}


		////////////////

		private void SubmitIssue() {
			if( this.CurrentModListItem == null ) { return; }
			if( !ModFeaturesHelpers.HasGithub( this.CurrentModListItem.Mod ) ) { return; }

			string issueTitle = this.IssueTitleInput.Text;
			string issueBody = this.IssueBodyInput.Text;
			if( string.IsNullOrEmpty( issueTitle ) || string.IsNullOrEmpty( issueBody ) ) { return; }

			this.AwaitingReport = true;
			this.DisableIssueInput();

			UIModControlPanelTab self = this;
			this.Logic.ReportIssue( this.CurrentModListItem.Mod, issueTitle, issueBody, delegate {
				self.AwaitingReport = false;
				self.ResetIssueInput = true;
				self.RequestClose = true;
			} );
		}
	}
}
