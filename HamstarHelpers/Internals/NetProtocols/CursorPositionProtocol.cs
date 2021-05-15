using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Services.Network;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	[Serializable]
	[IsNoisy]
	class CursorPositionProtocol : SimplePacketPayload {
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
			SimplePacket.SendToClient( protocol );

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

		public override void ReceiveOnServer( int fromWho ) {
			this.Receive();

			SimplePacket.SendToClient( this, -1, fromWho );
		}

		public override void ReceiveOnClient() {
			this.Receive();
		}


		////////////////

		private void Receive() {
			Client._LastKnownCursorPositions[this.PlayerWho] = (this.X, this.Y);
		}
	}
}