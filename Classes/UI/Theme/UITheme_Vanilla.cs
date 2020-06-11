using Microsoft.Xna.Framework;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Classes.UI.Theme {
	public partial class UITheme : ILoadable {
		/// <summary>
		/// Default "vanilla Terraria" UI theme presets.
		/// </summary>
		public static UITheme Vanilla;



		////////////////

		private void InitializeStatic() {
			UITheme.Vanilla = new UITheme();

			//Color defaultUIBlue = new Color( 73, 94, 171 );
			//Color defaultUIBlueMouseOver = new Color( 63, 82, 151 ) * 0.7f;

			////////////////

			UITheme.Vanilla.MainBgColor = new Color( 26, 40, 89 ) * 0.8f;
			UITheme.Vanilla.MainEdgeColor = new Color( 13, 20, 44 ) * 0.8f;
			UITheme.Vanilla.MainTextColor = Color.White;

			////////////////

			UITheme.Vanilla.HeadBgColor = new Color( 73, 94, 171 );
			UITheme.Vanilla.HeadEdgeColor = Color.Black;

			////////////////

			UITheme.Vanilla.ListBgColor = new Color( 33, 43, 79 ) * 0.8f;
			UITheme.Vanilla.ListEdgeColor = Color.Transparent;

			UITheme.Vanilla.ListItemBgColor = new Color( 63, 82, 151 ) * 0.7f;
			UITheme.Vanilla.ListItemEdgeColor = new Color( 89, 116, 213 ) * 0.7f;

			UITheme.Vanilla.ListItemBgLitColor = new Color( 73, 94, 171 );
			UITheme.Vanilla.ListItemEdgeLitColor = new Color( 89, 116, 213 );

			UITheme.Vanilla.ListItemBgSelectedColor = UITheme.Vanilla.ListItemBgLitColor;
			UITheme.Vanilla.ListItemEdgeSelectedColor = UITheme.Vanilla.ListItemEdgeLitColor;

			////////////////

			UITheme.Vanilla.InputBgColor = new Color( 63, 82, 151 ) * 0.7f;
			UITheme.Vanilla.InputEdgeColor = Color.Black;
			UITheme.Vanilla.InputTextColor = Color.White;

			UITheme.Vanilla.InputBgDisabledColor = Color.Lerp( UITheme.Vanilla.InputBgColor, Color.Gray, 0.25f );
			UITheme.Vanilla.InputEdgeDisabledColor = Color.Lerp( UITheme.Vanilla.InputEdgeColor, Color.Gray, 0.25f );
			UITheme.Vanilla.InputTextDisabledColor = Color.Lerp( UITheme.Vanilla.InputTextColor, Color.Gray, 0.25f );

			////////////////

			UITheme.Vanilla.ButtonBgColor = new Color( 63, 82, 151 ) * 0.7f;
			UITheme.Vanilla.ButtonEdgeColor = Color.Black;
			UITheme.Vanilla.ButtonTextColor = Color.White;

			UITheme.Vanilla.ButtonBgLitColor = new Color( 73, 94, 171 );
			UITheme.Vanilla.ButtonEdgeLitColor = Color.Black;
			UITheme.Vanilla.ButtonTextLitColor = Color.White;

			UITheme.Vanilla.ButtonBgDisabledColor = Color.Lerp( UITheme.Vanilla.ButtonBgColor, Color.Gray, 0.25f );
			UITheme.Vanilla.ButtonEdgeDisabledColor = Color.Lerp( UITheme.Vanilla.ButtonEdgeLitColor, Color.Gray, 0.25f );
			UITheme.Vanilla.ButtonTextDisabledColor = Color.Lerp( UITheme.Vanilla.ButtonTextLitColor, Color.Gray, 0.25f );

			////////////////

			UITheme.Vanilla.UrlColor = Color.Lerp( new Color( 80, 80, 255 ), Color.White, 0.5f );
			UITheme.Vanilla.UrlLitColor = Color.Lerp( new Color( 128, 128, 255 ), Color.White, 0.5f );
			UITheme.Vanilla.UrlVisitColor = Color.Lerp( new Color( 192, 0, 255 ), Color.White, 0.5f );
		}


		////////////////

		void ILoadable.OnModsLoad() {
			this.InitializeStatic();
		}

		void ILoadable.OnModsUnload() {
			UITheme.Vanilla = null;
		}

		void ILoadable.OnPostModsLoad() {
		}
	}
}
