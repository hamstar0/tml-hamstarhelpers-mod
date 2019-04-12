using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	partial class UIControlPanel : UIState {
		private static object ModDataListLock = new object();

		private static IList<string> SupportMessages = new List<string> {
			"Buy me coffee for coding! :)",
			"Did you know I make other mods?",
			"Want more?",
			"Please support Mod Helpers!"
		};



		////////////////

		public bool IsOpen { get; private set; }

		private UITheme Theme = UITheme.Vanilla;
		private UserInterface Backend = null;

		////

		private IDictionary<string, UIPanel> Tabs;
		private string CurrentTab = "";

		////

		private UIElement OuterContainer = null;
		private UIPanel InnerContainer = null;
		private UITextPanelButton DialogClose = null;

		////

		private bool HasClicked = false;
		
		private bool SetDialogToClose = false;



		////////////////

		public UIControlPanel() {
			this.IsOpen = false;
			this.AwaitingReport = false;
			this.InitializeToggler();
		}

		////////////////

		public override void OnInitialize() {
			this.InitializeComponents();
			this.InitializeControlPanelComponents();

			this.CurrentTab = "Control Panel";
			this.Tabs[ this.CurrentTab ] = this.InnerContainer;
		}

		////

		public override void OnActivate() {
			base.OnActivate();

			if( this.CurrentTab == "Control Panel" ) {
				this.OnActivateControlPanel();
			}
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( !this.IsOpen ) { return; }

			if( Main.playerInventory || Main.npcChatText != "" ) {
				this.Close();
				return;
			}

			if( this.OuterContainer.IsMouseHovering ) {
				Main.LocalPlayer.mouseInterface = true;
			}

			if( this.CurrentTab == "Control Panel" ) {
				this.UpdateControlPanel();
			}
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
