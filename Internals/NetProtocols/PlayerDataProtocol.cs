using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Protocol.Packet.Interfaces;


namespace HamstarHelpers.Internals.NetProtocols {
	/** @private */
	class PlayerDataProtocol : PacketProtocolSentToEither {
		public static void SyncToEveryone( ISet<int> permaBuffsById, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
			if( Main.netMode != 1 ) { throw new HamstarException( "Not client" ); }
			
			var protocol = new PlayerDataProtocol( permaBuffsById, hasBuffIds, equipSlotsToItemTypes );
			
			protocol.SendToServer( true );
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

		////

		protected override void SetServerDefaults( int fromWho ) { }


		////////////////

		protected override void ReceiveOnServer( int fromWho ) {
			Player player = Main.player[ fromWho ];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			
			myplayer.Logic.NetReceiveDataOnServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}

		protected override void ReceiveOnClient() {
			if( this.PlayerWho < 0 || this.PlayerWho >= Main.player.Length ) {
				//throw new HamstarException( "ModHelpers.PlayerDataProtocol.ReceiveWithClient - Invalid player index " + this.PlayerWho );
				throw new HamstarException( "Invalid player index " + this.PlayerWho );
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
