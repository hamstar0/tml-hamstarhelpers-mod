using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	public abstract class UIControlPanelTab : UIPanel {
		public UITheme Theme { get; protected set; }
		public bool IsInitialized { get; private set; }



		////////////////

		public sealed override void OnInitialize() {
			this.OnInitializeMe();
			this.IsInitialized = true;
		}


		public abstract void OnInitializeMe();
	}




	partial class UIControlPanel : UIState {
		public UIControlPanelTab GetTab( string name ) {
			return this.Tabs.GetOrDefault( name );
		}


		////////////////

		public void AddTab( string title, UIControlPanelTab tab ) {
			this.Tabs[ title ] = tab;
			this.TabTitleOrder[ title ] = this.TabTitleOrder.Count;

			if( this.IsInitialized ) {
				this.InitializeTab( title, tab );
			}
		}

		////

		private void InitializeTab( string title, UIControlPanelTab tab ) {
			tab.Width.Set( 0f, 1f );
			tab.Height.Set( 0f, 1f );

			this.AddTabCloseButton( title );
			this.AddTabButton( title );
		}


		////////////////

		private void AddTabCloseButton( string title ) {
			UIControlPanelTab tab = this.Tabs[title];
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
			UIControlPanelTab tab = this.Tabs[ title ];
			int idx = this.TabTitleOrder[ title ];

			int posX = UIControlPanel.TabWidth * idx;

			var button = new UITextPanelButton( tab.Theme, title );
			button.Left.Set( (float)posX, 0f );
			button.Top.Set( -UIControlPanel.TabHeight, 0f );
			button.Width.Set( UIControlPanel.TabWidth, 0f );
			button.Height.Set( UIControlPanel.TabHeight, 0f );
			button.OnClick += ( _, __ ) => {
				this.ChangeToTab( title );
			};

			this.OuterContainer.Append( button );

			this.TabButtons.Add( button );
			this.TabButtonHover.Add( false );
		}


		////////////////

		public bool ChangeToTab( string tabName ) {
			if( tabName == this.CurrentTabName ) {
				return true;
			}

			UIControlPanelTab tab;
			if( !this.Tabs.TryGetValue(tabName, out tab) ) {
				return false;
			}

			tab.Width.Set( 0f, 1f );
			tab.Height.Set( 0f, 1f );

			this.OuterContainer.RemoveChild( this.InnerContainer );
			this.InnerContainer.Remove();

			this.InnerContainer = tab;
			this.OuterContainer.Append( this.InnerContainer );

			if( !tab.IsInitialized ) {
				tab.Initialize();
			}

			this.Recalculate();
			this.OuterContainer.Recalculate();
			this.InnerContainer.Recalculate();

			this.CurrentTabName = tabName;
			
			return true;
		}
	}
}
