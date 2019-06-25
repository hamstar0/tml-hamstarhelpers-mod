using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class ControlPanelOpenCommand : ModCommand {
		public override CommandType Type => CommandType.Chat;
		public override string Command => "mh-cp-open";
		public override string Usage => "/" +this.Command;
		public override string Description => "Opens the Mod Helpers mod Control Panel.";


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 2 ) {
				throw new UsageException( "Command not available for server.", Color.Red );
			}

			var mymod = (ModHelpersMod)this.mod;

			if( ModLoader.version >= new Version(0, 11) || mymod.Config.DisableControlPanel ) {
				caller.Reply( "Control panel disabled.", Color.Red );
			} else {
				ModHelpersMod.Instance.ControlPanel.Open();
			}
		}
	}
}
