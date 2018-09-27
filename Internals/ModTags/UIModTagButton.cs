using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	internal class UIModTagButton : UITextPanelButton {
		public const int ColumnHeightTall = 31;
		public const int ColumnHeightShort = 8;
		public const int ColumnsInMid = 5;

		

		////////////////

		public int TagState { get; private set; }

		////////////////

		private readonly ModTagsUI ModTagUI;
		private bool CanDisableTags;

		public int Column;
		public int Row;



		////////////////
		
		public UIModTagButton( ModTagsUI modtagui, int pos, string label, string desc, bool can_disable_tags )
				: base( UITheme.Vanilla, label, 0.6f, false ) {
			this.TagState = 0;
			this.ModTagUI = modtagui;
			this.CanDisableTags = can_disable_tags;
			this.DrawPanel = false;

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
				this.ToggleEnableTag( this.ModTagUI.MyUI );
			};
			this.OnRightClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( !this.IsEnabled || !this.CanDisableTags ) { return; }
				this.ToggleDisableTag( this.ModTagUI.MyUI );
			};
			this.OnMouseOver += ( UIMouseEvent evt, UIElement listeningElement ) => {
				this.ModTagUI.HoverElement.SetText( desc );
				this.ModTagUI.HoverElement.Left.Set( Main.mouseX, 8f );
				this.ModTagUI.HoverElement.Top.Set( Main.mouseY, 8f );
				this.ModTagUI.HoverElement.Recalculate();
				this.RefreshTheme();
			};
			this.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( this.ModTagUI.HoverElement.Text == desc ) {
					this.ModTagUI.HoverElement.SetText( "" );
					this.ModTagUI.HoverElement.Recalculate();
				}
				this.RefreshTheme();
			};

			this.Disable();
			this.RecalculatePos();
			this.RefreshTheme();
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

		public void SetTagState( UIState ui, int state ) {
			if( this.TagState == state ) { return; }
			this.TagState = state;

			this.ModTagUI.OnTagStateChange( ui, this );
			this.RefreshTheme();
		}

		public void ToggleEnableTag( UIState ui ) {
			this.TagState = this.TagState <= 0 ? 1 : 0;

			this.ModTagUI.OnTagStateChange( ui, this );
			this.RefreshTheme();
		}

		public void ToggleDisableTag( UIState ui ) {
			this.TagState = this.TagState >= 0 ? -1 : 0;

			this.ModTagUI.OnTagStateChange( ui, this );
			this.RefreshTheme();
		}


		////////////////

		public override void RefreshTheme() {
			base.RefreshTheme();

			if( this.TagState > 0 ) {
				this.TextColor = Color.LimeGreen;
			} else if( this.TagState < 0 ) {
				this.TextColor = Color.Red;
			}
		}


		////////////////

		public Color GetBgColor() {
			Color bg_color = !this.IsEnabled ?
				this.Theme.ButtonBgDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonBgLitColor :
					this.Theme.ButtonBgColor;
			byte a = bg_color.A;
			
			if( this.Text.Contains("Mechanics:") ) {
				bg_color = Color.Lerp( bg_color, Color.Gold, 0.4f );
			} else if( this.Text.Contains("Theme:") ) {
				bg_color = Color.Lerp( bg_color, Color.DarkTurquoise, 0.4f );
			} else if( this.Text.Contains( "Content:" ) ) {
				bg_color = Color.Lerp( bg_color, Color.DarkRed, 0.4f );
			} else if( this.Text.Contains( "Where:" ) ) {
				bg_color = Color.Lerp( bg_color, Color.Green, 0.4f );
			} else if( this.Text.Contains( "When:" ) ) {
				bg_color = Color.Lerp( bg_color, Color.DarkViolet, 0.4f );
			}
			bg_color.A = a;

			return bg_color;
		}

		public Color GetEdgeColor() {
			Color edge_color = !this.IsEnabled ?
				this.Theme.ButtonEdgeDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonEdgeLitColor :
					this.Theme.ButtonEdgeColor;
			byte a = edge_color.A;
			
			if( this.Text.Contains( "Mechanics:" ) ) {
				edge_color = Color.Lerp( edge_color, Color.Goldenrod, 0.35f );
			} else if( this.Text.Contains( "Theme:" ) ) {
				edge_color = Color.Lerp( edge_color, Color.Aquamarine, 0.25f );
			} else if( this.Text.Contains( "Content:" ) ) {
				edge_color = Color.Lerp( edge_color, Color.Red, 0.25f );
			} else if( this.Text.Contains( "Where:" ) ) {
				edge_color = Color.Lerp( edge_color, Color.Green, 0.25f );
			} else if( this.Text.Contains( "When:" ) ) {
				edge_color = Color.Lerp( edge_color, Color.Purple, 0.25f );
			}
			edge_color.A = a;

			return edge_color;
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			Rectangle rect = this.GetOuterDimensions().ToRectangle();
			rect.X += 4;
			rect.Y += 4;
			rect.Width -= 4;
			rect.Height -= 5;

			HudHelpers.DrawBorderedRect( sb, this.GetBgColor(), this.GetEdgeColor(), rect, 2 );

			base.Draw( sb );
		}
	}
}
