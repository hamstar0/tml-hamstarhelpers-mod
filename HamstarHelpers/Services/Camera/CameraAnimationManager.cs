using System;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Camera {
	class CameraAnimationManager : ILoadable {
		public static CameraAnimationManager Instance => ModContent.GetInstance<CameraAnimationManager>();



		////////////////

		internal CameraMover CurrentMover = null;

		internal CameraZoomer CurrentZoomer = null;

		internal CameraShaker CurrentShaker = null;



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal void ApplyAnimations() {
			if( this.CurrentMover?.IsAnimating() == true ) {
				this.CurrentMover.Animate();
			}
			if( this.CurrentZoomer?.IsAnimating() == true ) {
				this.CurrentZoomer.Animate();
			}
			if( this.CurrentShaker?.IsAnimating() == true ) {
				this.CurrentShaker.Animate();
			}
		}
	}
}
