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
		private Func<bool> OnTick;
		private ISet<KillTileDelegate> OnKillTileHooks = new HashSet<KillTileDelegate>();
		private ISet<int> CheckedTiles = new HashSet<int>();



		////////////////

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
