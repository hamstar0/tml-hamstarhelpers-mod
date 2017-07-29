using HamstarHelpers.Utilities.Messages;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class HamstarHelpersPlayer : ModPlayer {
		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {	// Current player
				PlayerMessage.UpdatePlayerLabels();
				SimpleMessage.UpdateMessage();
			}
		}
	}
}
