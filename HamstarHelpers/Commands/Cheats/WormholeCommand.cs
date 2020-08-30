using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.Cheats;


namespace HamstarHelpers.Commands.Cheats {
	/// @private
	public class WormholeCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.World;
		/// @private
		public override string Command => "mh-cheat-wormhole";
		/// @private
		public override string Usage => "/" + this.Command + " Timothy";
		/// @private
		public override string Description => "Teleports to another player."
			+"\n  Parameters: <player name>";


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
			/*if( Main.netMode != NetmodeID.SinglePlayer ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}*/

			int tileX, tileY;

			if( args.Length == 1 ) {
				Player targPlr = Main.player.FirstOrDefault( plr => plr?.active == true && plr.name.Equals( args[0] ) );
				if( targPlr == null ) {
					caller.Reply( "Invalid player "+args[0], Color.Yellow );
					return;
				}
				if( targPlr.dead ) {
					caller.Reply( "Cannot teleport to dead player " + args[0], Color.Yellow );
					return;
				}

				tileX = (int)(targPlr.position.X / 16f);
				tileY = (int)(targPlr.position.Y / 16f);
			} else {
				caller.Reply( "Invalid parameters.", Color.Yellow );
				return;
			}

			PlayerCheats.TeleportTo( caller.Player, tileX, tileY );
		}
	}
}
