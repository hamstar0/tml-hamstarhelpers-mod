using HamstarHelpers.Helpers.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	class MyUserIdCommand : ModCommand {
		public override CommandType Type {
			get {
				return CommandType.Chat;
			}
		}
		public override string Command => "mh-my-userid";
		public override string Usage => "/" + this.Command;
		public override string Description => "Displays your user id.";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			string uid = PlayerIdentityHelpers.GetMyProperUniqueId();
			
			caller.Reply( "Your user ID is: " + uid, Color.Lime );
		}
	}
}
