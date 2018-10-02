using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ModTags.UI {
	internal class UIInfoDisplay : UIMenuPanel {
		private readonly TagsMenuContextBase MenuContext;
		private readonly UIText TextElem;



		////////////////

		public UIInfoDisplay( TagsMenuContextBase menu_context )
				: base( UITheme.Vanilla, 800f, 40f, -400f, 2f ) {
			this.MenuContext = menu_context;

			this.TextElem = new UIText( "" );
			this.TextElem.Width.Set( 0f, 1f );
			this.TextElem.Height.Set( 0f, 1f );
			this.Append( this.TextElem );

			//this.RefreshTheme();
			this.Recalculate();
		}


		////////////////
		
		public void SetText( string text, Color? color=null ) {
			this.TextElem.TextColor = color??Color.White;
			this.TextElem.SetText( text );
		}

		public string GetText() {
			return this.TextElem.Text;
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
