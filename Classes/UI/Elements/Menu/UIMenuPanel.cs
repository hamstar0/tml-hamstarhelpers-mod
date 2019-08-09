using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using Terraria;


namespace HamstarHelpers.Classes.UI.Elements.Menu {
	/// <summary>
	/// A UI panel specialized for menu use.
	/// </summary>
	public class UIMenuPanel : UIThemedPanel {
		protected float XCenterOffset;
		protected float YPos;



		////////////////

		/// <summary>
		/// </summary>
		/// <param name="theme"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="xCenterOffset">Offset from horizontal center of screen.</param>
		/// <param name="y">Offset from top of screen.</param>
		public UIMenuPanel( UITheme theme, float width, float height, float xCenterOffset, float y )
				: base( theme, true ) {
			this.Width.Set( width, 0f );
			this.Height.Set( height, 0f );

			this.XCenterOffset = xCenterOffset;
			this.YPos = y;

			this.RecalculatePos();
			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Recalculates panel's position relative to screen dimensions.
		/// </summary>
		public virtual void RecalculatePos() {
			this.Left.Set( ((float)Main.screenWidth / 2f) + this.XCenterOffset, 0f );
			this.Top.Set( this.YPos, 0f );
		}

		/// <summary>
		/// Recalculates whole panel's position and dimensions.
		/// </summary>
		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}
	}
}
