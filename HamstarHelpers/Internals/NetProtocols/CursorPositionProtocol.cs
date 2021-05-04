using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Services.Network;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	[Serializable]
	[IsNoisy]
	class CursorPositionProtocol : NetIOBroadcastPayload {
		internal static bool BroadcastCursorIf() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not a client." );
			}

			if( Client.LastKnownCursorPositions.ContainsKey( Main.myPlayer ) ) {
				(int X, int Y) lastPos = Client.LastKnownCursorPositions[ Main.myPlayer ];

				if( lastPos.X == Main.mouseX && lastPos.Y == Main.mouseY ) {
					return false;
				}
			}

			var protocol = new CursorPositionProtocol( (byte)Main.myPlayer, (short)Main.mouseX, (short)Main.mouseY );
			NetIO.Broadcast( protocol );

			return true;
		}



		////////////////

		public byte PlayerWho;
		public short X;
		public short Y;



		////////////////

		public CursorPositionProtocol() { }

		private CursorPositionProtocol( byte playerWho, short x, short y ) {
			this.PlayerWho = playerWho;
			this.X = x;
			this.Y = y;
		}

		////////////////

		public override bool ReceiveOnServerBeforeRebroadcast( int fromWho ) {
			Client._LastKnownCursorPositions[this.PlayerWho] = (this.X, this.Y);
			return true;
		}

		public override void ReceiveBroadcastOnClient() {
			Client._LastKnownCursorPositions[this.PlayerWho] = (this.X, this.Y);
		}
	}
}