using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	partial class UIControlPanel : UIState {
		public const string DefaultTabName = "Mod Control Panel";



		////////////////

		private UserInterface Backend = null;

		////

		private IDictionary<string, UIControlPanelTab> Tabs = new Dictionary<string, UIControlPanelTab>();
		private string CurrentTabName = "";
		private IList<UITextPanelButton> TabButtons = new List<UITextPanelButton>();

		////

		private UITheme Theme = UITheme.Vanilla;
		private UIElement OuterContainer = null;
		private UIPanel InnerContainer = null;

		////

		private bool HasClicked = false;


		////////////////

		public bool IsOpen { get; private set; }



		////////////////

		public UIControlPanel() {
			this.CurrentTabName = UIControlPanel.DefaultTabName;
			this.Tabs[ this.CurrentTabName ] = new UIModControlPanelTab( this.Theme );

			this.IsOpen = false;
			this.InitializeToggler();
		}

		////////////////

		public override void OnInitialize() {
			this.InitializeComponents();
		}


		////////////////

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

		public void RecalculateMe() {
			if( this.Backend != null ) {
				this.Backend.Recalculate();
			} else {
				this.Recalculate();
			}
		}

		public override void Recalculate() {
			base.Recalculate();

			if( this.OuterContainer != null ) {
				this.RecalculateContainer();
			}
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			if( !this.IsOpen ) { return; }

			base.Draw( sb );
		}
	}
}
