using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Cheats;


namespace HamstarHelpers.Internals.Logic {
	/// @private
	partial class PlayerLogic {
		public CheatModeType GetActiveCheatFlags() {
			if( !ModHelpersConfig.Instance.DebugModeCheats ) {
				return 0;
			}

			return this.ActiveCheats;
		}

		internal bool SetCheats( CheatModeType cheat ) {
			if( !ModHelpersConfig.Instance.DebugModeCheats ) {
				LogHelpers.AlertOnce( "Cheats are not enabled." );

				return false;
			}

			this.ActiveCheats = cheat;

			return true;
		}
	}
}
