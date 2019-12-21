using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Hooks.ExtendedHooks;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersTile : GlobalTile {
		public override void KillTile( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			if( !Main.gameMenu ) {
				var eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();
				eth.CallKillTileHooks( i, j, type, ref fail, ref effectOnly, ref noItem );
				eth.CallKillMultiTileHooks( i, j, type );
			}
		}
	}



	class ModHelpersWall : GlobalWall {
		public override void KillWall( int i, int j, int type, ref bool fail ) {
			if( !Main.gameMenu ) {
				var eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();
				eth.CallKillWallHooks( i, j, type, ref fail );
			}
		}
	}
}
