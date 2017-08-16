using HamstarHelpers.NetProtocol;
using HamstarHelpers.Utilities.Messages;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class MyModPlayer : ModPlayer {
		public bool HasEnteredWorld { get; private set; }

		public bool IsAboveSurface { get; private set; }


		////////////////

		public MyModPlayer() : base() { }

		public override void Initialize() {
			this.HasEnteredWorld = false;
			this.IsAboveSurface = false;
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (MyModPlayer)client_clone;
			clone.HasEnteredWorld = this.HasEnteredWorld;
			clone.IsAboveSurface = this.IsAboveSurface;
		}


		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI == this.player.whoAmI ) {    // Current player
				var mymod = (HamstarHelpers)this.mod;
				var modworld = mymod.GetModWorld<MyModWorld>();
				
				if( Main.netMode == 1 ) {   // Client
					ClientNetProtocol.SendRequestModDataFromClient( mymod );
				} else if( Main.netMode == 0 ) {    // Single
					this.PostEnterWorld();
				}
			}
		}

		public void PostEnterWorld() {
			var mymod = (HamstarHelpers)this.mod;
			var modworld = mymod.GetModWorld<MyModWorld>();
			
			this.HasEnteredWorld = true;
		}


		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {	// Current player
				PlayerMessage.UpdatePlayerLabels();
				SimpleMessage.UpdateMessage();
			}
			
			if( this.player.position.Y < Main.worldSurface * 16.0 ) {
				this.IsAboveSurface = true;
			} else {
				this.IsAboveSurface = false;
			}

			var modworld = this.mod.GetModWorld<MyModWorld>();

			if( Main.netMode != 2 ) {   // Not server
				if( this.player.whoAmI == Main.myPlayer ) { // Current player only
					if( modworld.HasCorrectID && this.HasEnteredWorld ) {
						modworld.Logic.Update();
					}
				}
			} else {    // Server
				modworld.Logic.ReadyServer = true;  // Needed?
			}
		}
	}
}
