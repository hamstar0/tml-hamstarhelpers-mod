using System;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.PlayerData;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public override void PostUpdateEverything() {
			CustomPlayerData.UpdateAll();
		}
	}
}
