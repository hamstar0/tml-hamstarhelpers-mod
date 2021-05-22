using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader;
using HamstarHelpers.Services.Hooks.ExtendedHooks;


namespace HamstarHelpers {
	class ModHelpersTile : GlobalTile {
		public override void KillTile( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
//ModContent.GetInstance<ModHelpersMod>().Logger.Info( "KillTile1 "+type+" ("+i+","+j+") - "
//	+"fail:"+fail+", effectOnly:"+effectOnly+", noItem:"+noItem);
			var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();

			if( !Main.gameMenu || Main.netMode == NetmodeID.Server ) {
				eth.CallKillTileHooks( i, j, type, ref fail, ref effectOnly, ref noItem );
				eth.CallKillMultiTileHooks( i, j, type );
			}
			// why was CallKillMultiTileHooks here instead, previously?
		}
	}



	class ModHelpersWall : GlobalWall {
		public override void KillWall( int i, int j, int type, ref bool fail ) {
			if( !Main.gameMenu || Main.netMode == NetmodeID.Server ) {
				var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();
				eth.CallKillWallHooks( i, j, type, ref fail );
			}
		}
	}
}
