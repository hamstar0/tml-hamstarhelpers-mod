using HamstarHelpers.Helpers.UIHelpers.Elements;
using HamstarHelpers.UIHelpers.Elements;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.UIHelpers {
	public class UITheme {
		////////////////
		public Color ModListBgColor = new Color( 0, 0, 0, 128 );
		public Color ModListEdgeColor = new Color( 32, 32, 32, 32 );
		public Color ModListItemBgColor = new Color( 64, 0, 16, 128 );
		public Color ModListItemEdgeColor = new Color( 224, 224, 224, 128 );
		public Color ModListItemBgLitColor = new Color( 112, 32, 48, 128 );
		public Color ModListItemEdgeLitColor = new Color( 255, 255, 255, 128 );
		public Color ModListItemBgSelectedColor = new Color( 160, 64, 80, 128 );
		public Color ModListItemEdgeSelectedColor = new Color( 224, 224, 224, 128 );
		////////////////

		
		
		public Color MainBgColor = new Color( 160, 0, 32, 192 );
		public Color MainEdgeColor = new Color( 224, 224, 224, 192 );

		////////////////

		public Color ListBgColor = new Color( 0, 0, 0, 128 );
		public Color ListEdgeColor = new Color( 32, 32, 32, 32 );

		public Color ListItemBgColor = new Color( 64, 0, 16, 128 );
		public Color ListItemEdgeColor = new Color( 224, 224, 224, 128 );

		public Color ListItemBgLitColor = new Color( 112, 32, 48, 128 );
		public Color ListItemEdgeLitColor = new Color( 255, 255, 255, 128 );

		public Color ListItemBgSelectedColor = new Color( 160, 64, 80, 128 );
		public Color ListItemEdgeSelectedColor = new Color( 224, 224, 224, 128 );

		////////////////

		public Color InputBgColor = new Color( 32, 0, 0, 128 );
		public Color InputEdgeColor = new Color( 224, 224, 224, 128 );
		public Color InputTextColor = new Color( 255, 255, 255, 255 );

		public Color InputBgDisabledColor = new Color( 80, 40, 56, 128 );
		public Color InputEdgeDisabledColor = new Color( 160, 160, 160, 128 );
		public Color InputTextDisabledColor = new Color( 128, 128, 128, 128 );

		////////////////

		public Color ButtonBgColor = new Color( 128, 0, 16, 128 );
		public Color ButtonEdgeColor = new Color( 224, 224, 224, 224 );
		public Color ButtonTextColor = new Color( 224, 224, 224, 224 );

		public Color ButtonBgLitColor = new Color( 160, 32, 48, 128 );
		public Color ButtonEdgeLitColor = new Color( 255, 255, 255, 255 );
		public Color ButtonTextLitColor = new Color( 255, 255, 255, 255 );

		public Color ButtonBgDisabledColor = new Color( 128, 80, 96, 128 );
		public Color ButtonEdgeDisabledColor = new Color( 160, 160, 160, 128 );
		public Color ButtonTextDisabledColor = new Color( 128, 128, 128, 96 );

		////////////////

		public Color UrlColor = new Color( 80, 80, 255 );
		public Color UrlLitColor = new Color( 128, 128, 255 );
		public Color UrlVisitColor = new Color( 192, 0, 255 );



		////////////////

		public virtual void ApplyPanel( UIPanel panel ) {
			panel.BackgroundColor = this.MainBgColor;
			panel.BorderColor = this.MainEdgeColor;
		}

		////////////////

		public virtual void ApplyInput( UITextField panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			panel.TextColor = this.InputTextColor;
		}
		public virtual void ApplyInput( UITextArea panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			panel.TextColor = this.InputTextColor;
		}
		
		public virtual void ApplyInputDisable( UITextArea panel ) {
			panel.BackgroundColor = this.InputBgDisabledColor;
			panel.BorderColor = this.InputEdgeDisabledColor;
			panel.TextColor = this.InputTextDisabledColor;
		}

		////////////////

		public virtual void ApplyButton( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgColor;
			panel.BorderColor = this.ButtonEdgeColor;
			panel.TextColor = this.ButtonTextColor;
		}

		public virtual void ApplyButtonLit( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgLitColor;
			panel.BorderColor = this.ButtonEdgeLitColor;
			panel.TextColor = this.ButtonTextLitColor;
		}

		public virtual void ApplyButtonDisable( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgDisabledColor;
			panel.BorderColor = this.ButtonEdgeDisabledColor;
			panel.TextColor = this.ButtonTextDisabledColor;
		}

		////////////////

		public virtual void ApplyList( UIPanel panel ) {
			panel.BackgroundColor = this.ListBgColor;
			panel.BorderColor = this.ListEdgeColor;
		}

		public virtual void ApplyListItem( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgColor;
			panel.BorderColor = this.ListItemEdgeColor;
		}

		public virtual void ApplyListItemLit( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgLitColor;
			panel.BorderColor = this.ListItemEdgeLitColor;
		}

		public virtual void ApplyListItemSelected( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgSelectedColor;
			panel.BorderColor = this.ListItemEdgeSelectedColor;
		}


		////////////////

		[System.Obsolete( "use UITheme.ApplyList", true )]
		public void ApplyModList( UIPanel panel ) {
			this.ApplyList( panel );
		}
		[System.Obsolete( "use UITheme.ApplyListItem", true )]
		public void ApplyModListItem( UIModData panel ) {
			this.ApplyListItem( panel );
		}
		[System.Obsolete( "use UITheme.ApplyListItemLit", true )]
		public void ApplyModListItemLit( UIModData panel ) {
			this.ApplyListItemLit( panel );
		}
		[System.Obsolete( "use UITheme.ApplyListItemSelected", true )]
		public void ApplyModListItemSelected( UIModData panel ) {
			this.ApplyListItemSelected( panel );
		}
	}
}
