using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Players;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
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



	class ModHelpersPlayer : ModPlayer {
		internal readonly static object MyValidatorKey;
		internal readonly static PromiseValidator LoadValidator;
		internal readonly static PromiseValidator SaveValidator;


		////////////////

		static ModHelpersPlayer() {
			ModHelpersPlayer.MyValidatorKey = new object();
			ModHelpersPlayer.LoadValidator = new PromiseValidator( ModHelpersPlayer.MyValidatorKey );
			ModHelpersPlayer.SaveValidator = new PromiseValidator( ModHelpersPlayer.MyValidatorKey );
		}



		////////////////

		public PlayerLogic Logic { get; private set; }


		////////////////

		public override bool CloneNewInstances { get { return false; } }
		
		public override void Initialize() {
			this.Logic = new PlayerLogic();
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (ModHelpersPlayer)client_clone;
			clone.Logic = this.Logic;
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 1 ) {
				if( to_who == -1 && new_player ) {
					PacketProtocol.QuickSendToServer<PlayerNewIdProtocol>();
				}
			} else if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					PacketProtocol.QuickSendToClient<PlayerNewIdProtocol>( from_who, -1 );

					Promises.AddSafeWorldLoadOncePromise( () => {
						this.Logic.OnServerConnect( ModHelpersMod.Instance, Main.player[from_who] );
					} );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			bool has_entered_world = false;
			int who = player.whoAmI;

			Action run = () => {
				var mymod = (ModHelpersMod)this.mod;

				if( Main.netMode == 0 ) {
					this.Logic.OnSingleConnect( mymod, Main.player[who] );
				} else if( Main.netMode == 1 ) {
					this.Logic.OnClientConnect( mymod, Main.player[who] );
				}
			};

			Promises.AddValidatedPromise<PlayerPromiseArguments>( ModHelpersPlayer.LoadValidator, ( args ) => {
				if( args.Who != who ) { return false; }

				run();

				has_entered_world = true;
				return false;
			} );

			Timers.SetTimer( "ModHelpersOnEnterWorldFailsafe", 2 * 60, () => {
				if( !has_entered_world ) {
					Main.NewText( "Warning: Player ID failed to load. Some mods might fail to load properly.", Color.Red );
					Main.NewText( "To fix, try restarting game or reloading mods. If this happens again, please report this issue.", Color.DarkGray );

					run();  // Run anyway
				}
				return false;
			} );
		}


		////////////////

		public override void Load( TagCompound tags ) {
			PlayerData.LoadAll( this.player.whoAmI, tags );

			this.Logic.Load( tags );
			
			var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

			Promises.TriggerValidatedPromise( ModHelpersPlayer.LoadValidator, ModHelpersPlayer.MyValidatorKey, args );
		}

		public override TagCompound Save() {
			var tags = new TagCompound();
			var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

			PlayerData.SaveAll( this.player.whoAmI, tags );
			
			Promises.TriggerValidatedPromise( ModHelpersPlayer.SaveValidator, ModHelpersPlayer.MyValidatorKey, args );

			this.Logic.Save( tags );

			return tags;
		}


		////////////////

		public override void PreUpdate() {
			if( Main.netMode == 2 ) {
				this.Logic.PreUpdateServer( (ModHelpersMod)this.mod, this.player );
			} else if( Main.netMode == 1 ) {
				this.Logic.PreUpdateClient( (ModHelpersMod)this.mod, this.player );
			} else {
				this.Logic.PreUpdateSingle( (ModHelpersMod)this.mod );
			}
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggers_set ) {
			var mymod = (ModHelpersMod)this.mod;
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

			mymod.CustomHotkeys.ProcessTriggers( triggers_set );
		}
	}
}
