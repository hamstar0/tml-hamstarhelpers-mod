using HamstarHelpers.Utilities.Network;
using System;
using System.IO;
using Terraria;


namespace HamstarHelpers.NetHelpers {
	class HHPingProtocol : PacketProtocol {
		public long StartTime;


		////////////////
		
		public override void SetClientDefaults() {
			this.StartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}


		public override void ReceiveOnServer( int from_who ) {
			this.SendData( from_who, -1, false );
		}


		public override void ReceiveOnClient() {
			long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			long time = now - this.StartTime;

			HamstarHelpersMod.Instance.NetHelpers.UpdatePing( (int)time );
		}

		////////////////

		public override PacketProtocol ReadData( BinaryReader reader ) {
			this.StartTime = reader.ReadInt64();
			return this;
		}

		public override void WriteData( BinaryWriter writer, PacketProtocol me ) {
			writer.Write( ( (HHPingProtocol)me ).StartTime );
		}
	}




	public partial class NetHelpers {
		public static int GetServerPing() {
			if( Main.netMode != 1 ) {
				throw new Exception("Only clients can gauge ping.");
			}

			return HamstarHelpersMod.Instance.NetHelpers.CurrentPing;
		}


		////////////////

		private int CurrentPing = 0;


		////////////////

		internal void UpdatePing( int ping ) {
			this.CurrentPing = (this.CurrentPing + ping) / 2;
		}
	}
}
