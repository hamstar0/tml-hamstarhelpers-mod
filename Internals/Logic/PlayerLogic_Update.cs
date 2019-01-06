using HamstarHelpers.Components.Network;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Timers;
using Terraria;


namespace HamstarHelpers.Internals.Logic {
	partial class PlayerLogic {
		private void PreUpdatePlayer( ModHelpersMod mymod, Player player ) {
			if( player.whoAmI == Main.myPlayer ) { // Current player
				var modworld = mymod.GetModWorld<ModHelpersWorld>();

				SimpleMessage.UpdateMessage();
				mymod.PlayerMessages.Update();
				this.DialogManager.Update( mymod );
			}

			foreach( int buffId in this.PermaBuffsById ) {
				player.AddBuff( buffId, 3 );
			}

			this.UpdateTml( mymod, player );
		}

		////////////////

		public void PreUpdateSingle( ModHelpersMod mymod ) {
			this.PreUpdatePlayer( mymod, Main.LocalPlayer );
		}

		public void PreUpdateClient( ModHelpersMod mymod, Player player ) {
			this.PreUpdatePlayer( mymod, player );

			if( player.whoAmI == Main.myPlayer ) { // Current player
				var myworld = mymod.GetModWorld<ModHelpersWorld>();
				myworld.WorldLogic.PreUpdateClient( mymod );
			}

			// Update ping every 15 seconds
			if( mymod.Config.IsServerGaugingAveragePing && this.TestPing++ > (60*15) ) {
				PacketProtocolSentToEither.QuickSendToServer<PingProtocol>();
				this.TestPing = 0;
			}
		}

		public void PreUpdateServer( ModHelpersMod mymod, Player player ) {
			if( player.whoAmI == Main.myPlayer ) { // Current player
				var modworld = mymod.GetModWorld<ModHelpersWorld>();
			}
			if( player.whoAmI != 255 ) {
				mymod.LoadHelpers.HasServerBegunHavingPlayers_Hackish = true;	// Weird hack?
			}

			foreach( int buffId in this.PermaBuffsById ) {
				player.AddBuff( buffId, 3 );
			}

			this.UpdateTml( mymod, player );

			// Every player must have their ids accounted for!
			if( !mymod.PlayerIdentityHelpers.PlayerIds.ContainsKey(player.whoAmI) ) {
				string timerName = "ModHelpersPlayerIdFailsafe_" + player.whoAmI;

				if( Timers.GetTimerTickDuration( timerName ) == 0 ) {
					Timers.SetTimer( timerName, 3 * 60, () => {
						PacketProtocolSentToEither.QuickRequestToClient<PlayerNewIdProtocol>( player.whoAmI, -1 );
						return false;
					} );
				}
			}
		}
	}
}
