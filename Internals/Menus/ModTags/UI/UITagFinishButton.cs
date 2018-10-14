using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags.UI {
	partial class UITagFinishButton : UIMenuButton {
		private readonly ModInfoTagsMenuContext MenuContext;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagFinishButton( ModInfoTagsMenuContext menu_context )
				: base( UITheme.Vanilla, "", 98f, 24f, -298f, 172f, 0.36f, true ) {
			this.MenuContext = menu_context;

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
			this.MenuContext.DisableTagButtons();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
			this.MenuContext.EnableTagButtons();
			
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
			
			this.MenuContext.EnableTagButtons();

			this.UpdateEnableState();
			this.MenuContext.ResetButton.UpdateEnableState();
		}

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			string mod_name = this.MenuContext.CurrentModName;

			if( string.IsNullOrEmpty( mod_name ) ) {
				this.Disable();
				return;
			}

			if( this.Text == "Modify Tags" ) {
				this.Enable();
				return;
			}

			if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( mod_name ) ) {
				this.Disable();
				return;
			}

			ISet<string> tags = this.MenuContext.GetTagsOfState( 1 );

			if( this.MenuContext.AllModTagsSnapshot != null && this.MenuContext.AllModTagsSnapshot.ContainsKey(mod_name) ) {
				if( tags.SetEquals( this.MenuContext.AllModTagsSnapshot[mod_name] ) ) {
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

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.IsMouseHovering ) {
				if( this.Text == "Submit Tags" ) {
					MenuContextBase.InfoDisplay.SetText( "Submit tags to online database.", Color.White );
				}
			}
		}
	}
}
