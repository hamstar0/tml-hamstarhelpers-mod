using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.ModInfo.Manager;
using HamstarHelpers.Internals.ModTags.ModInfo.MenuContext;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI.Buttons { 
	/// @private
	partial class UIEditModeMenuButton : UIMenuButton {
		public readonly static string ReadOnlyModeText = "Modify Tags";
		public readonly static string EditModeText = "Submit Tags";


		////////////////

		public static float ButtonWidth { get; private set; } = 98f;
		public static float ButtonHeight { get; private set; } = 24f;



		////////////////

		private readonly ModTagsEditorManager Manager;


		////////////////

		public bool IsLocked { get; private set; }
		public bool IsEditMode => this.Text == UIEditModeMenuButton.EditModeText;
		public bool IsReadOnlyMode => this.Text == UIEditModeMenuButton.ReadOnlyModeText;



		////////////////

		public UIEditModeMenuButton( UITheme theme, ModTagsEditorManager manager, float xCenterOffset, float y )
				: base( theme,
					UIEditModeMenuButton.ReadOnlyModeText,
					UIEditModeMenuButton.ButtonWidth,
					UIEditModeMenuButton.ButtonHeight,
					xCenterOffset,  //Old value: -98f,172f,
					y,
					0.36f,
					true ) {
			this.Manager = manager;

			this.OnMouseOver += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( this.IsEditMode ) {
					manager.SetInfoText( "Submit tags to online database.", Color.White );
					//MenuContext.InfoDisplay?.SetText( "Submit tags to online database.", Color.White );
				} else if( this.IsReadOnlyMode ) {
					manager.SetInfoText( "Enable changing current mod's tags.", Color.White );
				}
			};
			this.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
				manager.SetInfoText( "", Color.White );
			};

			this.RecalculatePosition();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			if( this.IsReadOnlyMode ) {
				this.SetEditModeForUI();
			} else if( this.IsEditMode ) {
				this.Manager.SubmitTags();
			}
		}


		////////////////

		public void LockForButton() {
			this.IsLocked = true;
		}

		public void UnlockForButton() {
			this.IsLocked = false;
		}

		////

		public void LockForUI() {
			this.LockForButton();

			this.Manager.TagsUI.RefreshButtonEnableStates();
		}

		public void UnlockForUI() {
			this.UnlockForButton();

			this.Manager.TagsUI.RefreshButtonEnableStates();
			//if( this.MenuContext.ResetButton.IsLocked ) {
			//	this.MenuContext.ResetButton.Unlock();
			//}
		}


		////////////////

		public void SetReadOnlyModeForButton() {
			this.SetText( UIEditModeMenuButton.ReadOnlyModeText );
		}

		public void SetEditModeForButton() {
			this.SetText( UIEditModeMenuButton.EditModeText );
		}

		////

		public void SetReadOnlyModeForUI() {
			this.SetReadOnlyModeForButton();

			this.Manager.TagsUI.RefreshButtonEnableStates();
		}

		public void SetEditModeForUI() {
			this.SetEditModeForButton();

			this.Manager.TagsUI.EnableCatTagInterface();
			this.Manager.TagsUI.RefreshButtonEnableStates();
		}

		////////////////

		public void RefreshEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			string modName = this.Manager.CurrentModName;
			ISet<string> tags = this.Manager.GetTagsWithGivenState( 1 );

			if( string.IsNullOrEmpty( modName ) ) {
				this.Disable();
				return;
			}

			// Already tagged?
			if( ModTagsEditorMenuContext.RecentTaggedMods.Contains( modName ) ) {
				this.Disable();
				return;
			}

			if( this.Manager.AllModTagsSnapshot?.ContainsKey( modName ) == true ) {
				// No changes?
				if( tags.SetEquals( this.Manager.AllModTagsSnapshot[modName] ) ) {
					this.Disable();
					return;
				}
			}

			if( this.IsEditMode ) {
				// Non-zero tags?
				if( tags.Count >= 2 ) {
					this.Enable();
					return;
				} else {
					this.Disable();
					return;
				}
			}
		}


		////////////////

		/*public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( this.Text == UIEditModeMenuButton.EditModeText ) {
				this.Manager.MyTagsUI.EnableEditMode();
			} else {
				this.Manager.MyTagsUI.DisableEditMode();
			}
		}*/
	}
}
