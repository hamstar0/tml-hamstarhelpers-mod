using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Logic {
	public class PlayerDataProtocol : AbstractPlayerDataProtocol {
		public override string GetName() { return "PlayerData"; }

		////////////////
		
		public bool HasUID;
		public string PrivateUID;
		public ISet<int> PermaBuffsById;

		////////////////

		public PlayerDataProtocol() : base( 0 ) { }

		internal PlayerDataProtocol( int player_who, ISet<int> perma_buff_ids ) : base( player_who ) {
			this.HasUID = false;
			this.PrivateUID = "";
			this.PermaBuffsById = perma_buff_ids;
		}

		internal PlayerDataProtocol( int player_who, bool has_uid, string uid, ISet<int> perma_buff_ids ) : base( player_who ) {
			this.HasUID = has_uid;
			this.PrivateUID = uid;
			this.PermaBuffsById = perma_buff_ids;
		}

		////////////////

		public override void ReceiveOnServer( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceive( this.HasUID, this.PrivateUID, this.PermaBuffsById );
		}

		public override void ReceiveOnClient() {
			Player player = Main.player[ this.PlayerWho ];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			myplayer.Logic.NetReceive( this.PermaBuffsById );
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

		public void NetSend( HamstarHelpersMod mymod, Player me, int to_who, int ignore_who, bool include_uid ) {
			PlayerDataProtocol protocol;

			if( include_uid ) {
				protocol = new PlayerDataProtocol( me.whoAmI, this.HasUID, this.PrivateUID, this.PermaBuffsById );
			} else {
				protocol = new PlayerDataProtocol( me.whoAmI, this.PermaBuffsById );
			}

			protocol.SendData( to_who, ignore_who );
		}

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
