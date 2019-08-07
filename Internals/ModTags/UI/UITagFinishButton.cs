using HamstarHelpers.Classes.UI.Elements.Menu;
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
					MenuContext.InfoDisplay?.SetText( "Submit tags to online database.", Color.White );
				} else if( this.Text == "Modify Tags" ) {
					MenuContext.InfoDisplay?.SetText( "Enable changing current mod's tags.", Color.White );
				}
			};
			this.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
				MenuContext.InfoDisplay?.SetText( "", Color.White );
			};

			this.RecalculatePos();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			if( this.Text == "Modify Tags" ) {
				this.SetModeSubmit();
			} else if( this.Text == "Submit Tags" ) {
				this.MenuContext.SubmitTags();
			}
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.UpdateEnableState();
			this.MenuContext.Panel.DisableTagButtons();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
			this.MenuContext.Panel.EnableTagButtons();
			
			if( this.MenuContext.ResetButton.IsLocked ) {
				this.MenuContext.ResetButton.Unlock();
			}
		}
		

		////////////////

		public void SetModeReadOnly() {
			this.SetText( "Modify Tags" );
			
			this.UpdateEnableState();
			this.MenuContext.ResetButton.UpdateEnableState();
		}

		public void SetModeSubmit() {
			this.SetText( "Submit Tags" );
			
			this.MenuContext.Panel.EnableTagButtons();

			this.UpdateEnableState();
			this.MenuContext.ResetButton.UpdateEnableState();
		}

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			string modName = this.MenuContext.CurrentModName;

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

			ISet<string> tags = this.MenuContext.GetTagsWithGivenState( 1 );

			if( this.MenuContext.AllModTagsSnapshot != null && this.MenuContext.AllModTagsSnapshot.ContainsKey(modName) ) {
				if( tags.SetEquals( this.MenuContext.AllModTagsSnapshot[modName] ) ) {
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

			this.MenuContext.UpdateMode( this.Text == "Submit Tags" );
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );
		}
	}
}
