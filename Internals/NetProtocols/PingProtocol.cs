using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Components.Network;


namespace HamstarHelpers.Internals.NetProtocols {
	class PingProtocol : PacketProtocolSentToEither {
		public long StartTime = -1;
		public long EndTime = -1;

		
		////////////////

		[PacketProtocolIgnore]
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
