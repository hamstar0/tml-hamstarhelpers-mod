using HamstarHelpers.Utilities.Network;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.NetProtocols {
	class HHPlayerDataProtocol : PacketProtocol {
		public static void SyncToOtherClients( int from_who, bool has_uid, string uid, ISet<int> perma_buffs_by_id ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not client" ); }

			var protocol = new HHPlayerDataProtocol( from_who, has_uid, uid, perma_buffs_by_id );
			protocol.SendToServer( true );
		}

		public static void SendToClient( int from_who, bool has_uid, string uid, ISet<int> perma_buffs_by_id ) {
			if( Main.netMode != 2 ) { throw new Exception("Not server"); }

			var protocol = new HHPlayerDataProtocol( from_who, has_uid, uid, perma_buffs_by_id );
			protocol.SendToClient( -1, -1 );
		}



		////////////////

		public int PlayerWho;
		public bool HasUID = false;
		public string PrivateUID = "";
		public ISet<int> PermaBuffsById = new HashSet<int>();


		////////////////

		public HHPlayerDataProtocol() {
			this.PlayerWho = 255;
		}

		internal HHPlayerDataProtocol( int player_who, ISet<int> perma_buff_ids ) {
			this.PlayerWho = player_who;
			this.HasUID = false;
			this.PrivateUID = "";
			this.PermaBuffsById = perma_buff_ids;
		}

		internal HHPlayerDataProtocol( int player_who, bool has_uid, string uid, ISet<int> perma_buff_ids ) {
			this.PlayerWho = player_who;
			this.HasUID = has_uid;
			this.PrivateUID = uid;
			this.PermaBuffsById = perma_buff_ids;
		}

		////////////////

		public override void SetServerDefaults() { }

		////////////////

		public override void ReceiveOnServer( int from_who ) {
			Player player = Main.player[from_who];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceive( this.HasUID, this.PrivateUID, this.PermaBuffsById );

			this.HasUID = false;
			this.PrivateUID = "";
		}

		public override void ReceiveOnClient() {
			Player player = Main.player[this.PlayerWho];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceive( this.PermaBuffsById );
		}

		////////////////

		public override bool ReceiveRequestOnServer( int from_who ) {
			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }
				var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();
				
				var plr_data = new HHPlayerDataProtocol( i, myplayer.Logic.PermaBuffsById );
				plr_data.SendToClient( from_who, -1 );
			}

			return true;
		}
	}
}
