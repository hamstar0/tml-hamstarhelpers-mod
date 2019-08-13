using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using Terraria;


namespace HamstarHelpers.Classes.UI.Elements.Menu {
	internal class UIMenuButton : UITextPanelButton {
		protected float PositionXCenterOffset;
		protected float PositionY;



		////////////////

		public UIMenuButton( UITheme theme, string text, float width, float height, float xCenterOffset, float y,
				float textScale=1f, bool largeText=false )
				: base( theme, text, textScale, largeText ) {
			this.Width.Set( width, 0f );
			this.Height.Set( height, 0f );

			this.PositionXCenterOffset = xCenterOffset;
			this.PositionY = y;

			this.RecalculatePosition();
		}


		////////////////

		public virtual void RecalculatePosition() {
			float left = ( (float)Main.screenWidth / 2f ) + this.PositionXCenterOffset;
			float top = this.PositionY;

			this.Left.Set( left, 0f );
			this.Top.Set( top, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePosition();
			base.Recalculate();
		}
	}
}
