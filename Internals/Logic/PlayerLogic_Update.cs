using HamstarHelpers.Components.Network;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Messages;
using Terraria;


namespace HamstarHelpers.Internals.Logic {
	partial class PlayerLogic {
		private void PreUpdatePlayer( HamstarHelpersMod mymod, Player player ) {
			if( player.whoAmI == Main.myPlayer ) { // Current player
				var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

				SimpleMessage.UpdateMessage();
				mymod.PlayerMessages.Update();
				this.DialogManager.Update( mymod );
			}

			foreach( int buff_id in this.PermaBuffsById ) {
				player.AddBuff( buff_id, 3 );
			}

			this.UpdateTml( mymod, player );
		}

		////////////////

		public void PreUpdateSingle( HamstarHelpersMod mymod ) {
			this.PreUpdatePlayer( mymod, Main.LocalPlayer );
		}

		public void PreUpdateClient( HamstarHelpersMod mymod, Player player ) {
			this.PreUpdatePlayer( mymod, player );

			if( player.whoAmI == Main.myPlayer ) { // Current player
				var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
				myworld.WorldLogic.PreUpdateClient( mymod );
			}

			// Update ping every 15 seconds
			if( mymod.Config.IsServerGaugingAveragePing && this.TestPing++ > (60*15) ) {
				PacketProtocol.QuickSendToServer<PingProtocol>();
				this.TestPing = 0;
			}
		}

		public void PreUpdateServer( HamstarHelpersMod mymod, Player player ) {
			if( player.whoAmI == Main.myPlayer ) { // Current player
				var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			}
			if( player.whoAmI != 255 ) {
				mymod.LoadHelpers.HasServerBegunHavingPlayers = true;
			}

			foreach( int buff_id in this.PermaBuffsById ) {
				player.AddBuff( buff_id, 3 );
			}

			this.UpdateTml( mymod, player );
		}
	}
}
