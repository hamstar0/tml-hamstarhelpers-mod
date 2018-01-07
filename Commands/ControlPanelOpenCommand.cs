using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class ControlPanelOpenCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "hhcpopen"; } }
		public override string Usage { get { return "/hhcpopen"; } }
		public override string Description { get { return "Opens the Hamstar's Helpers mod Control Panel."; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 2 ) {
				throw new UsageException( "Command not available for server.", Color.Red );
			}

			if( ((HamstarHelpersMod)this.mod).Config.DisableControlPanel ) {
				caller.Reply( "Control panel disabled.", Color.Yellow );
			} else {
				HamstarHelpersMod.Instance.ControlPanel.Open();
			}
		}
	}
}
