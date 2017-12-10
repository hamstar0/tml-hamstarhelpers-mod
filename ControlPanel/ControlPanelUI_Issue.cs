using HamstarHelpers.TmlHelpers;
using System;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI {
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

			ControlPanelUI self = this;
			string issue_title = this.IssueTitleInput.Text;
			string issue_body = this.IssueBodyInput.Text;
			if( string.IsNullOrEmpty( issue_title ) || string.IsNullOrEmpty( issue_body ) ) { return; }

			this.AwaitingReport = true;
			this.DisableIssueInput();

			var t = new Thread( new ThreadStart( delegate () {
				try {
					self.Logic.ReportIssue( self.CurrentModListItem.Mod, issue_title, issue_body );
				} catch( Exception e ) {
					ErrorLogger.Log( e.ToString() );
				}

				self.AwaitingReport = false;
				self.ResetIssueInput = true;
				self.CloseDialog = true;
			} ) );

			t.Start();
		}
	}
}
