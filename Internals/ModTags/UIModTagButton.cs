using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	internal class UIModTagButton : UITextPanelButton {
		public const int ColumnHeightTall = 31;
		public const int ColumnHeightShort = 8;
		public const int ColumnsInMid = 5;


		private readonly ModTagUI ModTagUI;

		public int Column;
		public int Row;

		public bool HasTag { get; private set; }



		////////////////
		
		public UIModTagButton( ModTagUI modtagui, bool is_read_only, bool has_tag, int pos, string label, string desc, float scale=1f )
				: base( UITheme.Vanilla, label, scale, false ) {
			this.ModTagUI = modtagui;
			this.HasTag = has_tag;

			this.DrawPanel = false;
			
			if( is_read_only ) {
				this.Disable();
			}

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

			this.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( !this.IsEnabled ) { return; }

				this.ToggleTag();
			};
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
			this.UpdateColor();
		}


		////////////////

		private void RecalculatePos() {
			float width = this.Width.Pixels;
			float left = (( (Main.screenWidth / 2) - 296 ) - (width - 8)) + ( (width - 2) * this.Column );
			float top = (16 * this.Row) + 40;

			this.Left.Set( left, 0f );
			this.Top.Set( top, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}


		////////////////

		public void EnableTag() {
			if( this.HasTag ) { return; }
			this.HasTag = true;

			this.ModTagUI.SubUpButton.UpdateEnableState();
			this.UpdateColor();
		}

		public void DisableTag() {
			if( !this.HasTag ) { return; }
			this.HasTag = false;
			
			this.ModTagUI.SubUpButton.UpdateEnableState();
			this.UpdateColor();
		}

		public void ToggleTag() {
			this.HasTag = !this.HasTag;

			this.ModTagUI.SubUpButton.UpdateEnableState();
			this.UpdateColor();
		}


		////////////////

		public override void RefreshTheme() {
			base.RefreshTheme();

			if( this.HasTag ) {
				this.TextColor = Color.LimeGreen;
			}
		}

		private void UpdateColor() {
			if( this.HasTag ) {
				this.TextColor = Color.LimeGreen;
			} else {
				this.RefreshTheme();
			}
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			Color edge_color = !this.IsEnabled ?
				this.Theme.ButtonEdgeDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonEdgeLitColor :
					this.Theme.ButtonEdgeColor;
			Color bg_color = !this.IsEnabled ?
				this.Theme.ButtonBgDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonBgLitColor :
					this.Theme.ButtonBgColor;
			Rectangle rect = this.GetOuterDimensions().ToRectangle();
			rect.X += 4;
			rect.Y += 4;
			rect.Width -= 4;
			rect.Height -= 5;

			HudHelpers.DrawBorderedRect( sb, bg_color, edge_color, rect, 2 );

			base.Draw( sb );
		}
	}
}
