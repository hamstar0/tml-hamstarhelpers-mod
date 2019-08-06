using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using Terraria;


namespace HamstarHelpers.Classes.UI.Elements.Menu {
	internal class UIMenuButton : UITextPanelButton {
		protected float XCenterOffset;
		protected float YPos;



		////////////////

		public UIMenuButton( UITheme theme, string text, float width, float height, float xCenterOffset, float y,
				float textScale=1f, bool largeText=false )
				: base( theme, text, textScale, largeText ) {
			this.Width.Set( width, 0f );
			this.Height.Set( height, 0f );

			this.XCenterOffset = xCenterOffset;
			this.YPos = y;

			this.RecalculatePos();
		}


		////////////////

		public virtual void RecalculatePos() {
			float left = ( (float)Main.screenWidth / 2f ) + this.XCenterOffset;
			float top = this.YPos;

			this.Left.Set( left, 0f );
			this.Top.Set( top, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}
	}
}
