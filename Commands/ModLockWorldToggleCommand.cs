using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Helpers.UserHelpers;
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
				return CommandType.Console | CommandType.World;
			}
		}
		public override string Command { get { return "mh-mod-lock-world-toggle"; } }
		public override string Usage { get { return "/" + this.Command; } }
		public override string Description { get { return "Toggles locking mods for the current world."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 1 ) {
				LogHelpers.Log( "ModLockWorldToggleCommand - Not supposed to run on client." );
				return;
			}

			if( Main.netMode == 2 && caller.CommandType != CommandType.Console ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
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
