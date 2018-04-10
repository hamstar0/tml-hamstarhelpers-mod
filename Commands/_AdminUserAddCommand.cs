using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	/*class AdminUserAddCommand : ModCommand {
		public static void AddAdmin( string name ) {
			if( String.IsNullOrEmpty( name ) ) { throw new UsageException( "Missing player name.", Color.Red ); }

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player != null && player.active && player.name != name ) { continue; }

				UserHelpers.UserHelpers.AddAdmin( player );

				caller.Reply( "Admin " + name + " added.", Color.GreenYellow );
				return;
			}
		}


		////////////////

		public override CommandType Type { get { return CommandType.World; } }
		public override string Command { get { return "hhadminadd"; } }
		public override string Usage { get { return "/hhadminadd hamstar"; } }
		public override string Description { get { return "Adds a player to the admin user list."; } }


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

				UserHelpers.UserHelpers.AddAdmin( player );

				caller.Reply( "Admin " + name + " added.", Color.GreenYellow );
				return;
			}

			caller.Reply( "No player by name "+name+".", Color.Red );
		}
	}


	class ConsoleAdminUserAddCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Console; } }
		public override string Command { get { return "hhconadminadd"; } }
		public override string Usage { get { return "/hhconadminadd hamstar"; } }
		public override string Description { get { return "Adds a player to the admin user list."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			string name = args[0];
			if( String.IsNullOrEmpty( name ) ) { throw new UsageException( "Missing player name.", Color.Red ); }

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player != null && player.active && player.name != args[0] ) { continue; }

				UserHelpers.UserHelpers.AddAdmin( player );

				caller.Reply( "Admin " + name + " added.", Color.GreenYellow );
				return;
			}

			caller.Reply( "No player by name " + name + ".", Color.Red );
		}
	}*/
}
