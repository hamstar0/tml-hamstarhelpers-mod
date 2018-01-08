using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	/*class AdminUserRemoveCommand : ModCommand {
		public override CommandType Type { get { return CommandType.World; } }
		public override string Command { get { return "hhadmindel"; } }
		public override string Usage { get { return "/hhadmindel fake_hamstar_omg"; } }
		public override string Description { get { return "Removes a player from the admin user list."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 1 ) {
				throw new UsageException( "Command not available for clients.", Color.Red );
			}

			if( Main.netMode != 0 && !UserHelpers.UserHelpers.IsAdmin(caller.Player) ) {
				throw new UsageException( "Only admins are allowed to use this command.", Color.Red );
			}

			string name = args[0];
			if( String.IsNullOrEmpty( name ) ) { throw new UsageException( "Missing player name.", Color.Red ); }

			for( int i=0; i<Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player != null && player.active && player.name != args[0] ) { continue; }

				if( UserHelpers.UserHelpers.RemoveAdmin( player ) ) {
					caller.Reply( name + " no longer an admin.", Color.White );
				} else {
					caller.Reply( name + " was not an admin.", Color.Yellow );
				}
				return;
			}

			caller.Reply( "No player by name "+name+" found.", Color.Red );
		}
	}*/
}
