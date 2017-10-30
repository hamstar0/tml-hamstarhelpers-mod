using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI : UIState {
		public bool IsTogglerLit { get; private set; }

		
		////////////////

		private void InitializeToggler() {
			this.IsTogglerLit = false;
		}


		////////////////

		public bool IsTogglerShown() {
			return Main.playerInventory;
		}


		////////////////

		public void UpdateToggler() {
			if( this.IsTogglerLit ) {
				Main.LocalPlayer.mouseInterface = true;
			}
		}


		////////////////

		public void DrawToggler( SpriteBatch sb ) {
			if( !this.IsTogglerShown() ) { return; }

			Texture2D tex;
			Color color;

			if( this.IsTogglerLit ) {
				tex = ControlPanelUI.ControlPanelLabelLit;
				color = new Color( 192, 192, 192, 192 );
			} else {
				tex = ControlPanelUI.ControlPanelLabel;
				color = new Color( 160, 160, 160, 160 );
			}

			sb.Draw( tex, ControlPanelUI.TogglerPosition, null, color );
		}


		////////////////
		
		public void CheckTogglerMouseInteraction() {
			bool is_click = Main.mouseLeft && Main.mouseLeftRelease;
			Vector2 pos = ControlPanelUI.TogglerPosition;
			Vector2 size = ControlPanelUI.ControlPanelLabel.Size();

			this.IsTogglerLit = false;

			if( this.IsTogglerShown() ) {
				if( Main.mouseX >= pos.X && Main.mouseX < (pos.X + size.X) ) {
					if( Main.mouseY >= pos.Y && Main.mouseY < (pos.Y + size.Y) ) {
						if( is_click && !this.HasClicked ) {
							if( this.IsOpen ) {
								this.Close();
							} else if( this.CanOpen() ) {
								this.Open();
							}
						}

						this.IsTogglerLit = true;
					}
				}
			}

			this.HasClicked = is_click;
		}
	}
}
