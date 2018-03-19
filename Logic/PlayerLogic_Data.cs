using HamstarHelpers.Utilities.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Logic {
	class HHPlayerDataProtocol : PacketProtocol {
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
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceive( this.HasUID, this.PrivateUID, this.PermaBuffsById );

			this.HasUID = false;
			this.PrivateUID = "";
		}

		public override void ReceiveOnClient() {
			Player player = Main.player[ this.PlayerWho ];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceive( this.PermaBuffsById );
		}

		////////////////

		public override bool ReceiveRequestOnServer( int from_who ) {
			for( int i=0; i<Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }
				var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

				var plr_data = new HHPlayerDataProtocol( i, myplayer.Logic.PermaBuffsById );
				plr_data.SendData( from_who, -1, false );
			}
			
			return true;
		}
	}




	partial class PlayerLogic {
		public PlayerLogic() {
			this.PrivateUID = Guid.NewGuid().ToString( "D" );
			this.HasUID = false;
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
			this.PermaBuffsById = new HashSet<int>();
		}

		
		////////////////

		public void Load( TagCompound tags ) {
			if( tags.ContainsKey( "uid" ) ) {
				this.PrivateUID = tags.GetString( "uid" );
			}
			if( tags.ContainsKey( "cp_new_since" ) ) {
				this.ControlPanelNewSince = tags.GetString( "cp_new_since" );
			}
			if( tags.ContainsKey( "perma_buffs" ) ) {
				var perma_buffs = tags.GetList<int>( "perma_buffs" );
				this.PermaBuffsById = new HashSet<int>( perma_buffs.ToArray() );
			}

			this.HasUID = true;
		}

		public TagCompound Save() {
			var perma_buffs = this.PermaBuffsById.ToArray();

			TagCompound tags = new TagCompound {
				{ "uid", this.PrivateUID },
				{ "cp_new_since", this.ControlPanelNewSince },
				{ "perma_buffs", perma_buffs }
			};
			return tags;
		}


		////////////////
		
		public void NetReceive( ISet<int> perma_buff_ids ) {
			this.PermaBuffsById = perma_buff_ids;
		}

		public void NetReceive( bool has_uid, string uid, ISet<int> perma_buff_ids ) {
			this.HasUID = has_uid;
			this.PrivateUID = uid;
			this.PermaBuffsById = perma_buff_ids;
		}
	}
}
