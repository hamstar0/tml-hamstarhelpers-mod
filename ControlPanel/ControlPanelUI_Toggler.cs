using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI : UIState {
		private static Vector2 TogglerPosition = new Vector2( 128, 0 );
		private static Version AlertVersion = new Version( 1, 2, 2 );



		////////////////

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
			this.CheckTogglerMouseInteraction();

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

			this.DrawTogglerAlert( sb );
		}


		private int AnimateAlert = 0;

		private void DrawTogglerAlert( SpriteBatch sb ) {
			Player myplayer = Main.LocalPlayer;
			var modplayer = myplayer.GetModPlayer<HamstarHelpersPlayer>();
			Version ver = new Version( modplayer.ControlPanelNewSince );

			if( ver >= ControlPanelUI.AlertVersion ) { return; }

			Color color = Color.Blue;
			Vector2 pos = ControlPanelUI.TogglerPosition;
			pos.X += 56f - (Main.fontMouseText.MeasureString("New!").X * 0.5f);
			pos.Y -= 4f;

			if( this.AnimateAlert >= 16 ) { this.AnimateAlert = 0; }

			if( this.AnimateAlert < 8 ) {
				color = Color.Lerp( Color.Yellow, Color.Gray, (float)this.AnimateAlert / 8f );
			} else if( this.AnimateAlert < 16 ) {
				color = Color.Lerp( Color.Gray, Color.Yellow, (float)(this.AnimateAlert - 8) / 8f );
			}
			this.AnimateAlert++;

			sb.DrawString( Main.fontMouseText, "New!", pos, color );
		}


		////////////////
		
		private void CheckTogglerMouseInteraction() {
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

								var modplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
								modplayer.ControlPanelNewSince = ControlPanelUI.AlertVersion.ToString();
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
