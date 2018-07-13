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

		public IList<CustomEntityData> Data;
		public int EntId;


		////////////////

		private CustomEntityProtocol( CustomEntity ent ) {
			this.Data = new List<CustomEntityData>( ent.PropertyDataOrder.Count );

			for( int i = 0; i < ent.PropertyDataOrder.Count; i++ ) {
				int idx = ent.PropertyDataOrder[i];
				var data = ent.PropertyData[idx];
				this.Data.Add( data );
			}
		}

		////////////////

		protected override void ReceiveWithClient() {
			var ent = CustomEntity.Get( this.EntId );
			ent.SetData( this.Data );
		}
	}
}
