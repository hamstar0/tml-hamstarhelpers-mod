using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class PlayerLoadSave {
		public Player MyPlayer = null;
	}




	class HamstarHelpersPlayer : ModPlayer {
		internal static readonly PlayerLoadSave PlayerLoad = new PlayerLoadSave();
		internal static readonly PlayerLoadSave PlayerSave = new PlayerLoadSave();
		


		////////////////

		public override bool CloneNewInstances { get { return false; } }
		
		public PlayerLogic Logic { get; private set; }


		////////////////

		public override void Initialize() {
			this.Logic = new PlayerLogic();
		}
		
		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.Logic.OnEnterWorldServer( (HamstarHelpersMod)this.mod, this.player );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( Main.netMode == 0 ) {
				this.Logic.OnEnterWorldSingle( mymod, player );
			} else if( Main.netMode == 1 ) {
				this.Logic.OnEnterWorldClient( mymod, player );
			}
		}


		////////////////

		public override void Load( TagCompound tags ) {
			this.Logic.Load( tags );

			HamstarHelpersPlayer.PlayerLoad.MyPlayer = this.player;
			Promises.TriggerCustomPromiseForObject( HamstarHelpersPlayer.PlayerLoad );
		}

		public override TagCompound Save() {
			HamstarHelpersPlayer.PlayerSave.MyPlayer = this.player;
			Promises.TriggerCustomPromiseForObject( HamstarHelpersPlayer.PlayerSave );

			return this.Logic.Save();
		}


		////////////////

		public override void PreUpdate() {
			if( Main.netMode == 2 ) {
				this.Logic.PreUpdateServer( (HamstarHelpersMod)this.mod, this.player );
			} else if( Main.netMode == 1 ) {
				this.Logic.PreUpdateClient( (HamstarHelpersMod)this.mod, this.player );
			} else {
				this.Logic.PreUpdateSingle( (HamstarHelpersMod)this.mod );
			}
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggers_set ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( mymod.ControlPanelHotkey.JustPressed ) {
				if( mymod.Config.DisableControlPanelHotkey ) {
					Main.NewText( "Control panel hotkey disabled.", Color.Red );
				} else {
					mymod.ControlPanel.Open();
				}
			}
		}
	}
}
