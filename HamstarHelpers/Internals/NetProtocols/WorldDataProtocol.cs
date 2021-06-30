using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader;
using HamstarHelpers.Libraries.World;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class WorldDataRequestProtocol : SimplePacketPayload {	//NetIORequestPayloadFromServer<WorldDataProtocol> {
		public static void QuickRequest() {
			if( Main.netMode == NetmodeID.Server ) {
				throw new ModHelpersException( "No client." );
			}
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

			this.HalfDays = WorldStateLibraries.GetElapsedHalfDays();
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
			var myplayer = (ModHelpersPlayer)TmlLibraries.SafelyGetModPlayer( Main.LocalPlayer, ModHelpersMod.Instance, "ModHelpersPlayer" );

			myworld.HasObsoleteId = this.HasObsoletedWorldId;
			myworld.ObsoleteId = this.ObsoletedWorldId;

			mymod.WorldStateHelpers.LoadFromData( this.HalfDays, this.ObsoletedWorldId );

			myplayer.Logic.FinishWorldDataSyncOnLocal();
		}
	}
}