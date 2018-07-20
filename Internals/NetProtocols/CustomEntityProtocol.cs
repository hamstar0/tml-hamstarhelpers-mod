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

		public IList<CustomEntityComponent> Components;
		public int NetId;


		////////////////

		private CustomEntityProtocol( CustomEntity ent ) {
			this.NetId = ent.whoAmI;
			this.Components = new List<CustomEntityComponent>( ent.ComponentsInOrder.Count );

			for( int i = 0; i < ent.ComponentsInOrder.Count; i++ ) {
				CustomEntityComponent component = ent.ComponentsInOrder[ i ];
				this.Components.Add( component );
			}
		}

		////////////////

		protected override void ReceiveWithClient() {
			var ent = CustomEntityManager.Instance[ this.NetId ];
			ent.SetComponents( this.Components );
		}
	}
}
