﻿using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.Menus.ModTags;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	/// @private
	partial class UITagFinishButton : UIMenuButton {
		private readonly ModTagsManager Manager;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagFinishButton( UITheme theme, ModTagsManager manager )
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

			this.UpdateEnableState();
			this.Manager.DisableTagButtons();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
			this.Manager.EnableTagButtons();
			this.Manager.EnableResetButton();
			//if( this.MenuContext.ResetButton.IsLocked ) {
			//	this.MenuContext.ResetButton.Unlock();
			//}
		}
		

		////////////////

		public void SetModeReadOnly() {
			this.SetText( "Modify Tags" );
			
			this.UpdateEnableState();
			this.Manager.UpdateResetButton();
			//this.MenuContext.ResetButton.UpdateEnableState();
		}

		public void SetModeSubmit() {
			this.SetText( "Submit Tags" );

			this.Manager.EnableTagButtons();
			//this.MenuContext.Panel.EnableTagButtons();

			this.UpdateEnableState();
			//this.MenuContext.ResetButton.UpdateEnableState();
			this.Manager.UpdateResetButton();
		}

		////////////////

		public void UpdateEnableState() {
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

			this.Manager.UpdateMode( this.Text == "Submit Tags" );
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );
		}
	}
}
