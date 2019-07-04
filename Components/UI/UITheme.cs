using HamstarHelpers.Components.UI.Elements;
using System.Reflection;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Components.UI {
	/// <summary>
	/// Defines a theme to use for common UI color and layout settings. Default values are a Mod Helpers custom theme.
	/// </summary>
	public partial class UITheme {
		/// <returns>Clone of the current object.</returns>
		public UITheme Clone() {
			return (UITheme)this.MemberwiseClone();
		}

		/// <summary>
		/// Copies internal fields into another.
		/// </summary>
		/// <param name="newTheme">Theme to copy from.</param>
		public void Switch( UITheme newTheme ) {
			foreach( FieldInfo field in typeof( UITheme ).GetFields() ) {
				field.SetValue( this, field.GetValue( newTheme ) );
			}

			/*this.ButtonBgColor = newTheme.ButtonBgColor;
			this.ButtonBgDisabledColor = newTheme.ButtonBgDisabledColor;
			this.ButtonBgLitColor = newTheme.ButtonBgLitColor;
			this.ButtonEdgeColor = newTheme.ButtonEdgeColor;
			this.ButtonEdgeDisabledColor = newTheme.ButtonEdgeDisabledColor;
			this.ButtonEdgeLitColor = newTheme.ButtonEdgeLitColor;
			this.ButtonTextColor = newTheme.ButtonTextColor;
			this.ButtonTextDisabledColor = newTheme.ButtonTextDisabledColor;
			this.ButtonTextLitColor = newTheme.ButtonTextLitColor;
			this.HeadBgColor = newTheme.HeadBgColor;
			this.HeadEdgeColor = newTheme.HeadEdgeColor;
			this.InputBgColor = newTheme.InputBgColor;
			this.InputBgDisabledColor = newTheme.InputBgDisabledColor;
			this.InputEdgeColor = newTheme.InputEdgeColor;
			this.InputEdgeDisabledColor = newTheme.InputEdgeDisabledColor;
			this.InputTextColor = newTheme.InputTextColor;
			this.InputTextDisabledColor = newTheme.InputTextDisabledColor;
			this.ListBgColor = newTheme.ListBgColor;
			this.ListEdgeColor = newTheme.ListEdgeColor;
			this.ListItemBgColor = newTheme.ListItemBgColor;
			this.ListItemBgLitColor = newTheme.ListItemBgLitColor;
			this.ListItemEdgeColor = newTheme.ListItemEdgeColor;
			this.ListItemEdgeLitColor = newTheme.ListItemEdgeLitColor;
			this.ListItemEdgeSelectedColor = newTheme.ListItemEdgeSelectedColor;
			this.MainBgColor = newTheme.MainBgColor;
			this.MainEdgeColor = newTheme.MainEdgeColor;
			this.ModListBgColor = newTheme.ModListBgColor;
			this.ModListEdgeColor = newTheme.ModListEdgeColor;
			this.ModListItemBgColor = newTheme.ModListItemBgColor;
			this.ModListItemBgLitColor = newTheme.ModListItemBgLitColor;
			this.ModListItemBgSelectedColor = newTheme.ModListItemBgSelectedColor;
			this.ModListItemEdgeColor = newTheme.ModListItemEdgeColor;
			this.ModListItemEdgeLitColor = newTheme.ModListItemEdgeLitColor;
			this.ModListItemEdgeSelectedColor = newTheme.ModListItemEdgeSelectedColor;
			this.UrlColor = newTheme.UrlColor;
			this.UrlLitColor = newTheme.UrlLitColor;
			this.UrlVisitColor = newTheme.UrlVisitColor;*/
		}


		////////////////

		/// <summary>
		/// Applies standard panel theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyPanel( UIPanel panel ) {
			panel.BackgroundColor = this.MainBgColor;
			panel.BorderColor = this.MainEdgeColor;
		}

		/// <summary>
		/// Applies standard "header" panel theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyHeader( UIPanel panel ) {
			panel.BackgroundColor = this.HeadBgColor;
			panel.BorderColor = this.HeadEdgeColor;
		}

		////////////////

		/// <summary>
		/// Applies standard text input theming to a UI text field.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyInput( UITextField panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			panel.TextColor = this.InputTextColor;
		}

		/// <summary>
		/// Applies standard text input theming to a UI text area.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyInput( UITextArea panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			panel.TextColor = this.InputTextColor;
		}

		/// <summary>
		/// Applies standard disabled text input theming to a UI text area.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyInputDisable( UITextArea panel ) {
			panel.BackgroundColor = this.InputBgDisabledColor;
			panel.BorderColor = this.InputEdgeDisabledColor;
			panel.TextColor = this.InputTextDisabledColor;
		}

		////////////////

		/// <summary>
		/// Applies standard button theming to a UI text panel button.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyButton( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgColor;
			panel.BorderColor = this.ButtonEdgeColor;
			panel.TextColor = this.ButtonTextColor;
		}

		/// <summary>
		/// Applies standard 'lit' button theming to a UI text panel button.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyButtonLit( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgLitColor;
			panel.BorderColor = this.ButtonEdgeLitColor;
			panel.TextColor = this.ButtonTextLitColor;
		}

		/// <summary>
		/// Applies standard disabled button theming to a UI text panel button.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyButtonDisable( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgDisabledColor;
			panel.BorderColor = this.ButtonEdgeDisabledColor;
			panel.TextColor = this.ButtonTextDisabledColor;
		}

		////////////////

		/// <summary>
		/// Applies standard list container theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyListContainer( UIPanel panel ) {
			panel.BackgroundColor = this.ListBgColor;
			panel.BorderColor = this.ListEdgeColor;
		}

		/// <summary>
		/// Applies standard list item theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyListItem( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgColor;
			panel.BorderColor = this.ListItemEdgeColor;
		}

		/// <summary>
		/// Applies standard 'lit' list item theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyListItemLit( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgLitColor;
			panel.BorderColor = this.ListItemEdgeLitColor;
		}

		/// <summary>
		/// Applies standard 'selected' list item theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyListItemSelected( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgSelectedColor;
			panel.BorderColor = this.ListItemEdgeSelectedColor;
		}
	}
}
