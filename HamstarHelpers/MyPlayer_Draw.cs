using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Camera;
using HamstarHelpers.Services.Cheats;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersPlayer : ModPlayer {
		public override void ModifyScreenPosition() {
			CameraAnimationManager.Instance.ApplyAnimations();
			Camera.Instance.ApplyCameraEffects();
		}


		////////////////

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			PlayerCheats.ModifyDrawLayers( layers, this.Logic.ActiveCheats );
		}
	}
}
