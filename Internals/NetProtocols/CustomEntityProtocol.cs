using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityProtocol : PacketProtocol {
		public static void SendToClients( CustomEntity ent ) {
			var protocol = new CustomEntityProtocol( ent );
			protocol.SendToClient( -1, -1 );
		}


		////////////////

		public IList<CustomEntityPropertyData> Data;
		public int NetId;


		////////////////

		private CustomEntityProtocol( CustomEntity ent ) {
			this.NetId = ent.whoAmI;
			this.Data = new List<CustomEntityPropertyData>( ent.PropertyDataOrder.Count );

			for( int i = 0; i < ent.PropertyDataOrder.Count; i++ ) {
				int idx = ent.PropertyDataOrder[i];
				var data = ent.PropertyData[idx];
				this.Data.Add( data );
			}
		}

		////////////////

		protected override void ReceiveWithClient() {
			var ent = CustomEntityManager.Entities[ this.NetId ];
			ent.SetData( this.Data );
		}
	}
}
