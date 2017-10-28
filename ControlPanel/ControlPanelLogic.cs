using HamstarHelpers.TmlHelpers;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.ControlPanel {
	class ControlPanelLogic {
		public static ISet<Mod> GetTopMods() {
			return ExtendedModManager.ExtendedMods;
		}
	}
}
