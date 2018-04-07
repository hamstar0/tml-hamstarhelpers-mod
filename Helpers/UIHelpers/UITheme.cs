using HamstarHelpers.Helpers.UIHelpers.Elements;
using HamstarHelpers.UIHelpers.Elements;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.UIHelpers {
	public partial class UITheme {
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
	}
}
