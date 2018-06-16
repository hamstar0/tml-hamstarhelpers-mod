using HamstarHelpers.Helpers.UIHelpers.Elements;
using HamstarHelpers.UIHelpers.Elements;
using System;
using System.Reflection;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.UIHelpers {
	[Obsolete("HamstarHelpers.Components.UI.UITheme", true)]
	public partial class UITheme {
		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public UITheme Clone() {
			return (UITheme)this.MemberwiseClone();
		}

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public void Switch( UITheme new_theme ) {
			foreach( FieldInfo field in typeof( UITheme ).GetFields() ) {
				field.SetValue( this, field.GetValue( new_theme ) );
			}
		}


		////////////////

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyPanel( UIPanel panel ) {
			panel.BackgroundColor = this.MainBgColor;
			panel.BorderColor = this.MainEdgeColor;
		}

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyHeader( UIPanel panel ) {
			panel.BackgroundColor = this.HeadBgColor;
			panel.BorderColor = this.HeadEdgeColor;
		}

		////////////////

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyInput( UITextField panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			panel.TextColor = this.InputTextColor;
		}
		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyInput( UITextArea panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			panel.TextColor = this.InputTextColor;
		}

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyInputDisable( UITextArea panel ) {
			panel.BackgroundColor = this.InputBgDisabledColor;
			panel.BorderColor = this.InputEdgeDisabledColor;
			panel.TextColor = this.InputTextDisabledColor;
		}

		////////////////

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyButton( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgColor;
			panel.BorderColor = this.ButtonEdgeColor;
			panel.TextColor = this.ButtonTextColor;
		}

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyButtonLit( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgLitColor;
			panel.BorderColor = this.ButtonEdgeLitColor;
			panel.TextColor = this.ButtonTextLitColor;
		}

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyButtonDisable( UITextPanelButton panel ) {
			panel.BackgroundColor = this.ButtonBgDisabledColor;
			panel.BorderColor = this.ButtonEdgeDisabledColor;
			panel.TextColor = this.ButtonTextDisabledColor;
		}

		////////////////

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyListContainer( UIPanel panel ) {
			panel.BackgroundColor = this.ListBgColor;
			panel.BorderColor = this.ListEdgeColor;
		}

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyListItem( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgColor;
			panel.BorderColor = this.ListItemEdgeColor;
		}

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyListItemLit( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgLitColor;
			panel.BorderColor = this.ListItemEdgeLitColor;
		}

		[Obsolete( "HamstarHelpers.Components.UI.UITheme", true )]
		public virtual void ApplyListItemSelected( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgSelectedColor;
			panel.BorderColor = this.ListItemEdgeSelectedColor;
		}
	}
}
