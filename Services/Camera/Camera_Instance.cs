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
		public Vector2 WorldPosition { get; private set; } = new Vector2( -1 );


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

		internal void ApplyCameraEffects() {
			if( this.WorldPosition.X < 0f ) {
				Main.screenPosition.X = this.WorldPosition.X;
			}
			if( this.WorldPosition.Y < 0f ) {
				Main.screenPosition.Y = this.WorldPosition.Y;
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


		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
