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
				return CommandType.Console;
			}
		}
		public override string Command { get { return "hhmodlockworldtoggle"; } }
		public override string Usage { get { return "/hhmodlockworldtoggle"; } }
		public override string Description { get { return "Toggles locking mods for the current world."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			//if( Main.netMode != 0 && !UserHelpers.UserHelpers.IsAdmin(caller.Player) ) {
			//	throw new UsageException( "Only admins are allowed to use this command.", Color.Red );
			//}

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
