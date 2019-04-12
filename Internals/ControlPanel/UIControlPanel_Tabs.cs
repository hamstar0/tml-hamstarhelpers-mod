using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	public abstract class UIControlPanelTab : UIPanel {
		public UITheme Theme { get; protected set; }
	}




	partial class UIControlPanel : UIState {
		public static int TabWidth => 128;
		public static int TabHeight => 24;
		


		////////////////

		public UIControlPanelTab CurrentTab => this.Tabs.GetOrDefault( this.CurrentTabName );
		public UIModControlPanelTab DefaultTab => (UIModControlPanelTab)this.Tabs[ UIControlPanel.DefaultTabName ];



		////////////////

		public UIPanel GetTab( string name ) {
			return this.Tabs.GetOrDefault( name );
		}

		////

		public void AddTab( string title, UIControlPanelTab tab ) {
			this.Tabs[ title ] = tab;

			this.AddTabCloseButton( tab );

			if( this.Tabs.Count > 1 ) {
				if( this.TabButtons.Count == 0 ) {
					this.AddTabButton( UIControlPanel.DefaultTabName );
				}
				this.AddTabButton( title );
			}
		}

		////////////////

		private void AddTabCloseButton( UIControlPanelTab tab ) {
			var closeButton = new UITextPanelButton( tab.Theme, "X" );

			closeButton.Top.Set( -8f, 0f );
			closeButton.Left.Set( -16f, 1f );
			closeButton.Width.Set( 24f, 0f );
			closeButton.Height.Set( 24f, 0f );

			closeButton.OnClick += ( _, __ ) => {
				this.Close();
				Main.PlaySound( SoundID.MenuClose );
			};
			closeButton.OnMouseOver += ( _, __ ) => {
				tab.Theme.ApplyButtonLit( closeButton );
			};
			closeButton.OnMouseOut += ( _, __ ) => {
				tab.Theme.ApplyButton( closeButton );
			};

			tab.Append( closeButton );
		}

		private void AddTabButton( string title ) {
			UIControlPanelTab tab = this.Tabs[title];
			int posX = UIControlPanel.TabWidth * ( this.Tabs.Count - 1 );

			var button = new UITextPanelButton( tab.Theme, title );
			button.Top.Set( -UIControlPanel.TabHeight, 0f );
			button.Left.Set( (float)posX, 0f );
			button.Width.Set( UIControlPanel.TabWidth, 0f );
			button.Height.Set( UIControlPanel.TabHeight, 0f );

			this.OuterContainer.Append( button );
			this.TabButtons.Add( button );
		}
	}
}
