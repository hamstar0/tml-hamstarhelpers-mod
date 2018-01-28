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
		
		public override void SendClientChanges( ModPlayer client_player ) {
			this.Logic.SendClientChanges( (HamstarHelpersMod)this.mod, client_player );
		}

		public override void OnEnterWorld( Player player ) {
			this.Logic.OnEnterWorld( (HamstarHelpersMod)this.mod, player );
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
			this.Logic.Update( (HamstarHelpersMod)this.mod, this.player );
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggers_set ) {
			this.Logic.ProcessTriggers( (HamstarHelpersMod)this.mod, triggers_set );
		}
	}
}
