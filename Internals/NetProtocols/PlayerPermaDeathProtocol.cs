﻿using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;
using HamstarHelpers.Services.Network.NetIO;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	[Serializable]
	class PlayerPermaDeathProtocol : NetProtocolBroadcastPayload {
		public static void BroadcastFromClient( int playerDeadWho, string msg ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			var protocol = new PlayerPermaDeathProtocol( playerDeadWho, msg );
			NetIO.Broadcast( protocol );
		}

		public static void BroadcastFromServer( int playerDeadWho, string msg ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			var protocol = new PlayerPermaDeathProtocol( playerDeadWho, msg );
			NetIO.SendToClients( protocol );
		}



		////////////////

		public int PlayerWho;
		public string Msg;



		////////////////

		private PlayerPermaDeathProtocol() { }

		protected PlayerPermaDeathProtocol( int playerWho, string msg ) {
			this.PlayerWho = playerWho;
			this.Msg = msg;
		}


		////////////////

		public override void ReceiveOnServerBeforeRebroadcast( int fromWho ) {
			//Player player = Main.player[ this.PlayerWho ];

			//PlayerHelpers.ApplyPermaDeath( player, this.Msg );	?
		}

		public override void ReceiveBroadcastOnClient( int fromWho ) {
			Player player = Main.player[ this.PlayerWho ];
			if( player == null || !player.active ) {
				LogHelpers.Alert( "Inactive player indexed as " + this.PlayerWho );
				return;
			}

			PlayerHelpers.ApplyPermaDeath( player, this.Msg );
		}
	}
}
