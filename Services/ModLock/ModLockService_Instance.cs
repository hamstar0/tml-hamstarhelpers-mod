using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Services.Hooks.LoadHooks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Services.ModHelpers {
	/// <summary>
	/// Provides a way to lock the given current loaded mods with a given world. May also be accessed in-game via. the
	/// Mod Helpers control panel.
	/// </summary>
	public partial class ModLockService {
		internal IDictionary<string, ISet<string>> WorldModLocks { get; private set; }
		private bool IsInitialized = false;
		private bool IsMismatched = false;
		private int ExitDuration = 60 * 20;
		private bool MismatchBroadcastMade = false;

		private ISet<string> FoundModNames = new HashSet<string>();
		private ISet<string> MissingModNames = new HashSet<string>();
		private ISet<string> ExtraModNames = new HashSet<string>();



		////////////////

		internal ModLockService() {
			this.WorldModLocks = new Dictionary<string, ISet<string>>();
			this.MismatchBroadcastMade = false;

			LoadHooks.AddWorldUnloadEachHook( this.OnWorldExit );
		}

		////////////////

		internal void PostLoad( ModHelpersWorld modworld ) {
			this.IsInitialized = true;
			this.MismatchBroadcastMade = false;
			this.ScanMods( modworld );
			this.ExitDuration = 60 * 20;
		}

		private void OnWorldExit() {
			this.MismatchBroadcastMade = false;
			this.ExitDuration = 60 * 20;
		}


		////////////////

		private void ScanMods( ModHelpersWorld modworld ) {
			this.FoundModNames = new HashSet<string>();
			this.MissingModNames = new HashSet<string>();
			this.ExtraModNames = new HashSet<string>();

			if( !this.WorldModLocks.ContainsKey( modworld.ObsoleteId2 ) ) {
				this.IsMismatched = false;
				return;
			}

			ISet<string> reqModNames = this.WorldModLocks[ modworld.ObsoleteId2] ;
			ISet<string> checkedModNames = new HashSet<string>();
			IEnumerable<Mod> allMods = ModListHelpers.GetAllLoadedModsPreferredOrder();

			foreach( Mod mod in allMods ) {
				if( !reqModNames.Contains( mod.Name ) ) {
					this.ExtraModNames.Add( mod.Name );
				} else {
					this.FoundModNames.Add( mod.Name );
				}
				checkedModNames.Add( mod.Name );
			}

			foreach( string modName in reqModNames ) {
				if( !checkedModNames.Contains( modName ) ) {
					this.MissingModNames.Add( modName );
				}
			}

			this.IsMismatched = ModLockService.IsModMismatchFound();
//LogHelpers.Log( "req_modNames:"+string.Join(",",req_modNames)+
//", extra:"+string.Join(",",this.ExtraModNames)+
//", found:"+string.Join(",",this.FoundModNames)+
//", missing:"+string.Join(",",this.MissingModNames ) );
//LogHelpers.Log( "IsInitialized:"+this.IsInitialized+" IsMismatched:"+this.IsMismatched+ ", ExitDuration:" + this.ExitDuration);
		}


		////////////////

		internal void Load( TagCompound tags ) {
			if( tags.ContainsKey( "world_mod_lock_count" ) ) {
				int worldCount = tags.GetInt( "world_mod_lock_count" );

				for( int i = 0; i < worldCount; i++ ) {
					string worldUid = tags.GetString( "world_mod_lock_uid_" + i );
					int modCount = tags.GetInt( "world_mod_lock_mods_" + i + "_count" );

					this.WorldModLocks[ worldUid ] = new HashSet<string>();

					for( int j = 0; j < modCount; j++ ) {
						string modName = tags.GetString( "world_mod_lock_mods_" + i + "_" + j );
//LogHelpers.Log( "Load world_mod_lock_mods_" + i + "_" + j +": "+modName );
						this.WorldModLocks[ worldUid ].Add( modName );
					}
				}
			}
		}

		internal void Save( TagCompound tags ) {
			tags["world_mod_lock_count"] = this.WorldModLocks.Count;

			int i = 0;
			foreach( var kv in this.WorldModLocks ) {
				string worldUid = kv.Key;
				ISet<string> modNames = kv.Value;

				tags["world_mod_lock_uid_" + i] = worldUid;
				tags["world_mod_lock_mods_" + i + "_count"] = modNames.Count;

				int j = 0;
				foreach( string modName in modNames ) {
//LogHelpers.Log( "Save world_mod_lock_mods_" + i + "_" + j +": "+modName );
					tags["world_mod_lock_mods_" + i + "_" + j] = modName;
					j++;
				}
				i++;
			}
		}


		////////////////

		internal void Update() {
			if( !this.IsInitialized ) { return; }
			if( !this.IsMismatched ) { return; }
//if( (this.ExitDuration % 60) == 0 ) {LogHelpers.Log( "bye? IsInitialized:"+this.IsInitialized+" IsMismatched:"+this.IsMismatched+"," + ( this.ExitDuration / 60) );}
			
			if( Main.netMode == 2 && !this.MismatchBroadcastMade ) {
				if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
					this.MismatchBroadcastMade = true;

					int eta = this.ExitDuration / 60;
					var msg = NetworkText.FromLiteral( "World mod mismatch found. Server shutting down in " + eta + " seconds." );

					NetMessage.BroadcastChatMessage( msg, Color.Red, -1 );
				}
			}

			if( this.ExitDuration > 0 ) {
				this.ExitDuration--;
			} else {
				if( Main.netMode == 0 || Main.netMode == 1 ) {
					TmlHelpers.ExitToMenu( false );
				} else if( Main.netMode == 2 ) {
					TmlHelpers.ExitToDesktop( false );
				}
			}
		}


		////////////////

		internal void DrawWarning( SpriteBatch sb ) {
			if( !this.IsInitialized ) { return; }
			if( !this.IsMismatched ) { return; }

			int eta = this.ExitDuration / 60;
			IEnumerable<Mod> mods = ModListHelpers.GetAllLoadedModsPreferredOrder();

			string warning = "World mod mismatch! Auto-exiting in " + eta;

			Vector2 pos = new Vector2( Main.screenWidth / 2, Main.screenHeight / 2 );
			Vector2 dim = Main.fontDeathText.MeasureString( warning );
			Vector2 mainPos = pos - (dim / 2);

			sb.DrawString( Main.fontDeathText, warning, mainPos, Color.Red );

			if( this.FoundModNames.Count > 0 ) {
				string needed = "Needed mods: " + string.Join( ", ", this.FoundModNames.ToArray() );
				mainPos.X += 128;
				mainPos.Y += 48;
				sb.DrawString( Main.fontMouseText, needed, mainPos, Color.White );
			}

			if( this.MissingModNames.Count > 0 ) {
				string missing = "Missing mods: " + string.Join( ", ", this.MissingModNames.ToArray() );
				mainPos.Y += 24;
				sb.DrawString( Main.fontMouseText, missing, mainPos, Color.White );
			}

			if( this.ExtraModNames.Count > 0 ) {
				string extra = "Extra mods: " + string.Join( ", ", this.ExtraModNames.ToArray() );
				mainPos.Y += 24;
				sb.DrawString( Main.fontMouseText, extra, mainPos, Color.White );
			}
		}
	}
}
