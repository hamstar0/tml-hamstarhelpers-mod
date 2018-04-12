using HamstarHelpers.Utilities.AnimatedColor;
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
		private static Version AlertVersion = new Version( 1, 3, 0 );



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

		public bool IsTogglerUpdateAlertShown() {
			Player myplayer = Main.LocalPlayer;
			var modplayer = myplayer.GetModPlayer<HamstarHelpersPlayer>();
			var ver = new Version( modplayer.Logic.ControlPanelNewSince );

			if( ver < ControlPanelUI.AlertVersion ) {
				return true;
			}

			foreach( var moditem in this.ModDataList ) {
				if( moditem.LatestAvailableVersion > moditem.Mod.Version ) {
					return true;
				}
			}

			return false;
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

			bool alert_shown = this.IsTogglerUpdateAlertShown();
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

			if( alert_shown ) {
				this.DrawTogglerAlert( sb );
			}

			if( this.IsTogglerLit ) {
				if( alert_shown ) {
					sb.DrawString( Main.fontMouseText, "New mod updates!", new Vector2( Main.mouseX + 8, Main.mouseY + 8 ), AnimatedColors.Alert.CurrentColor );
				} else {
					sb.DrawString( Main.fontMouseText, "Mod Control Panel", new Vector2( Main.mouseX + 8, Main.mouseY + 8 ), Color.White );
				}
			}
		}

		
		private void DrawTogglerAlert( SpriteBatch sb ) {
			Color color = AnimatedColors.Alert != null ? AnimatedColors.Alert.CurrentColor : Color.White;
			Vector2 pos = ControlPanelUI.TogglerPosition;
			pos.Y += 6f;
			//pos.X += 56f - (Main.fontMouseText.MeasureString("New!").X * 0.5f);
			//pos.Y -= 4f;

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
								modplayer.Logic.ControlPanelNewSince = ControlPanelUI.AlertVersion.ToString();
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
