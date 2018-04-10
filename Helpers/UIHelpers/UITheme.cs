using HamstarHelpers.Helpers.UIHelpers.Elements;
using HamstarHelpers.UIHelpers.Elements;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace HamstarHelpers.UIHelpers {
	public partial class UITheme {
		public UITheme Clone() {
			return (UITheme)this.MemberwiseClone();
		}

		public void Switch( UITheme new_theme ) {
			foreach( FieldInfo field in typeof( UITheme ).GetFields() ) {
				field.SetValue( this, field.GetValue( new_theme ) );
			}

			/*this.ButtonBgColor = new_theme.ButtonBgColor;
			this.ButtonBgDisabledColor = new_theme.ButtonBgDisabledColor;
			this.ButtonBgLitColor = new_theme.ButtonBgLitColor;
			this.ButtonEdgeColor = new_theme.ButtonEdgeColor;
			this.ButtonEdgeDisabledColor = new_theme.ButtonEdgeDisabledColor;
			this.ButtonEdgeLitColor = new_theme.ButtonEdgeLitColor;
			this.ButtonTextColor = new_theme.ButtonTextColor;
			this.ButtonTextDisabledColor = new_theme.ButtonTextDisabledColor;
			this.ButtonTextLitColor = new_theme.ButtonTextLitColor;
			this.HeadBgColor = new_theme.HeadBgColor;
			this.HeadEdgeColor = new_theme.HeadEdgeColor;
			this.InputBgColor = new_theme.InputBgColor;
			this.InputBgDisabledColor = new_theme.InputBgDisabledColor;
			this.InputEdgeColor = new_theme.InputEdgeColor;
			this.InputEdgeDisabledColor = new_theme.InputEdgeDisabledColor;
			this.InputTextColor = new_theme.InputTextColor;
			this.InputTextDisabledColor = new_theme.InputTextDisabledColor;
			this.ListBgColor = new_theme.ListBgColor;
			this.ListEdgeColor = new_theme.ListEdgeColor;
			this.ListItemBgColor = new_theme.ListItemBgColor;
			this.ListItemBgLitColor = new_theme.ListItemBgLitColor;
			this.ListItemEdgeColor = new_theme.ListItemEdgeColor;
			this.ListItemEdgeLitColor = new_theme.ListItemEdgeLitColor;
			this.ListItemEdgeSelectedColor = new_theme.ListItemEdgeSelectedColor;
			this.MainBgColor = new_theme.MainBgColor;
			this.MainEdgeColor = new_theme.MainEdgeColor;
			this.ModListBgColor = new_theme.ModListBgColor;
			this.ModListEdgeColor = new_theme.ModListEdgeColor;
			this.ModListItemBgColor = new_theme.ModListItemBgColor;
			this.ModListItemBgLitColor = new_theme.ModListItemBgLitColor;
			this.ModListItemBgSelectedColor = new_theme.ModListItemBgSelectedColor;
			this.ModListItemEdgeColor = new_theme.ModListItemEdgeColor;
			this.ModListItemEdgeLitColor = new_theme.ModListItemEdgeLitColor;
			this.ModListItemEdgeSelectedColor = new_theme.ModListItemEdgeSelectedColor;
			this.UrlColor = new_theme.UrlColor;
			this.UrlLitColor = new_theme.UrlLitColor;
			this.UrlVisitColor = new_theme.UrlVisitColor;*/
		}


		////////////////

		public virtual void ApplyPanel( UIPanel panel ) {
			panel.BackgroundColor = this.MainBgColor;
			panel.BorderColor = this.MainEdgeColor;
		}

		public virtual void ApplyHeader( UIPanel panel ) {
			panel.BackgroundColor = this.HeadBgColor;
			panel.BorderColor = this.HeadEdgeColor;
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

		[System.Obsolete( "use UITheme.ApplyListContainer", true )]
		public virtual void ApplyList( UIPanel panel ) {
			this.ApplyListContainer( panel );
		}

		public virtual void ApplyListContainer( UIPanel panel ) {
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
	}
}
