﻿using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Cheats;


namespace HamstarHelpers.Internals.Logic {
	/// @private
	partial class PlayerLogic {
		private void PreUpdateLocal( Player player ) {
			var mymod = ModHelpersMod.Instance;

			if( player.whoAmI == Main.myPlayer ) { // Current player
				mymod.PlayerMessages.Update();
				this.DialogManager.Update();
			}

			foreach( int buffId in this.PermaBuffsById ) {
				player.AddBuff( buffId, 3 );
			}

			PlayerCheats.UpdateCheats( player, this.ActiveCheats );

			this.UpdateTml( player );
		}


		////////////////

		public void PreUpdateSingle() {
			this.PreUpdateLocal( Main.LocalPlayer );
		}

		public void PreUpdateClient( Player player ) {
			this.PreUpdateLocal( player );

			if( player.whoAmI == Main.myPlayer ) { // Current player
				var myworld = ModContent.GetInstance<ModHelpersWorld>();
				myworld.WorldLogic.PreUpdateClient();

				// Update ping every 15 seconds
				if( ModHelpersConfig.Instance.IsServerGaugingAveragePing ) {
					if( this.TestPing++ > ModHelpersConfig.Instance.PingUpdateDelay ) {
						PingProtocol.QuickSendToServer();
						this.TestPing = 0;
					}
				}
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
