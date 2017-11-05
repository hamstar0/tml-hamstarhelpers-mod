using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.UIHelpers {
	public static class UIFactoryHelpers {
		public static UITextPanel<string> CreateButton( UITheme theme, string label, float left, float top, float width ) {
			var button = new UITextPanel<string>( label );

			button.Width.Set( width, 0f );
			button.Left.Set( left, 0f );
			button.Top.Set( top, 0f );

			button.SetPadding( 5f );

			theme.ApplyButton( button );
			
			button.OnMouseOver += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				theme.ApplyButtonLit( button );
			};
			button.OnMouseOut += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				theme.ApplyButton( button );
			};

			return button;
		}
	}
}
