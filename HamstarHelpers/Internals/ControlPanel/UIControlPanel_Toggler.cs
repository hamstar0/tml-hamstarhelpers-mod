using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.Messages.Inbox;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.ControlPanel;


namespace HamstarHelpers.Internals.ControlPanel {
	/// @private
	partial class UIControlPanel : UIState {
		private static Vector2 TogglerPosition {
			get {
				var config = ModHelpersConfig.Instance;
				int x = config.ControlPanelIconX < 0
					? Main.screenWidth + config.ControlPanelIconX
					: config.ControlPanelIconX;
				int y = config.ControlPanelIconY < 0
					? Main.screenHeight + config.ControlPanelIconY
					: config.ControlPanelIconY;

				if( x == 0 && y == 0 ) {
					if( Main.LocalPlayer.InfoAccMechShowWires && Main.LocalPlayer.rulerLine ) {
						y += 16;
					}
				}

				return new Vector2( x, y );
			}
		}



		////////////////

		public bool IsTogglerLit { get; private set; }



		////////////////

		private void InitializeToggler() {
			this.IsTogglerLit = false;

			LoadHooks.AddWorldLoadEachHook( () => {
				var uiModCtrlPanel = (UIModControlPanelTab)ModHelpersMod.Instance.ControlPanelUI.DefaultTab;
				int modUpdateCount = uiModCtrlPanel.GetModUpdatesAvailable();

				if( modUpdateCount > 0 ) {
					InboxMessages.SetMessage( "mod_updates", modUpdateCount + " mod updates available. See mod browser.", true );
				}
			} );
		}


		////////////////

		public bool IsTogglerShown() {
			return Main.playerInventory;
		}


		////////////////

		public void UpdateToggler() {
			this.RunTogglerMouseInteraction();

			if( this.IsTogglerLit ) {
				Main.LocalPlayer.mouseInterface = true;
			}
		}


		////////////////

		public void DrawToggler( SpriteBatch sb ) {
			if( !this.IsTogglerShown() ) { return; }

			bool alertShown = this.IsTogglerUpdateAlertShown( out string _ );
			Texture2D tex;
			Color color;

			if( this.IsTogglerLit ) {
				tex = UIControlPanel.ControlPanelIconLit;
				color = new Color( 192, 192, 192, 192 );
			} else {
				tex = UIControlPanel.ControlPanelIcon;
				color = new Color( 160, 160, 160, 160 );
			}

			sb.Draw( tex, UIControlPanel.TogglerPosition, null, color );

			if( alertShown ) {
				this.DrawTogglerAlert( sb );
			}

			if( this.IsTogglerLit ) {
				if( alertShown ) {
					sb.DrawString(
						spriteFont: Main.fontMouseText,
						text: "New mod updates!",
						position: new Vector2( Main.mouseX + 8, Main.mouseY + 8 ),
						color: AnimatedColors.Alert.CurrentColor
					);
				} else {
					sb.DrawString(
						spriteFont: Main.fontMouseText,
						text: "Mod Control Panel",
						position: new Vector2( Main.mouseX + 8, Main.mouseY + 8 ),
						color: Color.White
					);
				}
			}
		}


		////////////////

		private void RunTogglerMouseInteraction() {
			bool isClick = Main.mouseLeft && Main.mouseLeftRelease && !this.HasClicked;
			Vector2 pos = UIControlPanel.TogglerPosition;
			Vector2 size = UIControlPanel.ControlPanelIcon.Size();

			this.IsTogglerLit = false;

			if( this.IsTogglerShown() ) {
				bool isMouseOver = Main.mouseX >= pos.X && Main.mouseX < ( pos.X + size.X )
								&& Main.mouseY >= pos.Y && Main.mouseY < ( pos.Y + size.Y );

				if( isMouseOver ) {
					if( isClick ) {
						if( this.IsOpen ) {
							this.Close();
						} else if( this.CanOpen() ) {
							this.OpenViaToggler();
						}
					}

					this.IsTogglerLit = true;
				}
			}

			this.HasClicked = isClick;
		}


		////

		private void OpenViaToggler() {
			if( !this.IsTogglerUpdateAlertShown( out string tabName ) ) {
				tabName = UIControlPanel.DefaultTabName;
			} else {
				this.AlertTabs.Remove( tabName );
			}

			//this.Open();
			ControlPanelTabs.OpenTab( tabName );

			var mymod = ModHelpersMod.Instance;
			Version oldVers;
			Version newVers = UIControlPanel.AlertSinceVersion;

			if( Version.TryParse( mymod.Data.ControlPanelNewSince, out oldVers ) && oldVers != newVers ) {
				mymod.Data.ControlPanelNewSince = newVers.ToString();
				mymod.SaveModData();
			}
		}
	}
}
