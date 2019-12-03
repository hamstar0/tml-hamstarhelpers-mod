using HamstarHelpers.Classes.Loadable;
using System;
using System.Collections.Generic;
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



		////////////////

		private Func<bool> OnTick;
		private ISet<KillTileDelegate> OnKillTileHooks = new HashSet<KillTileDelegate>();
		private ISet<int> CheckedTiles = new HashSet<int>();



		////////////////

		private ExtendedTileHooks() { }

		////

		/// @private
		void ILoadable.OnModsLoad() {
			this.OnTick = Timers.Timers.MainOnTickGet();
			Main.OnTick += ExtendedTileHooks.Update;
		}

		/// @private
		void ILoadable.OnModsUnload() {
			Main.OnTick -= ExtendedTileHooks.Update;
		}

		/// @private
		void ILoadable.OnPostModsLoad() {
		}
	}
}
