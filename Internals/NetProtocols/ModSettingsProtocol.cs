using HamstarHelpers.Components.Protocols.Packet.Interfaces;


namespace HamstarHelpers.Internals.NetProtocols {
	/** @private */
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public static void QuickRequest() {
			PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>( -1 );
		}



		////////////////

		public HamstarHelpersConfigData Data;



		////////////////

		private ModSettingsProtocol() { }

		////////////////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = (HamstarHelpersConfigData)ModHelpersMod.Instance.Config.Clone();
			this.Data.PrivilegedUserId = "";
		}

		////////////////

		protected override void ReceiveReply() {
			ModHelpersMod.Instance.Config.LoadFromNetwork( this.Data );
		}
	}
}
