using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	class PlayerDataProtocol : NetProtocolBroadcastPayload {
		public static void BroadcastToAll( ISet<int> permaBuffsById, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModHelpersException( "Not client" ); }
			
			var protocol = new PlayerDataProtocol( permaBuffsById, hasBuffIds, equipSlotsToItemTypes );

			NetIO.Broadcast( protocol );
		}



		////////////////

		public int PlayerWho = 255;
		public ISet<int> PermaBuffsById = new HashSet<int>();
		public ISet<int> HasBuffIds = new HashSet<int>();
		public IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();



		////////////////

		private PlayerDataProtocol() { }

		private PlayerDataProtocol( ISet<int> permaBuffsById, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
			this.PlayerWho = Main.myPlayer;
			this.PermaBuffsById = permaBuffsById;
			this.HasBuffIds = hasBuffIds;
			this.EquipSlotsToItemTypes = equipSlotsToItemTypes;
		}


		////////////////

		public override void ReceiveOnServerBeforeRebroadcast( int fromWho ) {
			Player player = Main.player[ fromWho ];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			
			myplayer.Logic.NetReceiveDataOnServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}

		public override void ReceiveBroadcastOnClient( int fromWho ) {
			if( this.PlayerWho < 0 || this.PlayerWho >= Main.player.Length ) {
				//throw new HamstarException( "ModHelpers.PlayerDataProtocol.ReceiveWithClient - Invalid player index " + this.PlayerWho );
				throw new ModHelpersException( "Invalid player index " + this.PlayerWho );
			}

			Player player = Main.player[ this.PlayerWho ];
			if( player == null || !player.active ) {
				//LogHelpers.Log( "ModHelpers.PlayerDataProtocol.ReceiveWithClient - Inactive player indexed as " + this.PlayerWho );
				LogHelpers.Log( DebugHelpers.GetCurrentContext()+" - Inactive player indexed as " + this.PlayerWho );
				return;
			}

			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			myplayer.Logic.NetReceiveDataOnClient( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}
	}
}
