using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	internal class UIModTagButton : UITextPanelButton {
		public const int ColumnHeightTall = 27;
		public const int ColumnHeightShort = 7;
		public const int ColumnsInMid = 5;


		private readonly ModTagUI ModTagUI;

		public int Column;
		public int Row;

		public bool HasTag { get; private set; }


		////////////////
		
		public UIModTagButton( ModTagUI modtagui, bool has_tag, int pos, string label, string desc, float scale =1f, bool large=false )
				: base( UITheme.Vanilla, label, scale, large ) {
			this.ModTagUI = modtagui;
			this.HasTag = has_tag;

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
				this.ModTagUI.HoverElement.SetText( desc );
				this.ModTagUI.HoverElement.Left.Set( Main.mouseX, 0f );
				this.ModTagUI.HoverElement.Top.Set( Main.mouseY, 0f );
				this.ModTagUI.HoverElement.Recalculate();
				this.UpdateColor();
			};
			this.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( this.ModTagUI.HoverElement.Text == desc ) {
					this.ModTagUI.HoverElement.SetText( "" );
					this.ModTagUI.HoverElement.Recalculate();
				}
				this.UpdateColor();
			};

			this.RecalculatePos();
		}

		////////////////

		private void RecalculatePos() {
			float width = this.Width.Pixels;
			float left = (( ( Main.screenWidth / 2 ) - 296 ) - (width - 8)) + ( (width - 2) * this.Column );
			float top = (16 * this.Row) + 64;

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
