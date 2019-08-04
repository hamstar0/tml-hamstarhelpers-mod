using HamstarHelpers.Components.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using Terraria;


namespace HamstarHelpers.Components.UI.Elements.Menu {
	internal class UIMenuPanel : UIThemedPanel {
		protected float XCenterOffset;
		protected float YPos;



		////////////////

		public UIMenuPanel( UITheme theme, float width, float height, float xCenterOffset, float y ) : base( theme ) {
			this.Width.Set( width, 0f );
			this.Height.Set( height, 0f );

			this.XCenterOffset = xCenterOffset;
			this.YPos = y;

			this.RecalculatePos();
			this.RefreshTheme();
		}


		////////////////

		public virtual void RecalculatePos() {
			this.Left.Set( ((float)Main.screenWidth / 2f) + this.XCenterOffset, 0f );
			this.Top.Set( this.YPos, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}
	}
}
