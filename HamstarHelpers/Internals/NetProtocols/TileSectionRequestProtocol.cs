using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class TileSectionRequestProtocol : SimplePacketPayload {	//NetIOServerPayload {
		public static void SendToServer( int tileSectionX, int tileSectionY ) {
			SimplePacket.SendToServer( new TileSectionRequestProtocol(tileSectionX, tileSectionY) );
		}



		////////////////

		public int TileSectionX;
		public int TileSectionY;



		////////////////

		public TileSectionRequestProtocol( int tileSectionX, int tileSectionY ) {
			this.TileSectionX = tileSectionX;
			this.TileSectionY = tileSectionY;
		}

		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			var wldPos = new Vector2(
				this.TileSectionX * 200 * 16,
				this.TileSectionY * 150 * 16
			);

			RemoteClient.CheckSection( fromWho, wldPos, 1 );
			//NetMessage.SendSection( fromWho, this.TileSectionX, this.TileSectionY );
		}

		public override void ReceiveOnClient() {
			throw new NotImplementedException();
		}
	}
}