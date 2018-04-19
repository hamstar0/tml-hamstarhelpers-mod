using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Utilities.Network;
using System.IO;


namespace HamstarHelpers.NetProtocols {
	class HHPingProtocol : PacketProtocol {
		public long StartTime = -1;
		public long EndTime = -1;



		////////////////

		public override bool IsVerbose { get { return false; } }


		public override void SetClientDefaults() {
			this.StartTime = (long)SystemHelpers.TimeStamp().TotalMilliseconds;
		}

		public override void SetServerDefaults() { }


		////////////////

		public override void ReceiveOnServer( int from_who ) {
			if( this.EndTime == -1 ) {
				this.SendToClient( from_who, -1 );
			} else {
				HamstarHelpersMod.Instance.ServerBrowser.UpdatePingAverage( (int)( this.EndTime - this.StartTime ) );
			}
		}


		public override void ReceiveOnClient() {
			var now = (long)SystemHelpers.TimeStamp().TotalMilliseconds;

			this.EndTime = now;

			this.SendToServer( false );

			HamstarHelpersMod.Instance.NetHelpers.UpdatePing( (int)( now - this.StartTime ) );
		}

		////////////////

		public override PacketProtocol ReadData( BinaryReader reader ) {
			this.StartTime = reader.ReadInt64();
			this.EndTime = reader.ReadInt64();
			return this;
		}

		public override void WriteData( BinaryWriter writer, PacketProtocol me ) {
			var self = (HHPingProtocol)me;
			writer.Write( self.StartTime );
			writer.Write( self.EndTime );
		}
	}
}
