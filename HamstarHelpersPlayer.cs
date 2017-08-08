using HamstarHelpers.Utilities.Messages;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class HamstarHelpersPlayer : ModPlayer {
		public bool HasEnteredWorld { get; private set; }

		public HamstarHelpersPlayer() : base() { }

		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (HamstarHelpersPlayer)client_clone;
			clone.HasEnteredWorld = this.HasEnteredWorld;
		}


		public override void OnEnterWorld( Player player ) {
			this.HasEnteredWorld = true;
		}


		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {	// Current player
				PlayerMessage.UpdatePlayerLabels();
				SimpleMessage.UpdateMessage();
			}
		}
	}
}
