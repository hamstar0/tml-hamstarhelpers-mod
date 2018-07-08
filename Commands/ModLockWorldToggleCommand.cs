using HamstarHelpers.TmlHelpers.ModHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	class ModLockWorldToggleCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.Server;
			}
		}
		public override string Command { get { return "mhmodlockworldtoggle"; } }
		public override string Usage { get { return "/" + this.Command; } }
		public override string Description { get { return "Toggles locking mods for the current world."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 2 && caller.CommandType != CommandType.Console ) {
				bool success;
				bool has_priv = UserHelpers.UserHelpers.HasBasicServerPrivilege( caller.Player, out success );

				if( !success ) {
					caller.Reply( "Could not validate.", Color.Yellow );
					return;
				} else if( !has_priv ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}

			if( ModLockHelpers.IsWorldLocked() ) {
				caller.Reply( "World unlocked.", Color.GreenYellow );
				ModLockHelpers.UnlockWorld();
			} else {
				caller.Reply( "World locked.", Color.GreenYellow );
				ModLockHelpers.LockWorld();
			}
		}
	}
}
