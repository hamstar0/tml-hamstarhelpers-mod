using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class ItemNoGrabProtocol : PacketProtocolRequestToClient {
		protected class MyFactory : PacketProtocolData.Factory<ItemNoGrabProtocol> {
			private readonly int ItemWho;
			private readonly int NoGrabDelayAmt;


			////////////////

			public MyFactory( int itemWho, int noGrabDelayAmt ) {
				this.ItemWho = itemWho;
				this.NoGrabDelayAmt = noGrabDelayAmt;
			}

			////

			protected override void Initialize( ItemNoGrabProtocol data ) {
				data.ItemWho = this.ItemWho;
				data.NoGrabDelayAmt = this.NoGrabDelayAmt;
			}
		}



		////////////////

		public static void SendToServer( int itemWho, int noGrabDelayAmt ) {
			var factory = new MyFactory( itemWho, noGrabDelayAmt );
			ItemNoGrabProtocol protocol = factory.Create();
			
			protocol.SendToServer( false );
		}



		////////////////

		public int ItemWho;
		public int NoGrabDelayAmt;


		////////////////

		protected ItemNoGrabProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////////////////

		protected override void ReceiveReply( int fromWho ) {
			Item item = Main.item[ this.ItemWho ];
			if( item == null /*|| !item.active*/ ) {
				throw new HamstarException( "!ModHelpers.ItemNoGrabProtocol.ReceiveWithServer - Invalid item indexed at "+this.ItemWho );
			}

			item.noGrabDelay = this.NoGrabDelayAmt;
			item.ownIgnore = fromWho;
			item.ownTime = this.NoGrabDelayAmt;
		}
	}
}
