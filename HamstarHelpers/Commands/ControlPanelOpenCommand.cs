﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	/// @private
	public class ControlPanelOpenCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.Chat;
		/// @private
		public override string Command => "mh-cp-open";
		/// @private
		public override string Usage => "/" +this.Command;
		/// @private
		public override string Description => "Opens the Mod Helpers mod Control Panel.";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == NetmodeID.Server ) {
				caller.Reply( "Command not available for server.", Color.Red );
				return;
			}

			if( ModHelpersConfig.Instance.DisableControlPanel ) {
				caller.Reply( "Control panel disabled.", Color.Red );
			} else {
				ModHelpersMod.Instance.ControlPanelUI.Open();
			}
		}
	}
}
