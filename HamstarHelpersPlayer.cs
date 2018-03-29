using HamstarHelpers.Logic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersPlayer : ModPlayer {
		public PlayerLogic Logic;

		
		////////////////
		
		public override void Initialize() {
			this.Logic = new PlayerLogic();
		}

		////////////////

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (HamstarHelpersPlayer)client_clone;
			clone.Logic = this.Logic;
		}

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.Logic.OnEnterWorldForServer( (HamstarHelpersMod)this.mod, this.player );
				}
			}
		}

		public override void SendClientChanges( ModPlayer client_player ) {
			this.Logic.SendClientChanges( (HamstarHelpersMod)this.mod, this.player, client_player );
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( Main.netMode == 0 ) {
				this.Logic.OnEnterWorldForSingle( mymod, player );
			} else if( Main.netMode == 1 ) {
				this.Logic.OnEnterWorldForClient( mymod, player );
			}
		}


		////////////////

		public override void Load( TagCompound tags ) {
			this.Logic.Load( tags );
		}

		public override TagCompound Save() {
			return this.Logic.Save();
		}


		////////////////

		public override void PreUpdate() {
			if( Main.netMode == 2 ) {
				this.Logic.PreUpdateServer( (HamstarHelpersMod)this.mod, this.player );
			} else if( Main.netMode == 1 ) {
				this.Logic.PreUpdateClient( (HamstarHelpersMod)this.mod, this.player );
			} else {
				this.Logic.PreUpdateSingle( (HamstarHelpersMod)this.mod, this.player );
			}
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggers_set ) {
			this.Logic.ProcessTriggers( (HamstarHelpersMod)this.mod, triggers_set );
		}
	}
}
