using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;
using Terraria;


namespace HamstarHelpers.Classes.UI.Elements.Menu {
	/// <summary>
	/// A button intended for use with main menu UIs.
	/// </summary>
	public class UIMenuButton : UITextPanelButton {
		/// <summary>
		/// Distance of left edge of this element from the horizontal center of the screen.
		/// </summary>
		protected float PositionXCenterOffset;
		/// <summary>
		/// Distance of the top edge of this element from top of the screen.
		/// </summary>
		protected float PositionY;



		////////////////

		/// <summary></summary>
		/// <param name="theme">Appearance style.</param>
		/// <param name="text"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="xCenterOffset">Distance of left edge of this element from the horizontal center of the screen.</param>
		/// <param name="y">Distance of the top edge of this element from top of the screen.</param>
		/// <param name="textScale"></param>
		/// <param name="largeText"></param>
		public UIMenuButton( UITheme theme, string text, float width, float height, float xCenterOffset, float y,
				float textScale = 1f, bool largeText = false )
				: base( theme, text, textScale, largeText ) {
			this.Width.Set( width, 0f );
			this.Height.Set( height, 0f );

			this.PositionXCenterOffset = xCenterOffset;
			this.PositionY = y;

			this.RecalculatePosition();
		}


		////////////////

		/// <summary>
		/// Repositions this button.
		/// </summary>
		/// <param name="xCenterOffset">Distance of left edge of this element from the horizontal center of the screen.</param>
		/// <param name="y">Distance of the top edge of this element from top of the screen.</param>
		public void SetMenuSpacePosition( float xCenterOffset, float y ) {
			this.PositionXCenterOffset = xCenterOffset;
			this.PositionY = y;

			this.RecalculatePosition();
		}

		////////////////

		/// <summary>Recalculates only this element's position.</summary>
		public virtual void RecalculatePosition() {
			float left = ( (float)Main.screenWidth / 2f ) + this.PositionXCenterOffset;
			float top = this.PositionY;

			this.Left.Set( left, 0f );
			this.Top.Set( top, 0f );
		}

		/// <summary>Recalculates this element's position, and it's child elements.</summary>
		public override void Recalculate() {
			this.RecalculatePosition();
			base.Recalculate();
		}
	}
}
