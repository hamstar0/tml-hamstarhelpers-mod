using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Services.Timers;
using System;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		public virtual void RefreshButtonEnableStates() {
			this.ResetButton.RefreshEnableState();
		}


		////////////////

		public void SetCategory( string category ) {
			this.CurrentCategory = category;

			foreach( UICategoryMenuButton button in this.CategoryButtons.Values ) {
				if( button.Text != category ) {
					button.Unselect();
				} else {
					button.Select();
				}
			}

			foreach( TagDefinition tagDef in this.Manager.MyTags ) {
				if( tagDef.Category == category ) {
					this.TagButtons[ tagDef.Tag ].Show();
				} else {
					this.TagButtons[ tagDef.Tag ].Hide();
				}
			}
		}

		public void UnsetCategory() {
			this.CurrentCategory = "";

			foreach( (string category, UICategoryMenuButton button) in this.CategoryButtons ) {
				if( button.Text != category && button.IsSelected ) {
					button.Unselect();
				}
			}

			foreach( TagDefinition tagDef in this.Manager.MyTags ) {
				this.TagButtons[tagDef.Tag].Hide();
			}
		}


		////////////////

		public void EnableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Enable();
			}
		}

		public void DisableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Disable();
			}
		}

		public void ResetTagButtons( bool alsoDisable ) {
			foreach( var kv in this.TagButtons ) {
				if( alsoDisable ) {
					kv.Value.Disable();
				}
				kv.Value.SetTagState( 0 );
			}
		}


		////

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

		public void LockResetButton() {
			this.ResetButton.Lock();
		}
	}
}
