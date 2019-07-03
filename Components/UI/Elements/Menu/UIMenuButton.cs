using HamstarHelpers.Helpers.Debug;
using Terraria;


namespace HamstarHelpers.Components.UI.Elements.Menu {
	/// <summary>
	/// Defines a main menu button element. Designed to work outside of a UIState context.
	/// </summary>
	internal class UIMenuButton : UITextPanelButton {
		protected float XCenterOffset;
		protected float YPos;



		////////////////

		public UIMenuButton( UITheme theme, string text, float width, float height, float xCenterOffset, float y, float textScale=1f, bool largeText=false )
				: base( theme, text, textScale, largeText ) {
			this.Width.Set( width, 0f );
			this.Height.Set( height, 0f );

			this.XCenterOffset = xCenterOffset;
			this.YPos = y;

			this.RecalculatePos();
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
