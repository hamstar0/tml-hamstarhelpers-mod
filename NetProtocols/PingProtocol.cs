using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Utilities.Network;


namespace HamstarHelpers.NetProtocols {
	class PingProtocol : PacketProtocol {
		public long StartTime = -1;
		public long EndTime = -1;



		////////////////

		public override bool IsVerbose { get { return false; } }


		public override void SetClientDefaults() {
			this.StartTime = (long)SystemHelpers.TimeStamp().TotalMilliseconds;
		}

		public override void SetServerDefaults() { }


		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			if( this.EndTime == -1 ) {
				this.SendToClient( from_who, -1 );
			} else {
				HamstarHelpersMod.Instance.ServerBrowser.UpdatePingAverage( (int)( this.EndTime - this.StartTime ) );
			}
		}


		protected override void ReceiveWithClient() {
			var now = (long)SystemHelpers.TimeStamp().TotalMilliseconds;

			this.EndTime = now;

			this.SendToServer( false );

			HamstarHelpersMod.Instance.NetHelpers.UpdatePing( (int)( now - this.StartTime ) );
		}
	}
}
