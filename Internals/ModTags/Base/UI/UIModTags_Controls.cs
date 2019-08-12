using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Internals.ModTags.ModInfo.UI;
using HamstarHelpers.Services.Timers;
using System;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTags<T> : UIThemedPanel where T : ModTagsManager {
		public void OnTagStateChange( UITagButton button ) {
			this.RefreshButtonEnableStates();
		}


		////////////////

		public virtual void RefreshButtonEnableStates() {
			this.ResetButton.RefreshEnableState();
		}


		////////////////

		public void EnableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Enable();
			}
		}

		public static explicit operator UIModTags<T>( UIModTagsEditor v ) {
			throw new NotImplementedException();
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
