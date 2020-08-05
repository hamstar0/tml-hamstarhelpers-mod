using System;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Supplies a set of controls for manipulating changes of state for the player's 'camera' (screen position).
	/// </summary>
	public partial class AnimatedCamera : ILoadable {
		/// <summary></summary>
		public static AnimatedCamera Instance => ModContent.GetInstance<AnimatedCamera>();



		////////////////

		/// <summary></summary>
		public int MoveXFrom { get; private set; } = -1;

		/// <summary></summary>
		public int MoveYFrom { get; private set; } = -1;

		/// <summary></summary>
		public int MoveXTo { get; private set; } = -1;

		/// <summary></summary>
		public int MoveYTo { get; private set; } = -1;

		////

		/// <summary></summary>
		public int MoveTickDuration { get; private set; } = 0;

		/// <summary></summary>
		public int MoveTicksElapsed { get; private set; } = 0;

		/// <summary></summary>
		public int MoveTicksLingerDuration { get; private set; } = 0;


		////////////////
		
		/// <summary></summary>
		public float ZoomFrom { get; private set; } = -1;

		/// <summary></summary>
		public float ZoomTo { get; private set; } = -1;

		////

		/// <summary></summary>
		public int ZoomTickDuration { get; private set; } = 0;

		/// <summary></summary>
		public int ZoomTicksElapsed { get; private set; } = 0;

		/// <summary></summary>
		public int ZoomTicksLingerDuration { get; private set; } = 0;


		////////////////

		/// <summary></summary>
		public float ShakePeakMagnitude { get; private set; } = 0f;

		/// <summary></summary>
		public int ShakeTickDuration { get; private set; } = 0;

		/// <summary></summary>
		public int ShakeTicksElapsed { get; private set; } = 0;



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal void ApplyAnimations() {
			if( this.MoveTickDuration > 0 ) {
				this.AnimateMove();
			}
			if( this.ZoomTickDuration > 0 ) {
				this.AnimateZoom();
			}
			if( this.ShakeTickDuration > 0 ) {
				this.AnimateShakes();
			}
		}
	}
}
