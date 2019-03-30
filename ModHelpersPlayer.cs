using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
			
			var mymod = (ModHelpersMod)this.mod;
				
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
				//PlayerData.LoadAll( this.player.whoAmI, tags );

				this.Logic.Load( tags );

				var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

				Promises.TriggerValidatedPromise( ModHelpersPlayer.LoadValidator, ModHelpersPlayer.MyValidatorKey, args );
			} catch( Exception e ) {
				if( !(e is HamstarException) ) {
					//throw new HamstarException( "!ModHelpers.ModHelpersPlayer.Load - " + e.ToString() );
					throw new HamstarException( e.ToString() );
				}
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_B", 1 );
		}

		public override TagCompound Save() {
			var tags = new TagCompound();
			try {
				var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

				//PlayerData.SaveAll( this.player.whoAmI, tags );
			
				Promises.TriggerValidatedPromise( ModHelpersPlayer.SaveValidator, ModHelpersPlayer.MyValidatorKey, args );

				this.Logic.Save( tags );
			} catch( Exception e ) {
				if( !(e is HamstarException) ) {
					throw new HamstarException( e.ToString() );
				}
			}

			return tags;
		}


		////////////////

		public override void PreUpdate() {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<HitRadiusPlayerEntityComponent>();

			foreach( CustomEntity ent in ents ) {
				var hitRadComp = ent.GetComponentByType<HitRadiusPlayerEntityComponent>();
				float radius = hitRadComp.GetRadius( ent );

				if( Vector2.Distance( ent.Core.Center, this.player.Center ) <= radius ) {
					int dmg = 0;
					if( hitRadComp.PreHurt( ent, this.player, ref dmg ) ) {
						hitRadComp.PostHurt( ent, this.player, dmg );
					}
				}
			}

			if( Main.netMode == 2 ) {
				this.Logic.PreUpdateServer( this.player );
			} else if( Main.netMode == 1 ) {
				this.Logic.PreUpdateClient( this.player );
			} else {
				this.Logic.PreUpdateSingle();
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_B", 1 );
		}

		public override void PostUpdate() {
			var mymod = (ModHelpersMod)this.mod;

			if( player.whoAmI == Main.myPlayer ) { // Current player
				mymod.RecipeHack.Update();
			}
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggersSet ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;

			try {
				if( mymod.ControlPanelHotkey != null && mymod.ControlPanelHotkey.JustPressed ) {
					if( mymod.Config.DisableControlPanelHotkey ) {
						Main.NewText( "Control panel hotkey disabled.", Color.Red );
					} else {
						if( mymod.ControlPanel != null ) {
							if( mymod.ControlPanel.IsOpen ) {
								mymod.ControlPanel.Open();
							} else {
								mymod.ControlPanel.Close();
							}
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
