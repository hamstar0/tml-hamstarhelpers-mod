using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	internal class UISubmitUpdateButton : UITextPanelButton {
		private readonly ModInfoUI ModTagUI;

		public bool IsLocked = false;



		////////////////

		public UISubmitUpdateButton( ModInfoUI modtagui ) : base( UITheme.Vanilla, "", 0.65f, true ) {
			this.ModTagUI = modtagui;

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
				this.ModTagUI.SubmitTags();
			}
		}


		////////////////

		public void SetTagUpdateMode() {
			this.SetText( "Modify mod tags" );

			this.UpdateEnableState();
		}

		public void SetTagSubmitMode() {
			this.SetText( "Submit mod tags" );
			this.Disable();
			
			this.ModTagUI.EnableTagButtons();
		}

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			if( string.IsNullOrEmpty(this.ModTagUI.ModName) ) {
				this.Disable();
				return;
			}

			if( this.Text == "Modify mod tags" ) {
				this.Enable();
				return;
			}
			
			if( this.ModTagUI.GetSelectedTags().Count >= 3 ) {
				this.Enable();
			} else {
				this.Disable();
			}
		}
	}
}
