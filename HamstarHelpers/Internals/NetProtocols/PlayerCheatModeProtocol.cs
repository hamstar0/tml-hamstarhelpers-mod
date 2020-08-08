using System;
using Terraria;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Cheats;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	[Serializable]
	class PlayerCheatModeProtocol : NetProtocolBroadcastPayload {
		public static void BroadcastFromClient( CheatModeType cheatFlags ) {
			var protocol = new PlayerCheatModeProtocol( cheatFlags, Main.myPlayer );
			NetIO.Broadcast( protocol );
		}

		public static void BroadcastToClients( Player player, CheatModeType cheatFlags ) {
			var protocol = new PlayerCheatModeProtocol( cheatFlags, player.whoAmI );
			NetIO.SendToClients( protocol, player.whoAmI );
		}



		////////////////

		public CheatModeType CheatFlags;
		public int PlayerWho;



		////////////////

		private PlayerCheatModeProtocol() { }

		private PlayerCheatModeProtocol( CheatModeType cheatFlags, int playerWho ) {
			this.CheatFlags = cheatFlags;
			this.PlayerWho = playerWho;
		}


		////////////////

		public override bool ReceiveOnServerBeforeRebroadcast( int fromWho ) {
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.player[fromWho] );
			myplayer.Logic.SetCheats( this.CheatFlags );
			return true;
		}

		public override void ReceiveBroadcastOnClient() {
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.player[this.PlayerWho] );
			myplayer.Logic.SetCheats( this.CheatFlags );
		}
	}
}
