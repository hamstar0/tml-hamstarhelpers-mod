using HamstarHelpers.Classes.Loadable;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		private IDictionary<int, ISet<CustomPlayerData>> DataMap = new Dictionary<int, ISet<CustomPlayerData>>();


		////////////////

		/// <summary>
		/// Current player's `whoAmI` (`Main.player` array index) value.
		/// </summary>
		public int PlayerWho { get; private set; }

		/// <summary></summary>
		public Player Player => Main.player[ this.PlayerWho ];



		////////////////

		/// @private
		public void OnModsLoad() {
			Main.OnTick += CustomPlayerData.UpdateAll;
		}

		/// @private
		public void OnModsUnload() {
			Main.OnTick -= CustomPlayerData.UpdateAll;
		}

		/// @private
		public void OnPostModsLoad() { }
	}
}
