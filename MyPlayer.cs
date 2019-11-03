using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Services.Debug.DataDumper;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.ControlPanel;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	/// @private
	class ModHelpersPlayer : ModPlayer {
		public PlayerLogic Logic { get; private set; }


		////////////////

		public override bool CloneNewInstances => false;
		
		public override void Initialize() {
			this.Logic = new PlayerLogic();
		}

		public override void clientClone( ModPlayer clientClone ) {
			var clone = (ModHelpersPlayer)clientClone;
			clone.Logic = this.Logic;
		}


		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == 1 ) {
				if( toWho == -1 && fromWho == -1 && newPlayer ) {
					this.Logic.OnCurrentClientConnect( this.player );
				}
			}
			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.Logic.OnServerConnect( Main.player[fromWho] );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_A", 1 );
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }
			
			int who = player.whoAmI;
			
			if( Main.netMode == 0 ) {
				this.Logic.OnSingleEnterWorld( Main.player[who] );
			} else if( Main.netMode == 1 ) {
				this.Logic.OnCurrentClientEnterWorld( Main.player[who] );
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_B", 1 );
		}


		////////////////

		public override void Load( TagCompound tags ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_A", 1 );
			try {
				this.Logic.Load( tags );
			} catch( Exception e ) {
				if( !(e is ModHelpersException) ) {
					//throw new HamstarException( "!ModHelpers.ModHelpersPlayer.Load - " + e.ToString() );
					throw new ModHelpersException( e.ToString() );
				}
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_B", 1 );
		}

		public override TagCompound Save() {
			var tags = new TagCompound();
			try {
				this.Logic.Save( tags );
			} catch( Exception e ) {
				if( !(e is ModHelpersException) ) {
					throw new ModHelpersException( e.ToString() );
				}
			}

			return tags;
		}


		////////////////

		public override void PreUpdate() {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;

			mymod.ControlPanel.UpdateGlobal();

			if( Main.netMode == 2 ) {
				this.Logic.PreUpdateServer( this.player );
			} else if( Main.netMode == 1 ) {
				this.Logic.PreUpdateClient( this.player );
			} else {
				this.Logic.PreUpdateSingle();
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_B", 1 );
		}

		public override void PostUpdateRunSpeeds() {    //PostUpdate?
			var mymod = (ModHelpersMod)this.mod;

			if( player.whoAmI == Main.myPlayer && Main.playerInventory ) { // Current player
				mymod.RecipeHack.Update();
			}
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggersSet ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;

			try {
				if( mymod.ControlPanelHotkey != null && mymod.ControlPanelHotkey.JustPressed ) {
					if( mymod.ControlPanel != null ) {
						if( mymod.ControlPanel.IsOpen ) {
							ControlPanelTabs.CloseDialog();
						} else {
							ControlPanelTabs.OpenTab( UIControlPanel.DefaultTabName );
						}
					}
				}
			} catch( Exception e ) {
				LogHelpers.Warn( "(1) - " + e.ToString() );
				return;
			}

			try {
				if( mymod.DataDumpHotkey != null && mymod.DataDumpHotkey.JustPressed ) {
					string fileName;
					if( DataDumper.DumpToFile( out fileName ) ) {
						string msg = "Dumped latest debug data to log file " + fileName;

						Main.NewText( msg, Color.Azure );
						LogHelpers.Log( msg );
					}
				}
			} catch(Exception e ) {
				LogHelpers.Warn( "(2) - " + e.ToString() );
				return;
			}

			try {
				if( mymod.CustomHotkeys != null ) {
					mymod.CustomHotkeys.ProcessTriggers( triggersSet );
				}
			} catch(Exception e ) {
				LogHelpers.Warn( "(3) - " + e.ToString() );
				return;
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_B", 1 );
		}
	}
}
