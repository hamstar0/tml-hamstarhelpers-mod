using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.Cheats;


namespace HamstarHelpers.Commands.Cheats {
	/// @private
	public class TeleportCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.World;
		/// @private
		public override string Command => "mh-cheat-teleport";
		/// @private
		public override string Usage => "/" + this.Command + " 1234 567";
		/// @private
		public override string Description => "Teleports to the given tile coordinates. Negative values loop from opposite respective side."
			+"\n  Parameters: <tile x> <tile y>";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				LogHelpers.Alert( "Not supposed to run on client." );
				return;
			}

			if( !ModHelpersConfig.Instance.DebugModeCheats ) {
				caller.Reply( "Cheats mode not enabled.", Color.Red );
				return;
			}
			if( Main.netMode != NetmodeID.SinglePlayer ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}

			int tileX, tileY;

			if( args.Length == 2 ) {
				if( !Int32.TryParse(args[0], out tileX) || tileX < 0 || tileX >= Main.maxTilesX ) {
					caller.Reply( "Invalid tileX parameter.", Color.Yellow );
					return;
				}
				if( !Int32.TryParse(args[1], out tileY) || tileY < 0 || tileY >= Main.maxTilesY ) {
					caller.Reply( "Invalid tileY parameter.", Color.Yellow );
					return;
				}
			} else {
				caller.Reply( "Invalid parameters.", Color.Yellow );
				return;
			}

			PlayerCheats.TeleportTo( caller.Player, tileX, tileY );
		}
	}
}
