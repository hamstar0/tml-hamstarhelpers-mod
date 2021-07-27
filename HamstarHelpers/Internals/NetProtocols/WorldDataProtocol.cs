using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class WorldDataRequestProtocol : SimplePacketPayload {	//NetIORequestPayloadFromServer<WorldDataProtocol> {
		public static void QuickRequest() {
			SimplePacket.SendToServer( new WorldDataRequestProtocol() );
		}



		////////////////

		public WorldDataRequestProtocol() { }

		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			SimplePacket.SendToClient( new WorldDataProtocol(), fromWho, -1 );
		}

		public override void ReceiveOnClient() {
			throw new NotImplementedException();
		}
	}




	[Serializable]
	class WorldDataProtocol : SimplePacketPayload { //NetIOClientPayload {
		public int HalfDays;
		public bool HasObsoletedWorldId;
		public string ObsoletedWorldId;



		////////////////

		public WorldDataProtocol() {
			var myworld = ModContent.GetInstance<ModHelpersWorld>();

			this.HalfDays = WorldStateHelpers.GetElapsedHalfDays();
			this.HasObsoletedWorldId = myworld.HasObsoleteId;
			this.ObsoletedWorldId = myworld.ObsoleteId;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException();
		}

		public override void ReceiveOnClient() {
			var mymod = ModHelpersMod.Instance;
			var myworld = ModContent.GetInstance<ModHelpersWorld>();
			var myplayer = (ModHelpersPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, ModHelpersMod.Instance, "ModHelpersPlayer" );

			myworld.HasObsoleteId = this.HasObsoletedWorldId;
			myworld.ObsoleteId = this.ObsoletedWorldId;

			mymod.WorldStateHelpers.LoadFromData( this.HalfDays, this.ObsoletedWorldId );

			myplayer.Logic.FinishWorldDataSyncOnLocal();
		}
	}
}