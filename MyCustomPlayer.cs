using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Internals.Logic;
using System;
using Terraria;


namespace HamstarHelpers {
	/// @private
	class ModHelpersCustomPlayer : CustomPlayerData {
		//public PlayerLogic Logic { get; private set; }



		////////////////
		
		protected ModHelpersCustomPlayer() { }


		protected override void OnEnter( object data ) {
LogHelpers.Log("hello");
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( this.Player );

			if( Main.netMode == 0 ) {
				myplayer.Logic.OnSingleEnterWorld( this.Player );
			} else if( Main.netMode == 1 ) {
				myplayer.Logic.OnCurrentClientEnterWorld( this.Player );
			} else if( Main.netMode == 2 ) {
			}
		}
	}
}
