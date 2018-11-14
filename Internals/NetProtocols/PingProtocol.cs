using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;

namespace HamstarHelpers.Internals.NetProtocols {
	class PingProtocol : PacketProtocolSentToEither {
		public long StartTime = -1;
		public long EndTime = -1;



		////////////////
		
		[PacketProtocolIgnore]
		public override bool IsVerbose { get { return false; } }


		////////////////

		protected PingProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////////////////

		protected override void SetClientDefaults() {
			this.StartTime = (long)SystemHelpers.TimeStamp().TotalMilliseconds;
		}

		protected override void SetServerDefaults( int from_who ) { }


		////////////////

		protected override void ReceiveOnServer( int from_who ) {
			if( this.EndTime == -1 ) {
				this.SendToClient( from_who, -1 );
			} else {
				ModHelpersMod.Instance.ServerInfo.UpdatePingAverage( (int)( this.EndTime - this.StartTime ) );
			}
		}


		protected override void ReceiveOnClient() {
			var now = (long)SystemHelpers.TimeStamp().TotalMilliseconds;

			this.EndTime = now;

			this.SendToServer( false );

			ModHelpersMod.Instance.NetHelpers.UpdatePing( (int)( now - this.StartTime ) );
		}
	}
}
