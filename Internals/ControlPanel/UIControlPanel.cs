using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	partial class UIControlPanel : UIState {
		private UserInterface Backend = null;

		////

		private IDictionary<string, UIPanel> Tabs = new Dictionary<string, UIPanel>();
		private string CurrentTabName = "";

		////

		private UIElement OuterContainer = null;
		private UIPanel InnerContainer = null;

		////

		private bool HasClicked = false;


		////////////////

		public bool IsOpen { get; private set; }
		public UIPanel CurrentTab => this.Tabs.GetOrDefault( this.CurrentTabName );



		////////////////

		public UIControlPanel() {
			this.IsOpen = false;
			this.InitializeToggler();
		}

		////////////////

		public override void OnInitialize() {
			this.InitializeComponents();

			this.CurrentTabName = "Mod Control Panel";
			this.Tabs[ this.CurrentTabName ] = this.InnerContainer;
		}


		////////////////

		public UIPanel GetTab( string name ) {
			return this.Tabs.GetOrDefault( name );
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
