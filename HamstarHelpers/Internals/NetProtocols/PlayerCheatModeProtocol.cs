using System;
using Terraria;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Cheats;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class PlayerCheatModeProtocol : SimplePacketPayload {
		public static void BroadcastFromClient( CheatModeType cheatFlags ) {
			var protocol = new PlayerCheatModeProtocol( cheatFlags, Main.myPlayer );
			SimplePacket.SendToServer( protocol );
		}

		public static void BroadcastToClients( Player player, CheatModeType cheatFlags ) {
			var protocol = new PlayerCheatModeProtocol( cheatFlags, player.whoAmI );
			SimplePacket.SendToClient( protocol );
		}



		////////////////

		public int CheatFlags;
		public int PlayerWho;



		////////////////

		public PlayerCheatModeProtocol() { }

		private PlayerCheatModeProtocol( CheatModeType cheatFlags, int playerWho ) {
			this.CheatFlags = (int)cheatFlags;
			this.PlayerWho = playerWho;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.player[fromWho] );

			myplayer.Logic.SetCheats( (CheatModeType)this.CheatFlags );

			SimplePacket.SendToClient( this );
		}

		public override void ReceiveOnClient() {
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.player[this.PlayerWho] );

			myplayer.Logic.SetCheats( (CheatModeType)this.CheatFlags );
		}
	}
}