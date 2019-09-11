using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.Services.AnimatedTexture {
	/// <summary>
	/// Supplies a handy way to "animate" (lerp between) colors to make animations. Adjustable.
	/// </summary>
	public partial class AnimatedTexture {
		/// <summary>
		/// Creates an animated color object.
		/// </summary>
		/// <param name="frames"></param>
		/// <param name="durationPerFrame">Ticks of time to spend between each frame.</param>
		/// <param name="maxFrames">Number of frames to cycle through.</param>
		/// <returns></returns>
		public static AnimatedTexture Create( Texture2D frames, int durationPerFrame, int maxFrames ) {
			AnimatedTextureManager mngr = ModHelpersMod.Instance.AnimatedTextures;
			var def = new AnimatedTexture( frames, durationPerFrame, maxFrames );

			mngr.AddAnimation( def );

			return def;
		}
	}
}
