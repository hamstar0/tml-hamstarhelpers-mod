using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Services.Network;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	class CursorPositionProtocol : PacketProtocolBroadcast {
		internal static bool BroadcastCursor() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModHelpersException( "Not a client." ); }

			short x = (short)Main.mouseX;
			short y = (short)Main.mouseY;

			if( Client.LastKnownCursorPositions.ContainsKey(Main.myPlayer) ) {
				(int X, int Y) lastPos = Client.LastKnownCursorPositions[Main.myPlayer];
				if( lastPos == (x, y) ) {
					return false;
				}
			}

			var protocol = new CursorPositionProtocol( (byte)Main.myPlayer, x, y );

			protocol.SendToServer( true );
			return true;
		}



		////////////////

		public byte PlayerWho;
		public short X;
		public short Y;



		////////////////

		private CursorPositionProtocol() { }

		private CursorPositionProtocol( byte playerWho, short x, short y ) {
			this.PlayerWho = playerWho;
			this.X = x;
			this.Y = y;
		}

		////////////////

		protected override void ReceiveOnClient() {
			Client._LastKnownCursorPositions[ this.PlayerWho ] = (this.X, this.Y);
		}

		protected override void ReceiveOnServer( int fromWho ) {
			Client._LastKnownCursorPositions[this.PlayerWho] = (this.X, this.Y);
		}
	}
}
