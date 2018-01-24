using HamstarHelpers.TmlHelpers;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Logic {
	class WorldLogic {
		internal int StartupDelay = 0;

		public bool IsClientPlaying = false;
		public bool IsServerPlaying = false;

		public bool IsDay { get; private set; }
		public int HalfDaysElapsed { get; private set; }
		


		////////////////

		public WorldLogic( HamstarHelpersMod mymod ) { }
		
		////////////////

		public bool IsLoaded( HamstarHelpersMod mymod ) {
//var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
//var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
//LogHelpers.Log( "HasSetupContent: "+ mymod.HasSetupContent + ", HasCorrectID: "+ modworld.HasCorrectID+ ", who: "+myplayer.player.whoAmI+", HasSyncedModSettings: "+ myplayer.Logic.HasSyncedModData+", HasSyncedModData: " + myplayer.Logic.HasSyncedModData );
			if( !mymod.HasSetupContent ) { return false; }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( !modworld.HasCorrectID ) { return false; }

			if( Main.netMode == 0 || Main.netMode == 1 ) {
				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				return myplayer.Logic.IsSynced();
			}

			return true;
		}

		////////////////

		public void LoadForWorld( HamstarHelpersMod mymod, TagCompound tags ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( tags.ContainsKey( "world_id" ) ) {
				this.HalfDaysElapsed = tags.GetInt( "half_days_elapsed_" + modworld.ObsoleteID );
			}
		}

		public void SaveForWorld( HamstarHelpersMod mymod, TagCompound tags ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			tags.Set( "half_days_elapsed_" + modworld.ObsoleteID, (int)this.HalfDaysElapsed );
		}
		
		public void LoadFromNetwork( int half_days ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			this.HalfDaysElapsed = half_days;

			myplayer.Logic.FinishModDataSync();
		}

		
		////////////////
		
		public bool IsPlaying() {
			if( Main.netMode == 1 ) {
				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				if( !myplayer.Logic.HasSyncedModData ) {  // Client
					return false;
				}
			}
			if( Main.netMode != 2 && !this.IsClientPlaying ) {  // Client or single
				return false;
			}
			if( Main.netMode == 2 && !this.IsServerPlaying ) {  // Server
				return false;
			}
			return true;
		}

		public bool IsFullyReady() {
			if( !this.IsPlaying() ) {
				return false;
			}
			if( this.StartupDelay++ < ( 60 * 2 ) ) {    // Seems needed for day/night tracking (and possibly other things?)
				return false;
			}
			return true;
		}


		////////////////

		public void Update( HamstarHelpersMod mymod ) {
			if( !this.IsLoaded( mymod ) ) {
				return;
			}

			if( this.IsPlaying() ) {
				mymod.ControlPanel.UpdateModList();
			}

			mymod.AnimatedColors.Update();

			// Simply idle until ready (seems needed)
			if( !this.IsFullyReady() ) {
				this.IsDay = Main.dayTime;
				return;
			} else {
				this.UpdateLoaded( mymod );
			}
		}


		private void UpdateLoaded( HamstarHelpersMod mymod ) {
			this.UpdateDay( mymod );

			mymod.ModLockHelpers.Update();
			
			AltProjectileInfo.UpdateAll();
			AltNPCInfo.UpdateAll();
		}

		////////////////

		private void UpdateDay( HamstarHelpersMod mymod ) {
			if( this.IsDay != Main.dayTime ) {
				this.HalfDaysElapsed++;

				if( !this.IsDay ) {
					foreach( var kv in mymod.WorldHelpers.DayHooks ) { kv.Value(); }
				} else {
					foreach( var kv in mymod.WorldHelpers.NightHooks ) { kv.Value(); }
				}
			}
			this.IsDay = Main.dayTime;
		}
	}
}
