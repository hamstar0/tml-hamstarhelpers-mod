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
			if( !mymod.Config.IsServerPromptingUsersBeforeListingOnBrowser && (caller.CommandType & CommandType.Console) == 0 ) {
				caller.Reply( "Cannot set server private; grace period has expired. Set \"IsServerHiddenFromBrowser: true\" in config file, then restart server." );
				return;
			}

			mymod.ServerBrowser.StopLoopingServerAnnounce();

			caller.Reply( "Server set private. For future servers, set \"IsServerHiddenFromBrowser: true\" in the Mod Helpers config settings.", Color.GreenYellow );
		}
	}
}
