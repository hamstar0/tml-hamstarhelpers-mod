using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class PlayerDataProtocol : SimplePacketPayload {
		public static void BroadcastToAll(
					HashSet<int> permaBuffsById,
					HashSet<int> hasBuffIds,
					Dictionary<int, int> equipSlotsToItemTypes ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModHelpersException( "Not client" ); }

			var protocol = new PlayerDataProtocol( permaBuffsById, hasBuffIds, equipSlotsToItemTypes );

			SimplePacket.SendToServer( protocol );
		}



		////////////////

		public int PlayerWho = 255;
		public HashSet<int> PermaBuffsById = new HashSet<int>();
		public HashSet<int> HasBuffIds = new HashSet<int>();
		public Dictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();



		////////////////

		public PlayerDataProtocol() { }

		private PlayerDataProtocol(
					HashSet<int> permaBuffsById,
					HashSet<int> hasBuffIds,
					Dictionary<int, int> equipSlotsToItemTypes ) {
			this.PlayerWho = Main.myPlayer;
			this.PermaBuffsById = permaBuffsById;
			this.HasBuffIds = hasBuffIds;
			this.EquipSlotsToItemTypes = equipSlotsToItemTypes;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			Player player = Main.player[fromWho];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			myplayer.Logic.NetReceiveDataOnServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );

			SimplePacket.SendToClient( this );
		}

		public override void ReceiveOnClient() {
			if( this.PlayerWho < 0 || this.PlayerWho >= Main.player.Length ) {
//throw new HamstarException( "ModHelpers.PlayerDataProtocol.ReceiveWithClient - Invalid player index " + this.PlayerWho );
				throw new ModHelpersException( "Invalid player index " + this.PlayerWho );
			}

			Player player = Main.player[ this.PlayerWho ];
			if( player == null || !player.active ) {
//LogHelpers.Log( "ModHelpers.PlayerDataProtocol.ReceiveWithClient - Inactive player indexed as " + this.PlayerWho );
				LogHelpers.Log( DebugHelpers.GetCurrentContext() + " - Inactive player indexed as " + this.PlayerWho );
				return;
			}

			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			myplayer.Logic.NetReceiveDataOnClient( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}
	}
}