using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using Terraria;


namespace HamstarHelpers.Internals.ModPackBrowser {
	internal class UISubmitUpdateButton : UITextPanelButton {
		public UISubmitUpdateButton() : base( UITheme.Vanilla, "", 1.5f, false ) {
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

		public void SetMod( string modname ) {
			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				if( args.Found && args.ModTags.ContainsKey( modname ) ) {
					this.SetText( "Update mod tags" );
				} else {
					this.SetText( "Submit mod tags" );
					this.Disable();
				}
				return false;
			} );
		}
	}
}
