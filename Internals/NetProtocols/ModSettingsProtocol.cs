using HamstarHelpers.Components.Protocol.Packet.Interfaces;


namespace HamstarHelpers.Internals.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
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
