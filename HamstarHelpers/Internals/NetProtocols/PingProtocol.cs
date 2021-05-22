using System;
using HamstarHelpers.Libraries.DotNET;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class PingProtocol : SimplePacketPayload {
		public static void QuickSendToServer() {
			SimplePacket.SendToServer( new PingProtocol() );
		}



		////////////////

		public long StartTime = -1;
		public long ServerBounceTime = -1;
		public long ClientRoundTripTime = -1;



		////////////////

		public PingProtocol() {
			this.StartTime = (long)SystemLibraries.TimeStamp().TotalMilliseconds;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			this.ServerBounceTime = (long)SystemLibraries.TimeStamp().TotalMilliseconds;

			if( this.ClientRoundTripTime == -1 ) {
				SimplePacket.SendToClient( this, fromWho, -1 );
			} else {
				int upSpan = (int)( this.ServerBounceTime - this.StartTime );
				int downSpan = (int)( this.ClientRoundTripTime - this.ServerBounceTime );

				ModHelpersMod.Instance.NetHelpers.UpdatePing( upSpan, downSpan );
			}
		}


		public override void ReceiveOnClient() {
			this.ClientRoundTripTime = (long)SystemLibraries.TimeStamp().TotalMilliseconds;

			SimplePacket.SendToServer( this );

			if( this.ServerBounceTime != -1 ) {
				int upSpan = (int)( this.ServerBounceTime - this.StartTime );
				int downSpan = (int)( this.ClientRoundTripTime - this.ServerBounceTime );

				ModHelpersMod.Instance.NetHelpers.UpdatePing( upSpan, downSpan );
			}
		}
	}
}