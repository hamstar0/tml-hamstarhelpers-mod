using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;


namespace HamstarHelpers.Internals.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public HamstarHelpersConfigData Data;


		////////////////

		protected ModSettingsProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////////////////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = (HamstarHelpersConfigData)ModHelpersMod.Instance.Config.Clone();
			this.Data.PrivilegedUserId = "";
		}

		////////////////

		protected override void ReceiveReply() {
			ModHelpersMod.Instance.Config.LoadFromNetwork( ModHelpersMod.Instance, this.Data );
		}
	}
}
