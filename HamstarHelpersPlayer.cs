using HamstarHelpers.NetProtocol;
using HamstarHelpers.Utilities.Messages;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class HamstarHelpersPlayer : ModPlayer {
		public bool HasEnteredWorld { get; private set; }


		////////////////
		
		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (HamstarHelpersPlayer)client_clone;
			clone.HasEnteredWorld = this.HasEnteredWorld;
		}


		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI == this.player.whoAmI ) {    // Current player
				var mymod = (HamstarHelpersMod)this.mod;
				var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
				
				if( Main.netMode == 1 ) {   // Client
					ClientPacketHandlers.SendRequestModDataFromClient( mymod );
				} else if( Main.netMode == 0 ) {    // Single
					this.PostEnterWorld();
				}

				mymod.HasCurrentPlayerEnteredWorld = true;
			}
		}

		public void PostEnterWorld() {
			var mymod = (HamstarHelpersMod)this.mod;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			
			this.HasEnteredWorld = true;
		}


		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {	// Current player
				PlayerMessage.UpdatePlayerLabels();
				SimpleMessage.UpdateMessage();
			}

			var mymod = (HamstarHelpersMod)this.mod;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( Main.netMode != 2 ) {   // Not server
				if( this.player.whoAmI == Main.myPlayer ) { // Current player only
					if( modworld.HasCorrectID && this.HasEnteredWorld ) {
						modworld.Logic.Update( mymod );
					}
				}
			} else {    // Server
				modworld.Logic.ReadyServer = true;  // Needed?
			}
		}
	}
}
