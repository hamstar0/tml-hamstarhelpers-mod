using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class ItemNoGrabProtocol : PacketProtocol {
		public static void SendToServer( int item_who, int no_grab_delay_amt ) {
			var protocol = new ItemNoGrabProtocol( item_who, no_grab_delay_amt );
			protocol.SendToServer( false );
		}


		////////////////

		public int ItemWho;
		public int NoGrabDelayAmt;


		////////////////

		private ItemNoGrabProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		private ItemNoGrabProtocol( int item_who, int no_grab_delay_amt ) {
			this.ItemWho = item_who;
			this.NoGrabDelayAmt = no_grab_delay_amt;
		}

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
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
