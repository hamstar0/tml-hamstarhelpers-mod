using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Components.Errors;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerDataProtocol : PacketProtocol {
		public static void SyncToEveryone( ISet<int> perma_buffs_by_id, ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new PlayerDataProtocol( Main.myPlayer, perma_buffs_by_id, has_buff_ids, equip_slots_to_item_types );
			protocol.SendToServer( true );
		}



		////////////////

		public int PlayerWho = 255;
		public ISet<int> PermaBuffsById = new HashSet<int>();
		public ISet<int> HasBuffIds = new HashSet<int>();
		public IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();


		////////////////

		public PlayerDataProtocol() { }

		private PlayerDataProtocol( int player_who, ISet<int> perma_buff_ids, ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			this.PlayerWho = player_who;
			this.PermaBuffsById = perma_buff_ids;
			this.HasBuffIds = has_buff_ids;
			this.EquipSlotsToItemTypes = equip_slots_to_item_types;
		}

		////////////////
		
		public override void SetServerDefaults() { }

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			
			myplayer.Logic.NetReceiveDataServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}

		protected override void ReceiveWithClient() {
			if( this.PlayerWho < 0 || this.PlayerWho >= Main.player.Length ) {
				throw new HamstarException( "PlayerDataProtocol.ReceiveWithClient - Invalid player index "+this.PlayerWho );
			}

			Player player = Main.player[ this.PlayerWho ];
			if( player == null || !player.active ) {
				LogHelpers.Log( "PlayerDataProtocol.ReceiveWithClient - Inactive player indexed as " + this.PlayerWho );
				return;
			}

			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceiveDataClient( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}
	}
}
