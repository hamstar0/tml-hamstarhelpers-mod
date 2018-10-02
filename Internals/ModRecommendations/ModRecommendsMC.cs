using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ModRecommendations.UI;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModRecommendations {
	partial class ModRecommendsMenuContext {
		public string UIName => "UIModInfo";
		public string ContextName => "Mod Recommendations";

		////////////////

		protected UIState MyUI = null;
		
		internal UIRecommendsList RecommendsList;



		////////////////

		protected ModRecommendsMenuContext() {
			this.RecommendsList = new UIRecommendsList( this );

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Info Display", this.RecommendsList, false );
			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Set UI",
				ui => {
					this.MyUI = ui;
				},
				ui => {
					this.MyUI = null;
				}
			);
		}
	}
}
