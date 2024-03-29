﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Buffs;
using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Cheats;
using HamstarHelpers.Services.Debug.DataDumper;
using HamstarHelpers.Services.UI.ControlPanel;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersPlayer : ModPlayer {
		public PlayerLogic Logic { get; private set; }


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void Initialize() {
			this.Logic = new PlayerLogic();
		}

		public override void clientClone( ModPlayer clientClone ) {
			var clone = (ModHelpersPlayer)clientClone;
			clone.Logic = this.Logic;
			//clone.Logic = this.Logic.Clone();
		}


		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				if( /*toWho == -1 && fromWho == -1 &&*/ newPlayer ) {
					this.Logic.OnCurrentClientConnect();
				}
			}
			if( Main.netMode == NetmodeID.Server ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.Logic.OnServerConnect( Main.player[fromWho] );
				}
			}
		}

		/*public override void SendClientChanges( ModPlayer clientPlayer ) {
			var myclone = clientPlayer as ModHelpersPlayer;
			
			if( !this.Logic.Equals(myclone) ) {
				this.Logic.Sync();
			}
		}*/


		////////////////

		public override void Load( TagCompound tags ) {
			try {
				this.Logic.Load( tags );
			} catch( Exception e ) {
				if( !(e is ModHelpersException) ) {
					throw new ModHelpersException( e.ToString() );
				}
			}
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

			mymod.ControlPanelUI.UpdateGlobal();

			if( Main.netMode == NetmodeID.Server ) {
				this.Logic.PreUpdateServer( this.player );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
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

		public override bool CanBeHitByNPC( NPC npc, ref int cooldownSlot ) {
			return !this.player.HasBuff( ModContent.BuffType<DegreelessnessBuff>() );
		}

		public override bool CanBeHitByProjectile( Projectile proj ) {
			return !this.player.HasBuff( ModContent.BuffType<DegreelessnessBuff>() );
		}


		////////////////

		public override void ModifyWeaponDamage( Item item, ref float add, ref float mult, ref float flat ) {
			PlayerCheats.OnHit( ref mult, ref flat, this.Logic.GetActiveCheatFlags() );
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggersSet ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;

			try {
				if( mymod.ControlPanelHotkey != null && mymod.ControlPanelHotkey.JustPressed ) {
					if( mymod.ControlPanelUI != null ) {
						if( mymod.ControlPanelUI.IsOpen ) {
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
