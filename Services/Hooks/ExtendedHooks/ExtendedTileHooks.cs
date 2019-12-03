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
		private static object MyLock = new object();



		////////////////

		/// <summary>
		/// Represents a GlobalTile.KillTile hook binding.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="type"></param>
		/// <param name="fail"></param>
		/// <param name="effectOnly"></param>
		/// <param name="noItem"></param>
		public delegate void KillTileDelegate( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem );
		//public event KillTileEvent OnKillTile;



		////////////////
		
		/// <summary>
		/// Allows binding actions to the `GlobalTile.KillTile(...)` hook in such a way as to avoid stack overflow exceptions.
		/// </summary>
		/// <param name="hook"></param>
		public static void AddSafeKillTileHook( KillTileDelegate hook ) {
			ExtendedTileHooks eth;
			lock( ExtendedTileHooks.MyLock ) {
				eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();
			}

			eth.OnKillTileHooks.Add( hook );
		}
		
		/// <summary>
		/// Removes `GlobalTile.KillTile(...)` bound action hook.
		/// </summary>
		/// <param name="hook"></param>
		/// <returns></returns>
		public static bool RemoveSafeKillTileHook( KillTileDelegate hook ) {
			ExtendedTileHooks eth;
			lock( ExtendedTileHooks.MyLock ) {
				eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();
			}

			return eth.OnKillTileHooks.Remove( hook );
		}


		////////////////
		
		internal static void CallKillTileHooks( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			ExtendedTileHooks eth;
			lock( ExtendedTileHooks.MyLock ) {
				eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();
			}

			int tileToCheck = ( i << 16 ) + j;

			// Important stack overflow failsafe:
			if( eth.CheckedTiles.Contains( tileToCheck ) ) {
				return;
			}

			foreach( KillTileDelegate deleg in eth.OnKillTileHooks ) {
				deleg.Invoke( i, j, type, ref fail, ref effectOnly, ref noItem );
				eth.CheckedTiles.Add( tileToCheck );
			}
		}

		////

		private static void Update() {
			ExtendedTileHooks eth;
			lock( ExtendedTileHooks.MyLock ) {
				eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();
			}

			if( !eth.OnTick() ) {
				return;
			}

			eth.CheckedTiles.Clear();
		}
	}
}
