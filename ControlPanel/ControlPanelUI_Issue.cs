using HamstarHelpers.TmlHelpers;
using HamstarHelpers.UIHelpers.Elements;
using System;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI {
		public void EnableIssueInput() {
			if( !this.IssueTitleInput.IsEnabled ) {
				this.IssueBodyInput.Enable();
			}
			if( !this.IssueBodyInput.IsEnabled ) {
				this.IssueBodyInput.Enable();
			}
			if( !this.IssueSubmitButton.IsEnabled ) {
				this.IssueSubmitButton.Enable();
			}
		}

		public void DisableIssueInput() {
			if( this.IssueTitleInput.IsEnabled ) {
				this.IssueBodyInput.Disable();
			}
			if( this.IssueBodyInput.IsEnabled ) {
				this.IssueBodyInput.Disable();
			}
			if( this.IssueSubmitButton.IsEnabled ) {
				this.IssueSubmitButton.Disable();
			}
		}


		////////////////


		private void SubmitIssue() {
			if( this.CurrentModListItem == null ) { return; }
			if( !ExtendedModManager.HasGithub( this.CurrentModListItem.Mod ) ) { return; }

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
			} ) );

			t.Start();
		}
	}
}
