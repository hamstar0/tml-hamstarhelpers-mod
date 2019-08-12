using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.ModInfo.MenuContext;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI { 
	/// @private
	partial class UITagFinishButton : UIMenuButton {
		private readonly ModTagsEditorManager Manager;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagFinishButton( UITheme theme, ModTagsEditorManager manager )
				: base( theme, "", 98f, 24f, -98f, 172f, 0.36f, true ) {
			this.Manager = manager;

			this.OnMouseOver += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( this.Text == "Submit Tags" ) {
					manager.SetInfoText( "Submit tags to online database.", Color.White );
					//MenuContext.InfoDisplay?.SetText( "Submit tags to online database.", Color.White );
				} else if( this.Text == "Modify Tags" ) {
					manager.SetInfoText( "Enable changing current mod's tags.", Color.White );
				}
			};
			this.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
				manager.SetInfoText( "", Color.White );
			};

			this.RecalculatePos();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			if( this.Text == "Modify Tags" ) {
				this.SetModeSubmit();
			} else if( this.Text == "Submit Tags" ) {
				this.Manager.SubmitTags();
			}
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.Manager.TagsUI.RefreshButtonEnableStates();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.Manager.TagsUI.RefreshButtonEnableStates();
			//if( this.MenuContext.ResetButton.IsLocked ) {
			//	this.MenuContext.ResetButton.Unlock();
			//}
		}
		

		////////////////

		public void SetModeReadOnly() {
			this.SetText( "Modify Tags" );

			this.Manager.TagsUI.RefreshButtonEnableStates();
			//this.MenuContext.ResetButton.UpdateEnableState();
		}

		public void SetModeSubmit() {
			this.SetText( "Submit Tags" );

			this.Manager.TagsUI.EnableTagButtons();
			//this.MenuContext.Panel.EnableTagButtons();

			this.Manager.TagsUI.RefreshButtonEnableStates();
		}

		////////////////

		public void RefreshEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			string modName = this.Manager.CurrentModName;

			if( string.IsNullOrEmpty( modName ) ) {
				this.Disable();
				return;
			}

			if( this.Text == "Modify Tags" ) {
				this.Enable();
				return;
			}

			if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( modName ) ) {
				this.Disable();
				return;
			}

			ISet<string> tags = this.Manager.GetTagsWithGivenState( 1 );

			if( this.Manager.AllModTagsSnapshot != null && this.Manager.AllModTagsSnapshot.ContainsKey(modName) ) {
				if( tags.SetEquals( this.Manager.AllModTagsSnapshot[modName] ) ) {
					this.Disable();
					return;
				}
			}

			if( tags.Count >= 2 ) {
				this.Enable();
				return;
			} else {
				this.Disable();
				return;
			}
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( this.Text == "Submit Tags" ) {
				this.Manager.EnableSubmitMode();
			} else {
				this.Manager.DisableSubmitMode();
			}
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );
		}
	}
}
