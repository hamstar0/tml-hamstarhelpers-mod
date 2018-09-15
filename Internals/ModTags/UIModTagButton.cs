using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	internal class UIModTagButton : UITextPanelButton {
		public const int ColumnHeightTall = 31;
		public const int ColumnHeightShort = 8;
		public const int ColumnsInMid = 5;

		public int Column;
		public int Row;

		public bool HasTag { get; private set; }


		////////////////
		
		public UIModTagButton( int pos, string label, string desc, UIText hover_elem, float scale =1f, bool large=false )
				: base( UITheme.Vanilla, label, scale, large ) {
			this.HasTag = false;

			this.UpdateColor();

			int col_tall = UIModTagButton.ColumnHeightTall;
			int col_short = UIModTagButton.ColumnHeightShort;
			int cols_in_mid = UIModTagButton.ColumnsInMid;

			if( pos < col_tall ) {
				this.Column = 0;
				this.Row = pos;
			} else if( pos > ( col_tall + ( col_short * cols_in_mid ) ) ) {
				this.Column = 7;
				this.Row = pos - ( col_tall + ( col_short * cols_in_mid ) );
			} else {
				this.Column = 1 + (( pos - col_tall ) / col_short );
				this.Row = ( pos - col_tall ) % col_short;
			}

			this.Width.Set( 120f, 0f );
			this.Height.Set( 16f, 0f );
			this.OnMouseOver += ( UIMouseEvent evt, UIElement listeningElement ) => {
				hover_elem.SetText( desc );
				hover_elem.Left.Set( Main.mouseX, 0f );
				hover_elem.Top.Set( Main.mouseY, 0f );
				hover_elem.Recalculate();
				this.UpdateColor();
			};
			this.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( hover_elem.Text == desc ) {
					hover_elem.SetText( "" );
					hover_elem.Recalculate();
				}
				this.UpdateColor();
			};

			this.RecalculatePos();
		}

		////////////////

		private void RecalculatePos() {
			float width = this.Width.Pixels;
			float left = (( ( Main.screenWidth / 2 ) - 296 ) - (width - 8)) + ( (width - 2) * this.Column );
			float top = (16 * this.Row) + 48;

			this.Left.Set( left, 0f );
			this.Top.Set( top, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}


		////////////////

		public override void Draw( SpriteBatch spriteBatch ) {
			base.Draw( spriteBatch );
		}


		////////////////

		public void ToggleTag() {
			this.HasTag = !this.HasTag;

			this.UpdateColor();
		}

		private void UpdateColor() {
			this.TextColor = this.HasTag ?
				Color.LimeGreen :
				Color.DarkGray;
		}
	}
}
