using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class PlayerPromiseArguments : PromiseArguments {
		public int Who;
	}



	class HamstarHelpersPlayer : ModPlayer {
		internal readonly static object MyValidatorKey;
		internal readonly static PromiseValidator LoadValidator;
		internal readonly static PromiseValidator SaveValidator;


		////////////////

		static HamstarHelpersPlayer() {
			HamstarHelpersPlayer.MyValidatorKey = new object();
			HamstarHelpersPlayer.LoadValidator = new PromiseValidator( HamstarHelpersPlayer.MyValidatorKey );
			HamstarHelpersPlayer.SaveValidator = new PromiseValidator( HamstarHelpersPlayer.MyValidatorKey );
		}



		////////////////

		public PlayerLogic Logic { get; private set; }
//private readonly string MYUID = Guid.NewGuid().ToString();


		////////////////

		public override bool CloneNewInstances { get { return false; } }
		
		public override void Initialize() {
			this.Logic = new PlayerLogic();
//LogHelpers.Log( "CHECK "+this.MYUID+" Logic UID: "+this.Logic.PrivateUID+" "+this.Logic.HasUID );
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (HamstarHelpersPlayer)client_clone;
			clone.Logic = this.Logic;
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.Logic.OnEnterWorldServer( (HamstarHelpersMod)this.mod, this.player );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

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
//LogHelpers.Log( "LOAD "+this.MYUID+" Logic UID: "+this.Logic.PrivateUID+" "+this.Logic.HasUID );

			var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

			Promises.TriggerValidatedPromise( HamstarHelpersPlayer.LoadValidator, HamstarHelpersPlayer.MyValidatorKey, args );
		}

		public override TagCompound Save() {
			var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

			Promises.TriggerValidatedPromise( HamstarHelpersPlayer.SaveValidator, HamstarHelpersPlayer.MyValidatorKey, args );

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
			bool success;

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
				string file_name = DataDumper.DumpToFile( out success );
				Main.NewText( "Dumped latest debug data to log file "+file_name, Color.Azure );
			}
		}
	}
}
