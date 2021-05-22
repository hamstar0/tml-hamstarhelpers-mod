using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Extensions;
using HamstarHelpers.Libraries.DotNET.Reflection;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;


namespace HamstarHelpers.Internals.ControlPanel {
	/// @private
	partial class UIControlPanel : UIState {
		public const string DefaultTabName = "Mod Control Panel";

		////////////////

		public static int TabWidth => 160;
		public static int TabHeight => 24;



		////////////////

		private UserInterface Backend = null;

		////

		private IDictionary<string, UIControlPanelTab> Tabs = new Dictionary<string, UIControlPanelTab>();
		private IDictionary<string, int> TabTitleOrder = new Dictionary<string, int>();

		private IList<UITextPanelButton> TabButtons = new List<UITextPanelButton>();
		private IList<bool> TabButtonHover = new List<bool>();


		////

		private UITheme Theme = UITheme.Vanilla;
		private UIElement OuterContainer = null;
		private UIPanel InnerContainer = null;

		////

		private bool IsInitialized = false;
		private bool HasClicked = false;


		////////////////

		public string CurrentTabName { get; private set; } = "";

		public UIControlPanelTab CurrentTab => this.Tabs.GetOrDefault( this.CurrentTabName );
		public UIModControlPanelTab DefaultTab => (UIModControlPanelTab)this.Tabs[UIControlPanel.DefaultTabName];
		
		public bool IsOpen { get; private set; }



		////////////////

		public UIControlPanel() {
			this.CurrentTabName = UIControlPanel.DefaultTabName;
			this.Tabs[ this.CurrentTabName ] = new UIModControlPanelTab( this.Theme );
			this.TabTitleOrder[ this.CurrentTabName ] = this.TabTitleOrder.Count;

			this.IsOpen = false;
			this.InitializeToggler();
		}

		////////////////

		public override void OnInitialize() {
			this.InitializeComponents();
		}


		////////////////

		public void UpdateGlobal() {
			if( !Main.inFancyUI ) {
				this.IsOpen = false;
			}
		}

		public override void Update( GameTime gameTime ) {
			if( !this.IsOpen ) { return; }

			if( Main.playerInventory || Main.npcChatText != "" ) {
				this.Close();
				return;
			}

			if( this.OuterContainer.IsMouseHovering ) {
				Main.LocalPlayer.mouseInterface = true;
			}

			base.Update( gameTime );
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			if( !this.IsOpen ) { return; }

			for( int i=0; i<this.TabButtons.Count; i++ ) {
				this.ApplyTabButtonMouseInteractivity( i );
			}

			base.Draw( sb );
		}


		////////////////

		private void ApplyTabButtonMouseInteractivity( int idx ) {
			UITextPanelButton button = this.TabButtons[idx];

			if( !button.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
				if( this.TabButtonHover[idx] ) {
					this.TabButtonHover[idx] = false;

					Timers.RunNow( () => {
						button.MouseOut( new UIMouseEvent( button, new Vector2( Main.mouseX, Main.mouseY ) ) );
					} );
				}
				return;
			}

			if( !this.TabButtonHover[idx] ) {
				this.TabButtonHover[idx] = true;
				ReflectionLibraries.Set( button, "_isMouseHovering", true );
			}

			var evt = new UIMouseEvent( button, new Vector2( Main.mouseX, Main.mouseY ) );

			Timers.RunNow( () => {
				button.MouseOver( evt );
			} );

			if( Main.mouseLeft && Main.mouseLeftRelease ) {
				Timers.RunNow( () => {
					button.Click( evt );
				} );
			}
		}
	}
}
