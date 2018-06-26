using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.TmlHelpers.ModHelpers {
	public class ModLockHelpers {
		public static bool IsWorldLocked() {
			var mymod = HamstarHelpersMod.Instance;
			var modlock = mymod.ModLockHelpers;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			return modlock.WorldModLocks.ContainsKey( modworld.ObsoleteID2 );
		}

		public static bool IsModMismatchFound() {
			var mymod = HamstarHelpersMod.Instance;
			var modlock = mymod.ModLockHelpers;

			if( modlock.MissingModNames.Count > 0 ) { return true; }
			if( mymod.Config.WorldModLockMinimumOnly && modlock.ExtraModNames.Count > 0 ) { return true; }

			return false;
		}


		public static void LockWorld() {
			var mymod = HamstarHelpersMod.Instance;
			var modlock = mymod.ModLockHelpers;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			IEnumerable<Mod> all_mods = ModHelpers.GetAllMods();
			ISet<string> mod_names = new HashSet<string>();

			foreach( Mod mod in all_mods ) {
				mod_names.Add( mod.Name );
			}

			modlock.WorldModLocks[ modworld.ObsoleteID2 ] = mod_names;

			modlock.ScanMods( modworld );

			if( mymod.Config.WorldModLockMinimumOnly ) {
				Main.NewText( "Your world now requires exactly these mods: " + string.Join( ", ", mod_names ) );
			} else {
				Main.NewText( "Your world now requires at least these mods: " + string.Join( ", ", mod_names ) );
			}
		}

		public static void UnlockWorld() {
			var mymod = HamstarHelpersMod.Instance;
			var modlock = mymod.ModLockHelpers;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			modlock.WorldModLocks.Remove( modworld.ObsoleteID2 );
			modlock.MismatchBroadcastMade = false;

			modlock.ScanMods( modworld );

			Main.NewText( "Your world now has no mod requirement." );
		}



		////////////////

		internal IDictionary<string, ISet<string>> WorldModLocks { get; private set; }
		private bool IsInitialized = false;
		private bool IsMismatched = false;
		private int ExitDuration = 60 * 20;
		private bool MismatchBroadcastMade = false;

		private ISet<string> FoundModNames = new HashSet<string>();
		private ISet<string> MissingModNames = new HashSet<string>();
		private ISet<string> ExtraModNames = new HashSet<string>();


		////////////////

		internal ModLockHelpers() {
			this.WorldModLocks = new Dictionary<string, ISet<string>>();
			this.MismatchBroadcastMade = false;

			Promises.AddWorldUnloadEachPromise( this.OnWorldExit );
		}

		internal void OnWorldLoad( HamstarHelpersMod mymod, HamstarHelpersWorld modworld ) {
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

		private void ScanMods( HamstarHelpersWorld modworld ) {
			this.FoundModNames = new HashSet<string>();
			this.MissingModNames = new HashSet<string>();
			this.ExtraModNames = new HashSet<string>();

			if( !this.WorldModLocks.ContainsKey( modworld.ObsoleteID2 ) ) {
				this.IsMismatched = false;
				return;
			}

			ISet<string> req_mod_names = this.WorldModLocks[ modworld.ObsoleteID2] ;
			ISet<string> checked_mod_names = new HashSet<string>();
			IEnumerable<Mod> all_mods = ModHelpers.GetAllMods();

			foreach( Mod mod in all_mods ) {
				if( !req_mod_names.Contains( mod.Name ) ) {
					this.ExtraModNames.Add( mod.Name );
				} else {
					this.FoundModNames.Add( mod.Name );
				}
				checked_mod_names.Add( mod.Name );
			}

			foreach( string mod_name in req_mod_names ) {
				if( !checked_mod_names.Contains( mod_name ) ) {
					this.MissingModNames.Add( mod_name );
				}
			}

			this.IsMismatched = ModLockHelpers.IsModMismatchFound();
//LogHelpers.Log( "req_mod_names:"+string.Join(",",req_mod_names)+
//", extra:"+string.Join(",",this.ExtraModNames)+
//", found:"+string.Join(",",this.FoundModNames)+
//", missing:"+string.Join(",",this.MissingModNames ) );
//LogHelpers.Log( "IsInitialized:"+this.IsInitialized+" IsMismatched:"+this.IsMismatched+ ", ExitDuration:" + this.ExitDuration);
		}


		////////////////

		internal void Load( HamstarHelpersMod mymod, TagCompound tags ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( tags.ContainsKey( "world_mod_lock_count" ) ) {
				int world_count = tags.GetInt( "world_mod_lock_count" );

				for( int i = 0; i < world_count; i++ ) {
					string world_uid = tags.GetString( "world_mod_lock_uid_" + i );
					int mod_count = tags.GetInt( "world_mod_lock_mods_" + i + "_count" );

					this.WorldModLocks[ world_uid ] = new HashSet<string>();

					for( int j = 0; j < mod_count; j++ ) {
						string mod_name = tags.GetString( "world_mod_lock_mods_" + i + "_" + j );
//LogHelpers.Log( "Load world_mod_lock_mods_" + i + "_" + j +": "+mod_name );
						this.WorldModLocks[ world_uid ].Add( mod_name );
					}
				}
			}
		}

		internal void Save( HamstarHelpersMod mymod, TagCompound tags ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			tags.Set( "world_mod_lock_count", this.WorldModLocks.Count );

			int i = 0;
			foreach( var kv in this.WorldModLocks ) {
				string world_uid = kv.Key;
				ISet<string> mod_names = kv.Value;

				tags.Set( "world_mod_lock_uid_" + i, world_uid );
				tags.Set( "world_mod_lock_mods_" + i + "_count", mod_names.Count );

				int j = 0;
				foreach( string mod_name in mod_names ) {
//LogHelpers.Log( "Save world_mod_lock_mods_" + i + "_" + j +": "+mod_name );
					tags.Set( "world_mod_lock_mods_" + i + "_" + j, mod_name );
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
				var modworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();

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
			IEnumerable<Mod> mods = ModHelpers.GetAllMods();

			string warning = "World mod mismatch! Auto-exiting in " + eta;

			Vector2 pos = new Vector2( Main.screenWidth / 2, Main.screenHeight / 2 );
			Vector2 dim = Main.fontDeathText.MeasureString( warning );
			Vector2 main_pos = pos - (dim / 2);

			sb.DrawString( Main.fontDeathText, warning, main_pos, Color.Red );

			if( this.FoundModNames.Count > 0 ) {
				string needed = "Needed mods: " + string.Join( ", ", this.FoundModNames.ToArray() );
				main_pos.X += 128;
				main_pos.Y += 48;
				sb.DrawString( Main.fontMouseText, needed, main_pos, Color.White );
			}

			if( this.MissingModNames.Count > 0 ) {
				string missing = "Missing mods: " + string.Join( ", ", this.MissingModNames.ToArray() );
				main_pos.Y += 24;
				sb.DrawString( Main.fontMouseText, missing, main_pos, Color.White );
			}

			if( this.ExtraModNames.Count > 0 ) {
				string extra = "Extra mods: " + string.Join( ", ", this.ExtraModNames.ToArray() );
				main_pos.Y += 24;
				sb.DrawString( Main.fontMouseText, extra, main_pos, Color.White );
			}
		}
	}
}
