using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ModRecommendations.UI {
	internal class UIRecommendsList : UIMenuPanel {
		private readonly ModRecommendsMenuContext UIManager;

		private readonly UIText Label;
		private readonly UIList List;



		////////////////

		public UIRecommendsList( ModRecommendsMenuContext mc )
				: base( UITheme.Vanilla, 800f, 40f, -400f, 2f ) {
			this.UIManager = mc;

			this.Label = new UIText( "This mod recommends:" );
			this.Label.Left.Set( ((float)Main.screenWidth * 0.5f) + 300f, 0f );
			this.Label.Top.Set( 160f, 0f );
			this.Append( this.Label );

			this.List = new UIList();
			this.List.Left.Set( ((float)Main.screenWidth * 0.5f) + 300f, 0f );
			this.List.Top.Set( 180f, 0f );
			this.Append( this.List );

			this.Recalculate();
		}
	}
}
