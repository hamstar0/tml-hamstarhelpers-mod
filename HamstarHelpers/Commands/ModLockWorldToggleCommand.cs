using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.User;
using HamstarHelpers.Services.ModHelpers;


namespace HamstarHelpers.Commands {
	/// @private
	public class ModLockWorldToggleCommand : ModCommand {
		/// @private
		public override CommandType Type {
			get {
				if( Main.netMode == NetmodeID.SinglePlayer && !Main.dedServ ) {
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
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				LogLibraries.Log( "ModLockWorldToggleCommand - Not supposed to run on client." );
				return;
			}

			if( Main.netMode == NetmodeID.Server && caller.CommandType != CommandType.Console ) {
				if( !UserLibraries.HasBasicServerPrivilege( caller.Player ) ) {
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
