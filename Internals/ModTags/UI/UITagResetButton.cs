﻿using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	/// @private
	partial class UITagResetButton : UIMenuButton {
		private readonly ModTagsManager Manager;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagResetButton( UITheme theme, ModTagsManager manager )
				: base( theme, "Reset Tags", 98f, 24f, -196f, 172f, 0.36f, true ) {
			this.Manager = manager;

			this.RecalculatePos();
			this.UpdateEnableState();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			this.Manager.ResetTagButtons();
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.UpdateEnableState();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
		}
		

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			//if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( modInfoContext.CurrentModName ) ) {
			if( this.Manager.IsCurrentModRecentlyTagged() ) {
				this.Disable();
				return;
			}

			if( this.Manager.GetTagsWithGivenState(1).Count > 0 || this.Manager.GetTagsWithGivenState(-1).Count > 0 ) {
				//modInfoContext.FinishButton.Text == "Modify Tags"
				if( this.Manager.CanEditTags() ) {
					this.Disable();
					return;
				}
				
				this.Enable();
				return;
			} else {
				this.Disable();
				return;
			}
		}
	}
}
