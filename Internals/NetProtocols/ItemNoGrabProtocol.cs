using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Protocols.Packet.Interfaces;
using System;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	/** @private */
	class ItemNoGrabProtocol : PacketProtocolSendToServer {
		public static void SendToServer( int itemWho, int noGrabDelayAmt ) {
			var protocol = new ItemNoGrabProtocol( itemWho, noGrabDelayAmt );
			protocol.SendToServer( false );
		}



		////////////////

		public int ItemWho;
		public int NoGrabDelayAmt;



		////////////////

		private ItemNoGrabProtocol() { }

		private ItemNoGrabProtocol( int itemWho, int noGrabDelayAmt ) {
			this.ItemWho = itemWho;
			this.NoGrabDelayAmt = noGrabDelayAmt;
		}

		////

		protected override void InitializeClientSendData() { }


		////////////////

		protected override void Receive( int fromWho ) {
			Item item = Main.item[ this.ItemWho ];
			if( item == null /*|| !item.active*/ ) {
				//throw new HamstarException( "!ModHelpers.ItemNoGrabProtocol.ReceiveWithServer - Invalid item indexed at "+this.ItemWho );
				throw new HamstarException( "Invalid item indexed at "+this.ItemWho );
			}

			item.noGrabDelay = this.NoGrabDelayAmt;
			item.ownIgnore = fromWho;
			item.ownTime = this.NoGrabDelayAmt;
		}
	}
}
