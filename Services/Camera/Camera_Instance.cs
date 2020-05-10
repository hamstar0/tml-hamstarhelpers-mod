using System;
using Terraria;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers.Services.Camera {
	public partial class Camera {
		private static int OffsetX = 0;
		private static int OffsetY = 0;

		private static int WorldLeft = -1;
		private static int WorldTop = -1;

		////

		private static float ShakeMagnitude = 0f;
		private static int ShakeTickDuration = 0;
		private static int ShakeTicksElapsed = 0;



		////////////////

		internal static void ApplyCameraEffects() {
			if( Camera.WorldLeft != -1 ) {
				Main.screenPosition.X = Camera.WorldLeft;
			}
			if( Camera.WorldTop != -1 ) {
				Main.screenPosition.Y = Camera.WorldTop;
			}

			Main.screenPosition.X += Camera.OffsetX;
			Main.screenPosition.Y += Camera.OffsetY;

			if( Camera.ShakeTickDuration > 0 ) {
				if( Camera.ShakeTicksElapsed++ >= Camera.ShakeTickDuration ) {
					Camera.ShakeTicksElapsed = 0;
					Camera.ShakeTickDuration = 0;
				}

				var rand = TmlHelpers.SafelyGetRand();
				float shakeX = rand.NextFloat() * Camera.ShakeMagnitude;
				float shakeY = rand.NextFloat() * Camera.ShakeMagnitude;

				Main.screenPosition.X += shakeX - (Camera.ShakeMagnitude * 0.5f);
				Main.screenPosition.Y += shakeY - (Camera.ShakeMagnitude * 0.5f);
			}
		}
	}
}
