using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers {
	/// @private
	class ModHelpersCustomPlayer : CustomPlayerData {
		//public PlayerLogic Logic { get; private set; }



		////////////////
		
		protected ModHelpersCustomPlayer() { }


		protected override void OnEnter( bool isCurrentPlayer, object data ) {
			if( Main.netMode != NetmodeID.Server ) {
				if( !isCurrentPlayer ) {
					return;
				}
			}

			if( Main.netMode == NetmodeID.SinglePlayer ) {
				var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( this.Player );
				myplayer.Logic.OnSingleEnterWorld( this.Player );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( this.Player );
				myplayer.Logic.OnCurrentClientEnterWorld( this.Player );
			} else if( Main.netMode == NetmodeID.Server ) {
			}
		}
	}
}
