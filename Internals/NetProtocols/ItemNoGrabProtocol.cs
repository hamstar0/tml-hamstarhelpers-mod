using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using System;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class ItemNoGrabProtocol : PacketProtocolSendToServer {
		protected class MyFactory {
			private readonly int ItemWho;
			private readonly int NoGrabDelayAmt;
			
			public MyFactory( int itemWho, int noGrabDelayAmt ) {
				this.ItemWho = itemWho;
				this.NoGrabDelayAmt = noGrabDelayAmt;
			}
		}



		////////////////

		public static void SendToServer( int itemWho, int noGrabDelayAmt ) {
			var factory = new MyFactory( itemWho, noGrabDelayAmt );
			var protocol = PacketProtocolData.CreateDefault<ItemNoGrabProtocol>( factory );
			
			protocol.SendToServer( false );
		}



		////////////////

		public int ItemWho;
		public int NoGrabDelayAmt;



		////////////////

		protected ItemNoGrabProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		protected override Type GetMyFactoryType() {
			return typeof( MyFactory );
		}

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
