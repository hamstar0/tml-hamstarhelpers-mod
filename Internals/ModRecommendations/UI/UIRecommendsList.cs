﻿using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ModRecommendations.UI {
	internal class UIRecommendsList : UIMenuPanel {
		private readonly ModRecommendsMenuContext MenuContext;

		private readonly UIText Label;
		private readonly UIList List;



		////////////////

		public UIRecommendsList( ModRecommendsMenuContext mc )
				: base( UITheme.Vanilla, 160f, 400f, 300f, 160f ) {
			this.MenuContext = mc;

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


		////////////////

		public void Clear() {
			this.List.Clear();
		}

		////////////////

		public void AddModEntry( string mod_name, string why ) {
			//var mod_entry = new UIPanel

			//this.List.Append( mod_entry );
		}
	}
}
