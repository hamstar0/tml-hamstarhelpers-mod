using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.TModLoader;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Fx {
	public partial class CameraHelpers : ILoadable {
		private int OffsetX = 0;
		private int OffsetY = 0;

		private int WorldLeft = -1;
		private int WorldTop = -1;

		////

		private float ShakeMagnitude = 0f;
		private int ShakeTickDuration = 0;
		private int ShakeTicksElapsed = 0;



		////////////////

		/// @private
		public void OnModsLoad() { }

		/// @private
		public void OnModsUnload() { }

		/// @private
		public void OnPostModsLoad() { }


		////////////////

		internal void ApplyCameraEffects() {
			if( this.WorldLeft != -1 ) {
				Main.screenPosition.X = this.WorldLeft;
			}
			if( this.WorldTop != -1 ) {
				Main.screenPosition.Y = this.WorldTop;
			}

			Main.screenPosition.X += this.OffsetX;
			Main.screenPosition.Y += this.OffsetY;

			if( this.ShakeTickDuration > 0 ) {
				if( this.ShakeTicksElapsed++ >= this.ShakeTickDuration ) {
					this.ShakeTicksElapsed = 0;
					this.ShakeTickDuration = 0;
				}

				var rand = TmlHelpers.SafelyGetRand();
				float shakeX = rand.NextFloat() * this.ShakeMagnitude;
				float shakeY = rand.NextFloat() * this.ShakeMagnitude;

				Main.screenPosition.X += shakeX - (this.ShakeMagnitude * 0.5f);
				Main.screenPosition.Y += shakeY - (this.ShakeMagnitude * 0.5f);
			}
		}
	}
}
