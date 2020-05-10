using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework.Audio;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.OverlaySounds {
	/// <summary>
	/// Provides a way to create ambient sound effects that overlay existing game sounds and music.
	/// </summary>
	public partial class OverlaySound {
		/// <summary></summary>
		/// <param name="sourceMod"></param>
		/// <param name="soundPath"></param>
		/// <param name="fadeTicks"></param>
		/// <param name="playDurationTicks">If -1, sound repeats until `customCondition` says otherwise.</param>
		/// <param name="customCondition">Returns a volume scale float and a boolean to indicate if the sound has ended.</param>
		/// <returns></returns>
		public static OverlaySound Create(
				Mod sourceMod,
				string soundPath,
				int fadeTicks,
				int playDurationTicks=-1,
				Func<(float VolumeOverride, float PanOverride, bool IsEnded)> customCondition=null ) {
			return new OverlaySound( sourceMod, soundPath, fadeTicks, playDurationTicks, customCondition );
		}



		////////////////

		private Mod SourceMod;
		private string SoundPath;
		private SoundEffectInstance MyInstance = null;

		private int ElapsedTicks = 0;
		private int ElapsedFadeTicks = 0;

		private int MaxPlayDurationTicks;
		private int FadeTicks;

		private Func<(float VolumeOverride, float PanOverride, bool IsEnded)> CustomCondition = null;


		////////////////

		/// <summary></summary>
		public bool IsFadingOut { get; private set; } = false;

		/// <summary></summary>
		public bool IsEndlessLoop => this.MaxPlayDurationTicks < 0;



		////////////////

		internal OverlaySound(
				Mod sourceMod,
				string soundPath,
				int fadeTicks,
				int playDurationTicks,
				Func<(float VolumeOverride, float PanOverride, bool IsEnded)> customCondition ) {
			this.SourceMod = sourceMod;
			this.SoundPath = soundPath;
			this.FadeTicks = fadeTicks <= 1 ? 3 : fadeTicks;
			this.MaxPlayDurationTicks = playDurationTicks;
			this.CustomCondition = customCondition;
		}
	}
}
