using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using HamstarHelpers.Services.AnimatedColor;


namespace HamstarHelpers.Internals.ControlPanel {
	/// @private
	partial class UIControlPanel : UIState {
		private static Version AlertSinceVersion = new Version( 2, 0, 0 );
		


		////////////////
		
		


		////////////////

		public bool IsTogglerUpdateAlertShown( out string tabName ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod.Data == null ) {
				LogHelpers.WarnOnce( "No mod data." );
				tabName = null;
				return false;
			}

			var ver = new Version( mymod.Data.ControlPanelNewSince );

			if( ver < UIControlPanel.AlertSinceVersion ) {
				tabName = UIControlPanel.DefaultTabName;
				return true;
			}

			UIModControlPanelTab uiModCtrlPanel = mymod.ControlPanelUI.DefaultTab;
			if( uiModCtrlPanel.GetModUpdatesAvailable() > 0 ) {
				tabName = UIControlPanel.DefaultTabName;
				return true;
			}

			//ControlPanelTabs.

			tabName = null;
			return false;
		}


		////////////////

		private void DrawTogglerAlert( SpriteBatch sb ) {
			Color color = AnimatedColors.Alert?.CurrentColor ?? Color.White;
			Vector2 pos = UIControlPanel.TogglerPosition;
			pos.Y += 6f;
			//pos.X += 56f - (Main.fontMouseText.MeasureString("New!").X * 0.5f);
			//pos.Y -= 4f;

			//sb.DrawString( Main.fontMouseText, "New!", pos, color );
			sb.DrawString(
				spriteFont: Main.fontMouseText,
				text: "New!",
				position: pos+new Vector2(-0.35f,-0.35f),
				color: Color.Black,
				rotation: 0f,
				origin: default( Vector2 ),
				scale: 0.64f,
				effects: SpriteEffects.None,
				layerDepth: 1f
			);
			sb.DrawString(
				spriteFont: Main.fontMouseText,
				text: "New!",
				position: pos,
				color: color,
				rotation: 0f,
				origin: default( Vector2),
				scale: 0.6f,
				effects: SpriteEffects.None,
				layerDepth: 1f
			);
		}
	}
}
