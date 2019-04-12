using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	public interface UIControlPanelTab {
		UITheme Theme { get; }
		void Append( UIElement button );
	}




	partial class UIControlPanel : UIState {
		public UIPanel CurrentTab => this.Tabs.GetOrDefault( this.CurrentTabName );

		public UIModControlPanel DefaultTab => (UIModControlPanel)this.Tabs.GetOrDefault( UIControlPanel.DefaultTabName );



		////////////////

		public UIPanel GetTab( string name ) {
			return this.Tabs.GetOrDefault( name );
		}

		public void AddTab( string title, UIControlPanelTab tabBody ) {
			this.Tabs[ title ] = tabBody as UIPanel;
		}
	}
}
