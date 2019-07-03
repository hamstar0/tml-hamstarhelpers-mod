using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.ModHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	/// @private
	public class ModLockWorldToggleCommand : ModCommand {
		/// @private
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		/// @private
		public override string Command => "mh-mod-lock-world-toggle";
		/// @private
		public override string Usage => "/" + this.Command;
		/// @private
		public override string Description => "Toggles locking mods for the current world.";


		////////////////

		/// @private
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

			if( ModLockService.IsWorldLocked() ) {
				caller.Reply( "World unlocked.", Color.Lime );
				ModLockService.UnlockWorld();
			} else {
				caller.Reply( "World locked.", Color.Lime );
				ModLockService.LockWorld();
			}
		}
	}
}
