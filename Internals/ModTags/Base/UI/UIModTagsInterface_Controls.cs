using HamstarHelpers.Classes.ModTagDefinitions;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Services.Timers;
using System;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		public void EnableCatTagInterface() {
			foreach( UICategoryMenuButton catButton in this.CategoryButtons.Values ) {
				catButton.Enable();
			}

			foreach( (string tagName, UITagMenuButton tagButton) in this.TagButtons ) {
				if( this.Manager.MyTagMap[tagName].Category == this.CurrentCategory ) {
					tagButton.TakeOut();
				} else {
					tagButton.PutAway();
				}
			}
		}

		public void DisableCatTagInterface() {
			foreach( UICategoryMenuButton catButton in this.CategoryButtons.Values ) {
				catButton.Disable();
			}

			foreach( (string tagName, UITagMenuButton tagButton) in this.TagButtons ) {
				tagButton.PutAway();
			}
		}


		////////////////

		public virtual void RefreshControls() {
			this.ResetButton.RefreshEnableState( true );
		}


		////////////////

		public void SetCategory( string category ) {
			this.UnsetCategory();
			this.CurrentCategory = category;

			foreach( (string tagName, UITagMenuButton tagButton) in this.TagButtons ) {
				if( this.Manager.MyTagMap[tagName].Category == this.CurrentCategory ) {
					tagButton.TakeOut();
				} else {
					tagButton.PutAway();
				}
			}
		}

		public void UnsetCategory() {
			this.CurrentCategory = "";

			foreach( (string category, UICategoryMenuButton catButton) in this.CategoryButtons ) {
				if( catButton.IsSelected ) {
					catButton.Unselect();
				}
			}

			foreach( ModTagDefinition tagDef in this.Manager.MyTags ) {
				var tagButton = this.TagButtons[tagDef.Tag];

				tagButton.PutAway();
			}
		}


		////////////////

		public void ResetTagButtonOnStates( bool alsoDisable ) {
			foreach( (string tagName, UITagMenuButton tagButton) in this.TagButtons ) {
				if( alsoDisable ) {
					tagButton.Disable();
				}
				tagButton.SetTagState( 0 );
			}
		}


		////////////////

		public void SafelySetTagButton( string tag ) {
			var button = this.TagButtons[tag];

			if( button.TagState != 1 ) {
				if( Timers.GetTimerTickDuration( "ModHelpersTagsEditDefaults" ) <= 0 ) {
					Timers.SetTimer( "ModHelpersTagsEditDefaults", 60, () => {
						button.SetTagState( 1 );
						return false;
					} );
				}
			}
		}


		////////////////

		public void EnableResetButton() {
			this.ResetButton?.Enable();
		}

		public void DisableResetButton() {
			this.ResetButton?.Disable();
		}
	}
}
