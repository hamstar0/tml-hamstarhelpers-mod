using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI : UIState {
		private static Vector2 TogglerPosition {
			get {
				var config = HamstarHelpersMod.Instance.Config;
				int x = config.ControlPanelIconX < 0 ? Main.screenWidth + config.ControlPanelIconX : config.ControlPanelIconX;
				int y = config.ControlPanelIconY < 0 ? Main.screenHeight + config.ControlPanelIconY : config.ControlPanelIconY;

				return new Vector2( x, y );
			}
		}
		private static Version AlertVersion = new Version( 1, 2, 6 );



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
				tex = ControlPanelUI.ControlPanelIconLit;
				color = new Color( 192, 192, 192, 192 );
			} else {
				tex = ControlPanelUI.ControlPanelIcon;
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
			pos.Y += 6f;
			//pos.X += 56f - (Main.fontMouseText.MeasureString("New!").X * 0.5f);
			//pos.Y -= 4f;

			if( this.AnimateAlert >= 16 ) { this.AnimateAlert = 0; }

			if( this.AnimateAlert < 8 ) {
				color = Color.Lerp( Color.Yellow, Color.Gray, (float)this.AnimateAlert / 8f );
			} else if( this.AnimateAlert < 16 ) {
				color = Color.Lerp( Color.Gray, Color.Yellow, (float)(this.AnimateAlert - 8) / 8f );
			}
			this.AnimateAlert++;

			//sb.DrawString( Main.fontMouseText, "New!", pos, color );
			sb.DrawString( Main.fontMouseText, "New!", pos+new Vector2(-0.35f,-0.35f), Color.Black, 0f, default( Vector2 ), 0.64f, SpriteEffects.None, 1f );
			sb.DrawString( Main.fontMouseText, "New!", pos, color, 0f, default(Vector2), 0.6f, SpriteEffects.None, 1f );
		}


		////////////////
		
		private void CheckTogglerMouseInteraction() {
			bool is_click = Main.mouseLeft && Main.mouseLeftRelease;
			Vector2 pos = ControlPanelUI.TogglerPosition;
			Vector2 size = ControlPanelUI.ControlPanelIcon.Size();

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
