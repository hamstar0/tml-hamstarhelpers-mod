using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	public class UITextPanelButton : UITextPanel<string> {
		private UITheme Theme;

		public bool IsEnabled { get; private set; }


		////////////////

		public UITextPanelButton( UITheme theme, string label, float scale = 1f, bool large = false ) : base( label, scale, large ) {
			this.Theme = theme;
			this.IsEnabled = true;

			this.SetPadding( 5f );

			var self = this;

			theme.ApplyButton( this );
			this.OnMouseOver += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !self.IsEnabled ) { return; }
				theme.ApplyButtonLit( self );
			};
			this.OnMouseOut += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !self.IsEnabled ) { return; }
				theme.ApplyButton( self );
			};

			this.RefreshTheme();
		}


		////////////////

		public void Enable() {
			this.IsEnabled = true;
			this.RefreshTheme();
		}

		public void Disable() {
			this.IsEnabled = false;
			this.RefreshTheme();
		}


		////////////////

		public virtual void RefreshTheme() {
			if( this.IsEnabled ) {
				this.Theme.ApplyButton( this );
			} else {
				this.Theme.ApplyButtonDisable( this );
			}
		}
	}
}
