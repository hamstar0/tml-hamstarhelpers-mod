using HamstarHelpers.Utilities.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;

namespace HamstarHelpers.UIHelpers {
	public class UITheme {
		public Color MainBgColor = new Color( 160, 0, 32, 192 );
		public Color MainEdgeColor = new Color( 224, 224, 224, 192 );

		public Color ModListBgColor = new Color( 0, 0, 0, 128 );
		public Color ModListEdgeColor = new Color( 32, 32, 32, 32 );

		public Color ModListItemBgColor = new Color( 64, 0, 16, 128 );
		public Color ModListItemBgLitColor = new Color( 96, 32, 48, 128 );
		public Color ModListItemBgSelectedColor = new Color( 160, 64, 80, 128 );
		public Color ModListItemEdgeColor = new Color( 224, 224, 224, 128 );
		public Color ModListItemEdgeLitColor = new Color( 255, 255, 255, 128 );
		public Color ModListItemEdgeSelectedColor = new Color( 224, 224, 224, 128 );

		public Color IssueInputBgColor = new Color( 128, 0, 16, 128 );
		public Color IssueInputEdgeColor = new Color( 224, 224, 224, 128 );

		public Color ButtonBgColor = new Color( 160, 32, 48, 128 );
		public Color ButtonBgLitColor = new Color( 128, 0, 16, 128 );
		public Color ButtonEdgeColor = new Color( 224, 224, 224, 128 );



		////////////////

		public void ApplyPanel( UIPanel panel ) {
			panel.BackgroundColor = this.MainBgColor;
			panel.BorderColor = this.MainEdgeColor;
		}

		////////////////

		public void ApplyButton( UITextPanel<string> panel ) {
			panel.BackgroundColor = this.ButtonBgColor;
			panel.BorderColor = this.ButtonEdgeColor;
		}

		public void ApplyButtonLit( UITextPanel<string> panel ) {
			panel.BackgroundColor = this.ButtonBgLitColor;
		}

		////////////////

		public void ApplyModList( UIPanel panel ) {
			panel.BackgroundColor = this.ModListBgColor;
			panel.BorderColor = this.ModListEdgeColor;
		}

		////////////////

		public void ApplyModListItem( UIModData panel ) {
			panel.BackgroundColor = this.ModListItemBgColor;
			panel.BorderColor = this.ModListItemEdgeColor;
		}

		public void ApplyModListItemLit( UIModData panel ) {
			panel.BackgroundColor = this.ModListItemBgLitColor;
			panel.BorderColor = this.ModListItemEdgeLitColor;
		}

		public void ApplyModListItemSelected( UIModData panel ) {
			panel.BackgroundColor = this.ModListItemBgSelectedColor;
			panel.BorderColor = this.ModListItemEdgeSelectedColor;
		}
	}
}
