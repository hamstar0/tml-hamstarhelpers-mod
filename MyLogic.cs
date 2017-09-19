using HamstarHelpers.TmlHelpers;
using Terraria;


namespace HamstarHelpers {
	class MyLogic {
		private bool IsLoaded = false;
		private int StartupDelay = 0;

		public bool IsDay { get; private set; }
		public int HalfDaysElapsed { get; private set; }

		public bool ReadyClient = false;
		public bool ReadyServer = false;



		public MyLogic( HamstarHelpersMod mymod ) { }


		public void LoadOnce( int half_days ) {
			this.IsLoaded = true;
			this.HalfDaysElapsed = half_days;
		}


		public bool IsReady() {
			if( Main.netMode == 1 && !this.IsLoaded ) {  // Client
				return false;
			}
			if( Main.netMode != 2 && !this.ReadyClient ) {  // Client or single
				return false;
			}
			if( Main.netMode == 2 && !this.ReadyServer ) {  // Server
				return false;
			}
			if( this.StartupDelay++ < (60 * 2) ) {    // UGH!!!!!!
				return false;
			}
			return true;
		}


		public void Update() {
			// Simply idle (and keep track of day) until ready
			if( !this.IsReady() ) {
				this.IsDay = Main.dayTime;
				return;
			}

			this.UpdateDay();
			AltNPCInfo.UpdateAll();
		}

		////////////////

		private void UpdateDay() {
			if( this.IsDay != Main.dayTime ) {
				this.HalfDaysElapsed++;

				if( !this.IsDay ) {
					foreach( var kv in WorldHelpers.WorldHelpers.DayHooks ) { kv.Value(); }
				} else {
					foreach( var kv in WorldHelpers.WorldHelpers.NightHooks ) { kv.Value(); }
				}
			}
			this.IsDay = Main.dayTime;
		}
	}
}
