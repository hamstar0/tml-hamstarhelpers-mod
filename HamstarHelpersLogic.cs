using HamstarHelpers.TmlHelpers;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersLogic {
		internal bool HasSyncedModData = false;
		internal int StartupDelay = 0;

		public bool IsClientPlaying = false;
		public bool IsServerPlaying = false;

		public bool IsDay { get; private set; }
		public int HalfDaysElapsed { get; private set; }
		


		////////////////

		public HamstarHelpersLogic( HamstarHelpersMod mymod ) { }
		
		////////////////

		public bool IsLoaded( HamstarHelpersMod mymod ) {
//DebugHelpers.DebugHelpers.SetDisplay( "load", "HasSyncedModData: "+ this.HasSyncedModData + ", HasSetupContent: "+ mymod.HasSetupContent + ", HasCorrectID: "+ modworld.HasCorrectID+ ", HasSyncedModSettings: "+ myplayer.HasSyncedModSettings+ ", HasSyncedPlayerData: " + myplayer.HasSyncedPlayerData, 30 );
			if( !this.HasSyncedModData ) { return false; }

			if( !mymod.HasSetupContent ) { return false; }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( !modworld.HasCorrectID ) { return false; }

			if( Main.netMode == 0 || Main.netMode == 1 ) {
				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				return myplayer.HasSyncedModSettings && myplayer.HasSyncedPlayerData;
			}

			return true;
		}

		////////////////

		public void Load( HamstarHelpersMod mymod, TagCompound tags ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( tags.ContainsKey( "world_id" ) ) {
				this.HalfDaysElapsed = tags.GetInt( "half_days_elapsed_" + modworld.ObsoleteID );
			}
			
			this.HasSyncedModData = true;
		}

		public void Save( HamstarHelpersMod mymod, TagCompound tags ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			tags.Set( "half_days_elapsed_" + modworld.ObsoleteID, (int)this.HalfDaysElapsed );
		}
		
		public void LoadFromNetwork( int half_days ) {
			this.HalfDaysElapsed = half_days;

			this.HasSyncedModData = true;
		}

		
		////////////////
		
		public bool IsPlaying() {
			if( Main.netMode == 1 && !this.HasSyncedModData ) {  // Client
				return false;
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

			// Simply idle until ready (seems needed)
			if( !this.IsFullyReady() ) {
				this.IsDay = Main.dayTime;
				return;
			}

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
