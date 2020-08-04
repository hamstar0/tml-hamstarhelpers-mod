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
			var protocol = new PlayerCheatModeProtocol( cheatFlags );
			NetIO.Broadcast( protocol );
		}

		public static void SendToClients( Player player, CheatModeType cheatFlags ) {
			var protocol = new PlayerCheatModeProtocol( cheatFlags );
			NetIO.SendToClients( protocol, player.whoAmI );
		}



		////////////////

		public CheatModeType CheatFlags;



		////////////////

		private PlayerCheatModeProtocol() { }

		private PlayerCheatModeProtocol( CheatModeType cheatFlags ) {
			this.CheatFlags = cheatFlags;
		}


		////////////////

		public override void ReceiveOnServerBeforeRebroadcast( int fromWho ) {
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.player[fromWho] );
			myplayer.Logic.SetCheats( this.CheatFlags );
		}

		public override void ReceiveBroadcastOnClient() {
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.player[fromWho] );
			myplayer.Logic.SetCheats( this.CheatFlags );
		}
	}
}
