using Microsoft.Xna.Framework;


namespace HamstarHelpers.UIHelpers {
	public partial class OldUITheme {
		public static OldUITheme Vanilla;

		static OldUITheme() {
			OldUITheme.Vanilla = new OldUITheme();

			//Color defaultUIBlue = new Color( 73, 94, 171 );
			//Color defaultUIBlueMouseOver = new Color( 63, 82, 151 ) * 0.7f;

			////////////////

			OldUITheme.Vanilla.MainBgColor = new Color( 26, 40, 89 ) * 0.8f;
			OldUITheme.Vanilla.MainEdgeColor = new Color( 13, 20, 44 ) * 0.8f;

			////////////////

			OldUITheme.Vanilla.HeadBgColor = new Color( 73, 94, 171 );
			OldUITheme.Vanilla.HeadEdgeColor = Color.Black;

			////////////////

			OldUITheme.Vanilla.ListBgColor = new Color( 33, 43, 79 ) * 0.8f;
			OldUITheme.Vanilla.ListEdgeColor = Color.Transparent;

			OldUITheme.Vanilla.ListItemBgColor = new Color( 63, 82, 151 ) * 0.7f;
			OldUITheme.Vanilla.ListItemEdgeColor = new Color( 89, 116, 213 ) * 0.7f;

			OldUITheme.Vanilla.ListItemBgLitColor = new Color( 73, 94, 171 );
			OldUITheme.Vanilla.ListItemEdgeLitColor = new Color( 89, 116, 213 );

			OldUITheme.Vanilla.ListItemBgSelectedColor = OldUITheme.Vanilla.ListItemBgLitColor;
			OldUITheme.Vanilla.ListItemEdgeSelectedColor = OldUITheme.Vanilla.ListItemEdgeLitColor;

			////////////////

			OldUITheme.Vanilla.InputBgColor = new Color( 63, 82, 151 ) * 0.7f;
			OldUITheme.Vanilla.InputEdgeColor = Color.Black;
			OldUITheme.Vanilla.InputTextColor = Color.White;

			OldUITheme.Vanilla.InputBgDisabledColor = Color.Lerp( OldUITheme.Vanilla.InputBgColor, Color.Gray, 0.25f );
			OldUITheme.Vanilla.InputEdgeDisabledColor = Color.Lerp( OldUITheme.Vanilla.InputEdgeColor, Color.Gray, 0.25f );
			OldUITheme.Vanilla.InputTextDisabledColor = Color.Lerp( OldUITheme.Vanilla.InputTextColor, Color.Gray, 0.25f );

			////////////////

			OldUITheme.Vanilla.ButtonBgColor = new Color( 63, 82, 151 ) * 0.7f;
			OldUITheme.Vanilla.ButtonEdgeColor = Color.Black;
			OldUITheme.Vanilla.ButtonTextColor = Color.White;

			OldUITheme.Vanilla.ButtonBgLitColor = new Color( 73, 94, 171 );
			OldUITheme.Vanilla.ButtonEdgeLitColor = Color.Black;
			OldUITheme.Vanilla.ButtonTextLitColor = Color.White;

			OldUITheme.Vanilla.ButtonBgDisabledColor = Color.Lerp( OldUITheme.Vanilla.ButtonBgColor, Color.Gray, 0.25f );
			OldUITheme.Vanilla.ButtonEdgeDisabledColor = Color.Lerp( OldUITheme.Vanilla.ButtonEdgeLitColor, Color.Gray, 0.25f );
			OldUITheme.Vanilla.ButtonTextDisabledColor = Color.Lerp( OldUITheme.Vanilla.ButtonTextLitColor, Color.Gray, 0.25f );

			////////////////

			OldUITheme.Vanilla.UrlColor = Color.Lerp( new Color( 80, 80, 255 ), Color.White, 0.5f );
			OldUITheme.Vanilla.UrlLitColor = Color.Lerp( new Color( 128, 128, 255 ), Color.White, 0.5f );
			OldUITheme.Vanilla.UrlVisitColor = Color.Lerp( new Color( 192, 0, 255 ), Color.White, 0.5f );
		}
	}
}
