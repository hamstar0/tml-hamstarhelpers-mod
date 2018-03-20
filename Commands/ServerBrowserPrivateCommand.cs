using HamstarHelpers.Utilities.Timers;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	class ServerBrowserPrivateCommand : ModCommand {
		public override CommandType Type {
			get {
				return CommandType.Console | CommandType.Server;
			}
		}
		public override string Command { get { return "hhprivateserver"; } }
		public override string Usage { get { return "/hhprivateserver"; } }
		public override string Description { get { return "Sets current server to not be listed on the server browser."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (HamstarHelpersMod)this.mod;
			if( !mymod.Config.IsServerPromptingForBrowser && (caller.CommandType & CommandType.Console) == 0 ) {
				caller.Reply( "Cannot set server private; grace period has expired." );
				return;
			}

			mymod.ServerBrowser.StopUpdates();

			caller.Reply( "Server set to be private. To set all future servers private, set IsServerHiddenFromBrowser to false in the Hamstar's Helpers config settings.", Color.GreenYellow );
		}
	}
}
