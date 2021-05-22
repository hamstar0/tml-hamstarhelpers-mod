using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader;
using System;
using Terraria;


namespace HamstarHelpers.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for tile-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class ExtendedTileHooks : ILoadable {
		/// <summary>
		/// Supplies a condition to check for when to skip tile killing.
		/// </summary>
		public static Func<bool> NonGameplayKillTileCondition {
			get {
				var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();
				return eth.KillTileSkipCondition;
			}
			set {
				var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();
				eth.KillTileSkipCondition = value;
			}
		}



		////////////////

		/// <summary>
		/// Allows binding actions to the `GlobalTile.KillTile(...)` hook in such a way as to avoid stack overflow exceptions.
		/// </summary>
		/// <param name="hook"></param>
		public static void AddSafeKillTileHook( KillTileDelegate hook ) {
			var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();

			lock( ExtendedTileHooks.MyLock ) {
				eth.OnKillTileHooks.Add( hook );
			}
		}


		////////////////

		/// <summary>
		/// Allows binding actions to the `GlobalWall.KillWall(...)` hook in such a way as to avoid stack overflow exceptions.
		/// </summary>
		/// <param name="hook"></param>
		public static void AddSafeWallKillHook( KillWallDelegate hook ) {
			var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();

			lock( ExtendedTileHooks.MyLock ) {
				eth.OnKillWallHooks.Add( hook );
			}
		}


		////////////////

		/// <summary>
		/// Allows binding actions to `GlobalTile.KillTile(...)` calls for tiles larger than 1x1, and only for the top left
		/// tile.
		/// </summary>
		/// <param name="hook"></param>
		public static void AddKillMultiTileHook( KillMultiTileDelegate hook ) {
			var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();

			lock( ExtendedTileHooks.MyLock ) {
				eth.OnKillMultiTileHooks.Add( hook );
			}
		}
	}
}
