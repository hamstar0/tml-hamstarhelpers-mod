using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class ItemNoGrabProtocol : NetIOServerPayload {
		public static void SendToServer( int itemWho, int noGrabDelayAmt ) {
			var protocol = new ItemNoGrabProtocol( itemWho, noGrabDelayAmt );
			NetIO.SendToServer( protocol );
		}



		////////////////

		public int ItemWho;
		public int NoGrabDelayAmt;



		////////////////

		public ItemNoGrabProtocol() { }

		private ItemNoGrabProtocol( int itemWho, int noGrabDelayAmt ) {
			this.ItemWho = itemWho;
			this.NoGrabDelayAmt = noGrabDelayAmt;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			Item item = Main.item[this.ItemWho];
			if( item == null /*|| !item.active*/ ) {
				//throw new HamstarException( "!ModHelpers.ItemNoGrabProtocol.ReceiveWithServer - Invalid item indexed at "+this.ItemWho );
				throw new ModHelpersException( "Invalid item indexed at " + this.ItemWho );
			}

			item.noGrabDelay = this.NoGrabDelayAmt;
			item.ownIgnore = fromWho;
			item.ownTime = this.NoGrabDelayAmt;
		}
	}
}