using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	internal class PlayerPromiseValidator : PromiseValidator {
		internal readonly static object MyValidatorKey = new object();
		internal readonly static PlayerPromiseValidator LoadValidator;
		internal readonly static PlayerPromiseValidator SaveValidator;

		////////////////

		static PlayerPromiseValidator() {
			PlayerPromiseValidator.LoadValidator = new PlayerPromiseValidator();
			PlayerPromiseValidator.SaveValidator = new PlayerPromiseValidator();
		}

		////////////////

		public Player MyPlayer = null;

		////////////////

		public PlayerPromiseValidator() : base( PlayerPromiseValidator.MyValidatorKey ) { }
	}




	class HamstarHelpersPlayer : ModPlayer {
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

			PlayerPromiseValidator.LoadValidator.MyPlayer = this.player;
			Promises.TriggerValidatedPromise( PlayerPromiseValidator.LoadValidator, PlayerPromiseValidator.MyValidatorKey );
		}

		public override TagCompound Save() {
			PlayerPromiseValidator.SaveValidator.MyPlayer = this.player;
			Promises.TriggerValidatedPromise( PlayerPromiseValidator.SaveValidator, PlayerPromiseValidator.MyValidatorKey );

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
					if( mymod.ControlPanel.IsOpen ) {
						mymod.ControlPanel.Open();
					} else {
						mymod.ControlPanel.Close();
					}
				}
			}

			if( mymod.DataDumpHotkey.JustPressed ) {
				string file_name = DataDumper.DumpToFile();
				Main.NewText( "Dumped latest debug data to log file "+file_name, Color.Azure );
			}
		}
	}
}
