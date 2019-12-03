using HamstarHelpers.Services.Hooks.ExtendedHooks;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersTile : GlobalTile {
		public override void KillTile( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			if( !Main.gameMenu ) {
				ExtendedTileHooks.CallKillTileHooks( i, j, type, ref fail, ref effectOnly, ref noItem );
			}
		}
	}



	class ModHelpersWall : GlobalWall {
		public override void KillWall( int i, int j, int type, ref bool fail ) {
			if( !Main.gameMenu ) {
				ExtendedTileHooks.CallKillWallHooks( i, j, type, ref fail );
			}
		}
	}
}
