using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.User;
using HamstarHelpers.Services.Cheats;


namespace HamstarHelpers.Commands.Cheats {
	/// @private
	public class CheatToggleCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.World;
		/// @private
		public override string Command => "mh-cheat-toggle";
		/// @private
		public override string Usage => "/" + this.Command + " god fly";
		/// @private
		public override string Description => "Toggles cheat modes."
			+"\n  Parameters: [<mode 1> <mode 2> ...]"
			+"\n  Modes:"
			+"\n    bilbo: Invisible."
			+"\n    god: Invincibility."
			+"\n    mdk: Maximum damage weapons."
			+"\n    fly: Flight accessories unlimited.";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				LogLibraries.Alert( "Not supposed to run on client." );
				return;
			}

			if( !ModHelpersConfig.Instance.DebugModeCheats ) {
				caller.Reply( "Cheats mode not enabled.", Color.Red );
				return;
			}
			/*if( Main.netMode != NetmodeID.SinglePlayer ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}*/

			if( !PlayerCheats.TryParseCheatFlags(args, out CheatModeType cheatFlags) ) {
				caller.Reply( "Invalid cheat identifiers.", Color.Red );
				return;
			}

			PlayerCheats.ToggleCheats( caller.Player, cheatFlags );

			string cheats = string.Join( ", ", PlayerCheats.OutputActiveCheats(caller.Player) );
			caller.Reply( "Cheats active: "+cheats, Color.Lime );
		}
	}
}
