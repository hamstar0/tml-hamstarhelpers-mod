using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Network;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.NetProtocols {
	class HHPlayerDataProtocol : PacketProtocol {
		public static void SyncToOtherClients( int from_who, bool has_uid, string uid, ISet<int> perma_buffs_by_id,
				ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new HHPlayerDataProtocol( from_who, has_uid, uid, perma_buffs_by_id, has_buff_ids, equip_slots_to_item_types );
			protocol.SendToServer( true );
		}

		public static void SendStateToClient( int from_who, bool has_uid, string uid,
				ISet<int> perma_buffs_by_id, ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			var protocol = new HHPlayerDataProtocol( from_who, has_uid, uid, perma_buffs_by_id, has_buff_ids, equip_slots_to_item_types );
			protocol.SendToClient( -1, -1 );
		}

		public static void SendStateToServer( ISet<int> perma_buffs_by_id, ISet<int> has_buff_ids,
				IDictionary<int, int> equip_slots_to_item_types ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new HHPlayerDataProtocol( Main.myPlayer, perma_buffs_by_id, has_buff_ids, equip_slots_to_item_types );
			protocol.SendToServer( false );
		}



		////////////////

		public int PlayerWho = 255;
		public bool HasUID = false;
		public string PrivateUID = "";
		public ISet<int> PermaBuffsById = new HashSet<int>();
		public ISet<int> HasBuffIds = new HashSet<int>();
		public IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();


		////////////////

		public HHPlayerDataProtocol() { }

		private HHPlayerDataProtocol( int player_who, ISet<int> perma_buff_ids,
				ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			this.PlayerWho = player_who;
			this.HasUID = false;
			this.PrivateUID = "";
			this.PermaBuffsById = perma_buff_ids;
			this.HasBuffIds = has_buff_ids;
			this.EquipSlotsToItemTypes = equip_slots_to_item_types;
		}

		private HHPlayerDataProtocol( int player_who, bool has_uid, string uid, ISet<int> perma_buff_ids,
				ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			this.PlayerWho = player_who;
			this.HasUID = has_uid;
			this.PrivateUID = uid;
			this.PermaBuffsById = perma_buff_ids;
			this.HasBuffIds = has_buff_ids;
			this.EquipSlotsToItemTypes = equip_slots_to_item_types;
		}

		////////////////

		public override void SetServerDefaults() { }

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			Player player = Main.player[from_who];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceiveServer( this.HasUID, this.PrivateUID, this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );

			// Empty this data so we don't retransmit it to clients
			this.HasUID = false;
			this.PrivateUID = "";
		}

		protected override void ReceiveWithClient() {
			Player player = Main.player[this.PlayerWho];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceiveClient( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}

		////////////////

		/*protected override bool ReceiveRequestWithServer( int from_who ) {
			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }
				var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();
				
				var protocol = new HHPlayerDataProtocol( i, myplayer.Logic.PermaBuffsById );
				protocol.SendToClient( from_who, -1 );
			}

			return true;
		}*/
	}
}
