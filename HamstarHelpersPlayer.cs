using HamstarHelpers.Logic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersPlayer : ModPlayer {
		private PlayerLogic _Logic = null;
		public PlayerLogic Logic {
			get {
				if( this._Logic == null ) { this._Logic = new PlayerLogic(); }
				return this._Logic;
			}
		}

		
		////////////////
		
		public override void clientClone( ModPlayer client_clone ) {
			var clone = (HamstarHelpersPlayer)client_clone;
			clone._Logic = this.Logic;
		}

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.Logic.OnEnterWorldForServer( (HamstarHelpersMod)this.mod, this.player );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( Main.netMode == 0 ) {
				this.Logic.OnEnterWorldForSingle( mymod, player );
			} else if( Main.netMode == 1 ) {
				this.Logic.OnEnterWorldForClient( mymod, player );
			}
		}

		public override void SendClientChanges( ModPlayer client_player ) {
			this.Logic.SendClientChanges( (HamstarHelpersMod)this.mod, this.player, client_player );
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
