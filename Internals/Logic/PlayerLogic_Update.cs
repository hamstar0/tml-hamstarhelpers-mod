using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Messages;
using Terraria;


namespace HamstarHelpers.Internals.Logic {
	partial class PlayerLogic {
		private void PreUpdatePlayer( Player player ) {
			var mymod = ModHelpersMod.Instance;

			if( player.whoAmI == Main.myPlayer ) { // Current player
				var modworld = mymod.GetModWorld<ModHelpersWorld>();

				SimpleMessage.UpdateMessage();
				mymod.PlayerMessages.Update();
				this.DialogManager.Update();
			}

			foreach( int buffId in this.PermaBuffsById ) {
				player.AddBuff( buffId, 3 );
			}

			this.UpdateTml( player );
		}

		////////////////

		public void PreUpdateSingle() {
			this.PreUpdatePlayer( Main.LocalPlayer );
		}

		public void PreUpdateClient( Player player ) {
			this.PreUpdatePlayer( player );

			var mymod = ModHelpersMod.Instance;

			if( player.whoAmI == Main.myPlayer ) { // Current player
				var myworld = mymod.GetModWorld<ModHelpersWorld>();
				myworld.WorldLogic.PreUpdateClient();
			}

			// Update ping every 15 seconds
			if( mymod.Config.IsServerGaugingAveragePing && this.TestPing++ > (60*15) ) {
				PacketProtocolSentToEither.QuickSendToServer<PingProtocol>();
				this.TestPing = 0;
			}
		}

		public void PreUpdateServer( Player player ) {
			var mymod = ModHelpersMod.Instance;

			if( player.whoAmI != 255 ) {
				mymod.LoadHelpers.HasServerBegunHavingPlayers_Hackish = true;	// Weird hack?
			}

			foreach( int buffId in this.PermaBuffsById ) {
				player.AddBuff( buffId, 3 );
			}

			this.UpdateTml( player );
		}
	}
}
