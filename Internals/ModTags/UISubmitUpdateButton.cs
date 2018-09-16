using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Internals.ModPackBrowser {
	internal class UISubmitUpdateButton : UITextPanelButton {
		private readonly ModTagUI ModTagUI;



		public UISubmitUpdateButton( ModTagUI modtagui )
				: base( UITheme.Vanilla, "", 1.5f, false ) {
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
			this.Top.Set( 4f, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}


		////////////////

		public void SetTagUpdateMode() {
			this.SetText( "Update mod tags" );
		}

		public void SetTagSubmitMode() {
			this.SetText( "Submit mod tags" );
			this.Disable();
		}

		////////////////

		public void UpdateEnableState( IDictionary<string, UIModTagButton> buttons ) {
			int tag_count = 0;

			foreach( var kv in buttons ) {
				if( kv.Value.HasTag ) {
					tag_count++;
				}
			}

			if( tag_count >= 3 ) {
				this.Enable();
			} else {
				this.Disable();
			}
		}
	}
}
