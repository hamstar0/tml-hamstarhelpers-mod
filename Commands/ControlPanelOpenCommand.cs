﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class ControlPanelOpenCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "mh-cp-open"; } }
		public override string Usage { get { return "/"+this.Command; } }
		public override string Description { get { return "Opens the Mod Helpers mod Control Panel."; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 2 ) {
				throw new UsageException( "Command not available for server.", Color.Red );
			}

			var mymod = (ModHelpersMod)this.mod;

			if( mymod.Config.DisableControlPanel ) {
				caller.Reply( "Control panel disabled.", Color.Red );
			} else {
				ModHelpersMod.Instance.ControlPanel.Open();
			}
		}
	}
}
