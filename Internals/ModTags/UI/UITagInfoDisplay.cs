using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ModTags.UI {
	internal class UITagInfoDisplay : UIPanel {
		public const int ColumnHeightTall = 31;
		public const int ColumnHeightShort = 8;
		public const int ColumnsInMid = 5;

		

		////////////////

		public int TagState { get; private set; }

		////////////////

		private readonly TagsMenuContextBase UIManager;
		private readonly UIText InfoDisplay;



		////////////////
		
		public UITagInfoDisplay( TagsMenuContextBase modtagui ) {
			this.TagState = 0;
			this.UIManager = modtagui;

			this.Width.Set( 600f, 0f );
			this.Height.Set( 24f, 0f );

			this.InfoDisplay = new UIText( "" );
			this.InfoDisplay.Width.Set( 0f, 1f );
			this.InfoDisplay.Height.Set( 0f, 1f );
			this.Append( this.InfoDisplay );

			//this.RefreshTheme();
			this.Recalculate();
		}


		////////////////

		private void RecalculatePos() {
			this.Left.Set( (Main.screenWidth / 2) - (this.Width.Pixels/2f), 0f );
			this.Top.Set( 160f, 0f );
		}

		public override void Recalculate() {
			this.Left.Set( ( Main.screenWidth / 2 ) - ( this.Width.Pixels / 2f ), 0f );
			this.Top.Set( 160f, 0f );

			base.Recalculate();
		}


		////////////////

		/*public override void RefreshTheme() {
			base.RefreshTheme();

			if( this.TagState > 0 ) {
				this.TextColor = Color.LimeGreen;
			} else if( this.TagState < 0 ) {
				this.TextColor = Color.Red;
			}
		}*/


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
