using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Protocol.Packet.Interfaces;
using HamstarHelpers.Helpers.DotNET;


namespace HamstarHelpers.Internals.NetProtocols {
	/** @private */
	class PingProtocol : PacketProtocolSentToEither {
		public long StartTime = -1;
		public long EndTime = -1;

		

		////////////////

		[ProtocolIgnore]
		public override bool IsVerbose => false;



		////////////////

		private PingProtocol() { }


		////////////////

		protected override void SetClientDefaults() {
			this.StartTime = (long)SystemHelpers.TimeStamp().TotalMilliseconds;
		}

		protected override void SetServerDefaults( int fromWho ) { }


		////////////////

		protected override void ReceiveOnServer( int fromWho ) {
			if( this.EndTime == -1 ) {
				this.SendToClient( fromWho, -1 );
			} else {
				ModHelpersMod.Instance.ServerInfo.UpdatePingAverage( (int)( this.EndTime - this.StartTime ) );
			}
		}


		protected override void ReceiveOnClient() {
			var now = (long)SystemHelpers.TimeStamp().TotalMilliseconds;

			this.EndTime = now;

			this.SendToServer( false );

			if( this.StartTime != -1 ) {
				ModHelpersMod.Instance.NetHelpers.UpdatePing( (int)( now - this.StartTime ) );
			}
		}
	}
}
