using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ModTags.UI {
	internal class UIInfoDisplay : UIMenuPanel {
		private readonly TagsMenuContextBase UIManager;
		private readonly UIText InfoDisplay;



		////////////////

		public UIInfoDisplay( TagsMenuContextBase modtagui )
				: base( UITheme.Vanilla, 800f, 40f, -400f, 2f ) {
			this.UIManager = modtagui;

			this.InfoDisplay = new UIText( "" );
			this.InfoDisplay.Width.Set( 0f, 1f );
			this.InfoDisplay.Height.Set( 0f, 1f );
			this.Append( this.InfoDisplay );

			//this.RefreshTheme();
			this.Recalculate();
		}


		////////////////

		public void SetText( string text ) {
			this.InfoDisplay.SetText( text );
		}


		////////////////

		/*public override void Draw( SpriteBatch sb ) {
			Rectangle rect = this.GetOuterDimensions().ToRectangle();
			rect.X += 4;
			rect.Y += 4;
			rect.Width -= 4;
			rect.Height -= 5;

			HudHelpers.DrawBorderedRect( sb, this.GetBgColor(), this.GetEdgeColor(), rect, 2 );

			base.Draw( sb );
		}*/
	}
}
