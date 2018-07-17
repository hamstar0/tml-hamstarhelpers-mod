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

		public IList<CustomEntityComponentData> Data;
		public int NetId;


		////////////////

		private CustomEntityProtocol( CustomEntity ent ) {
			this.NetId = ent.whoAmI;
			this.Data = new List<CustomEntityComponentData>( ent.ComponentDataOrder.Count );

			for( int i = 0; i < ent.ComponentDataOrder.Count; i++ ) {
				int idx = ent.ComponentDataOrder[i];
				var data = ent.ComponentData[idx];
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
