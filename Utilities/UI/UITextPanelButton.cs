using HamstarHelpers.UIHelpers;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace HamstarHelpers.Utilities.UI {
	public class UITextPanelButton : UITextPanel<string> {
		private UITheme Theme;

		public bool IsEnabled { get; private set; }


		////////////////

		public UITextPanelButton( UITheme theme, string label ) : base( label ) {
			this.Theme = theme;
			this.IsEnabled = true;

			this.SetPadding( 5f );

			var self = this;

			theme.ApplyButton( this );
			this.OnMouseOver += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !self.IsEnabled ) { return; }
				theme.ApplyButtonLit( this );
			};
			this.OnMouseOut += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !self.IsEnabled ) { return; }
				theme.ApplyButton( this );
			};
		}


		////////////////

		public void Enable() {
			this.IsEnabled = true;
			this.Theme.ApplyButton( this );
		}

		public void Disable() {
			this.IsEnabled = false;
			this.Theme.ApplyButtonDisable( this );
		}
	}
}
