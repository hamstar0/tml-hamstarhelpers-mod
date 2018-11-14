using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class ItemNoGrabProtocol : PacketProtocolRequestToClient {
		protected class MyFactory : PacketProtocolData.Factory<ItemNoGrabProtocol> {
			public MyFactory( int item_who, int no_grab_delay_amt, out ItemNoGrabProtocol protocol ) : base( out protocol ) {
				protocol.ItemWho = item_who;
				protocol.NoGrabDelayAmt = no_grab_delay_amt;
			}
		}


		////////////////

		public static void SendToServer( int item_who, int no_grab_delay_amt ) {
			ItemNoGrabProtocol protocol;
			new MyFactory( item_who, no_grab_delay_amt, out protocol );
			
			protocol.SendToServer( false );
		}


		////////////////

		public int ItemWho;
		public int NoGrabDelayAmt;


		////////////////

		protected ItemNoGrabProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////////////////

		protected override void Receive( int from_who ) {
			Item item = Main.item[ this.ItemWho ];
			if( item == null /*|| !item.active*/ ) {
				throw new HamstarException( "!ModHelpers.ItemNoGrabProtocol.ReceiveWithServer - Invalid item indexed at "+this.ItemWho );
			}

			item.noGrabDelay = this.NoGrabDelayAmt;
			item.ownIgnore = from_who;
			item.ownTime = this.NoGrabDelayAmt;
		}
	}
}
