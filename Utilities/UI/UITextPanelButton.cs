using HamstarHelpers.UIHelpers;
using Terraria.UI;


namespace HamstarHelpers.Utilities.UI {
	[System.Obsolete( "use UIHelpers.UI.UITextPanelButton", true )]
	public class UITextPanelButton : UIElement {
		private UIHelpers.Elements.UITextPanelButton TrueElement;

		public bool IsEnabled { get { return this.TrueElement.IsEnabled; } }


		////////////////

		public UITextPanelButton( UITheme theme, string label ) : base() {
			this.TrueElement = new UIHelpers.Elements.UITextPanelButton( theme, label );
			this.Append( this.TrueElement );

			CalculatedStyle dim = this.TrueElement.GetDimensions();
			this.Width.Set( dim.Width, 0f );
			this.Height.Set( dim.Height, 0f );
		}


		////////////////

		public void Enable() {
			this.TrueElement.Enable();
		}

		public void Disable() {
			this.TrueElement.Disable();
		}
	}
}
