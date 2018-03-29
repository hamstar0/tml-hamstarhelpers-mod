using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Utilities.Network;
using System;
using System.IO;
using Terraria;


namespace HamstarHelpers.NetHelpers {
	class HHPingProtocol : PacketProtocol {
		public long StartTime = -1;
		public long EndTime = -1;



		////////////////

		public override bool IsVerbose { get { return false; } }
		

		public override void SetClientDefaults() {
			this.StartTime = SystemHelpers.TimeStampInSeconds();
		}

		public override void SetServerDefaults() { }


		////////////////

		public override void ReceiveOnServer( int from_who ) {
			if( this.EndTime == -1 ) {
				this.SendData( from_who, -1, false );
			} else {
				HamstarHelpersMod.Instance.ServerBrowser.UpdatePingAverage( (int)(this.EndTime - this.StartTime) );
			}
		}


		public override void ReceiveOnClient() {
			long now = SystemHelpers.TimeStampInSeconds();

			this.EndTime = now;

			this.SendData( -1, -1, false );

			HamstarHelpersMod.Instance.NetHelpers.UpdatePing( (int)(now - this.StartTime) );
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




	public partial class NetHelpers {
		public static int GetServerPing() {
			if( Main.netMode != 1 ) {
				throw new Exception("Only clients can gauge ping.");
			}

			return HamstarHelpersMod.Instance.NetHelpers.CurrentPing;
		}


		////////////////

		private int CurrentPing = -1;


		////////////////

		internal void UpdatePing( int ping ) {
			this.CurrentPing = ((this.CurrentPing * 2) + ping) / 3;
		}
	}
}
