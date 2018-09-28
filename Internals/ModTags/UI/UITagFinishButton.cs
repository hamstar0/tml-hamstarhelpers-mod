using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	internal class UITagFinishButton : UITextPanelButton {
		private readonly ModInfoTagsMenuContext UIManager;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagFinishButton( ModInfoTagsMenuContext modtagui ) : base( UITheme.Vanilla, "", 0.65f, true ) {
			this.UIManager = modtagui;

			this.Width.Set( 256f, 0f );
			this.Height.Set( 40f, 0f );

			this.RecalculatePos();
		}

		public override void OnInitialize() {
			base.OnInitialize();
		}


		////////////////

		private void RecalculatePos() {
			this.Left.Set( (Main.screenWidth / 2) - 128, 0f );
			this.Top.Set( 2f, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			if( this.Text == "Modify mod tags" ) {
				this.SetTagSubmitMode();
			} else {
				this.UIManager.SubmitTags();
			}
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.UpdateEnableState();
			this.UIManager.DisableTagButtons();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
			this.UIManager.EnableTagButtons();
		}
		

		////////////////

		public void SetTagUpdateMode() {
			if( this.IsLocked ) { return; }

			this.SetText( "Modify mod tags" );

			this.UpdateEnableState();
		}

		public void SetTagSubmitMode() {
			if( this.IsLocked ) { return; }

			this.SetText( "Submit mod tags" );
			this.Disable();
			
			this.UIManager.EnableTagButtons();
		}

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			if( string.IsNullOrEmpty(this.UIManager.ModName) ) {
				this.Disable();
				return;
			}

			if( this.Text == "Modify mod tags" ) {
				this.Enable();
				return;
			}

			if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( this.UIManager.ModName ) ) {
				this.Disable();
				return;
			}

			if( this.UIManager.GetTagsOfState(1).Count >= 2 ) {
				this.Enable();
			} else {
				this.Disable();
			}
		}
	}
}
