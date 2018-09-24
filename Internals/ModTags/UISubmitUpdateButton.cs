﻿using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	internal class UISubmitUpdateButton : UITextPanelButton {
		private readonly ModTagUI ModTagUI;



		////////////////

		public UISubmitUpdateButton( ModTagUI modtagui ) : base( UITheme.Vanilla, "", 0.65f, true ) {
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

			if( this.Text == "Update mod tags" ) {
				this.SetTagSubmitMode();
			} else {
				this.SubmitTags();
			}
		}


		////////////////

		public void SetTagUpdateMode() {
			this.SetText( "Update mod tags" );

			this.UpdateEnableState();
		}

		public void SetTagSubmitMode() {
			this.SetText( "Submit mod tags" );
			this.Disable();
			
			this.ModTagUI.EnableButtons();
		}

		////////////////

		public void UpdateEnableState() {
			if( this.Text == "Update mod tags" ) {
				this.Enable();
				return;
			}

			int tag_count = 0;

			foreach( var kv in this.ModTagUI.TagButtons ) {
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


		////////////////

		private void SubmitTags() {
			//TODO
		}
	}
}
