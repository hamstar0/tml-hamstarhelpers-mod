using HamstarHelpers.DisplayHelpers;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	public class HamstarHelpersPlayer : ModPlayer {
		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {
				PlayerMessage.UpdatePlayerLabels();
				SimpleMessage.UpdateMessage();
			}
		}
	}
}
