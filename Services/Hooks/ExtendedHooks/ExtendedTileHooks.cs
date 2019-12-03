using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using System;
using Terraria;


namespace HamstarHelpers.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for tile-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class ExtendedTileHooks : ILoadable {
		/// <summary>
		/// Allows binding actions to the `GlobalTile.KillTile(...)` hook in such a way as to avoid stack overflow exceptions.
		/// </summary>
		/// <param name="hook"></param>
		public static void AddSafeKillTileHook( KillTileDelegate hook ) {
			ExtendedTileHooks eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();

			lock( ExtendedTileHooks.MyLock ) {
				eth.OnKillTileHooks.Add( hook );
			}
		}


		////////////////

		/// <summary>
		/// Allows binding actions to the `GlobalWall.KillWall(...)` hook in such a way as to avoid stack overflow exceptions.
		/// </summary>
		/// <param name="hook"></param>
		public static void AddSafeWallKillTileHook( KillWallDelegate hook ) {
			ExtendedTileHooks eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();

			lock( ExtendedTileHooks.MyLock ) {
				eth.OnKillWallHooks.Add( hook );
			}
		}


		////////////////

		internal static void CallKillTileHooks( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			var eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();

			int tileToCheck = (i << 16) + j;

			// Important stack overflow failsafe:
			if( eth.CheckedTiles.Contains( tileToCheck ) ) {
				return;
			}

			foreach( KillTileDelegate deleg in eth.OnKillTileHooks ) {
				lock( ExtendedTileHooks.MyLock ) {
					deleg.Invoke( i, j, type, ref fail, ref effectOnly, ref noItem );
				}
			}
			eth.CheckedTiles.Add( tileToCheck );
		}

		internal static void CallKillWallHooks( int i, int j, int type, ref bool fail ) {
			var eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();

			int wallToCheck = (i << 16) + j;

			// Important stack overflow failsafe:
			if( eth.CheckedWalls.Contains( wallToCheck ) ) {
				return;
			}

			foreach( KillWallDelegate deleg in eth.OnKillWallHooks ) {
				lock( ExtendedTileHooks.MyLock ) {
					deleg.Invoke( i, j, type, ref fail );
				}
			}
			eth.CheckedWalls.Add( wallToCheck );
		}


		////////////////

		private static void Update() {
			ExtendedTileHooks eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();

			if( !eth.OnTick() ) {
				return;
			}

			eth.CheckedTiles.Clear();
			eth.CheckedWalls.Clear();
		}
	}
}
