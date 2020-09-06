using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers.Services.Camera {
	public partial class Camera : ILoadable {
		/// <summary></summary>
		public static Camera Instance => ModContent.GetInstance<Camera>();



		////////////////

		private float OldZoomScale = -1;


		////////////////

		/// <summary></summary>
		public int? WorldPositionX { get; private set; } = null;

		/// <summary></summary>
		public int? WorldPositionY { get; private set; } = null;


		/// <summary></summary>
		public int OffsetX { get; private set; } = 0;

		/// <summary></summary>
		public int OffsetY { get; private set; } = 0;


		////

		/// <summary></summary>
		public float ShakeMagnitude { get; private set; } = 0f;


		////

		/// <summary></summary>
		public float ZoomScale { get; private set; } = -1;



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }


		////////////////

		internal void ApplyCameraEffects() {
			if( this.WorldPositionX.HasValue ) {
				Main.screenPosition.X = Math.Max( this.WorldPositionX.Value, 0 );
			}
			if( this.WorldPositionY.HasValue ) {
				Main.screenPosition.Y = Math.Max( this.WorldPositionY.Value, 0 );
			}
			Main.screenPosition.X += this.OffsetX;
			Main.screenPosition.Y += this.OffsetY;

			if( this.ShakeMagnitude > 0 ) {
				var rand = TmlHelpers.SafelyGetRand();
				float shakeX = rand.NextFloat() * this.ShakeMagnitude;
				float shakeY = rand.NextFloat() * this.ShakeMagnitude;

				Main.screenPosition.X += shakeX - (this.ShakeMagnitude * 0.5f);
				Main.screenPosition.Y += shakeY - (this.ShakeMagnitude * 0.5f);
			}
		}
	}
}
