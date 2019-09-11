using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.AnimatedTexture {
	/// <summary>
	/// Supplies a simple, handy way to "animate" (lerp between) colors to make animations. Adjustable.
	/// </summary>
	public partial class AnimatedTexture {
		/// <summary>
		/// Frames texture.
		/// </summary>
		public Texture2D FramesTexture { get; private set; }
		/// <summary>
		/// Total frames of this animation.
		/// </summary>
		public int MaxFrames { get; private set; }
		/// <summary>
		/// Duration (in ticks) to spend between each frame.
		/// </summary>
		public float FrameDuration { get; private set; }

		/// <summary>
		/// Percent of duration elapsed between any given 2 frames.
		/// </summary>
		public float Progress { get; private set; }
		/// <summary>
		/// Which frame in the cycle is being animated.
		/// </summary>
		public int CurrentFrame { get; private set; }



		////////////////

		/// <summary></summary>
		/// <param name="frames"></param>
		/// <param name="durationPerFrame"></param>
		/// <param name="maxFrames"></param>
		public AnimatedTexture( Texture2D frames, int durationPerFrame, int maxFrames ) {
			this.FramesTexture = frames;
			this.FrameDuration = durationPerFrame;
			this.MaxFrames = maxFrames;
			this.Progress = 0;
		}

		internal void AdvanceFrame() {
			this.Progress += 1;
			if( this.Progress >= this.FrameDuration ) {
				this.Progress = 0;

				this.CurrentFrame += 1;
				if( this.CurrentFrame >= this.MaxFrames ) {
					this.CurrentFrame = 0;
				}
			}
		}


		////////////////

		/// <summary></summary>
		public Rectangle GetFrame() {
			int frameHeight = this.FramesTexture.Height / this.MaxFrames;
			int frameY = frameHeight * this.CurrentFrame;
			return new Rectangle( 0, frameY, this.FramesTexture.Width, frameHeight );
		}


		////////////////

		/// <summary>
		/// Draws the current animator at a given screen position.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="pos"></param>
		/// <param name="color"></param>
		/// <param name="rotation"></param>
		/// <param name="scale"></param>
		/// <param name="origin"></param>
		/// <param name="spriteFx"></param>
		public void Draw( SpriteBatch sb,
				Vector2 pos,
				Color? color = null,
				float rotation=0f,
				float scale=1f,
				Vector2 origin=default(Vector2),
				SpriteEffects spriteFx=SpriteEffects.None ) {
			Rectangle rect = this.GetFrame();

			sb.Draw( this.FramesTexture,
				pos,
				new Rectangle?(rect),
				color.HasValue ? color.Value : Color.White,
				rotation,
				origin,
				scale,
				spriteFx,
				1f
			);
		}

		/// <summary>
		/// Draws the current animator at a given screen rectangle.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="destRect"></param>
		/// <param name="color"></param>
		/// <param name="rotation"></param>
		/// <param name="origin"></param>
		/// <param name="spriteFx"></param>
		public void Draw( SpriteBatch sb,
				Rectangle destRect,
				Color? color = null,
				float rotation = 0f,
				Vector2 origin = default( Vector2 ),
				SpriteEffects spriteFx = SpriteEffects.None ) {
			Rectangle frameRect = this.GetFrame();

			sb.Draw( this.FramesTexture,
				destRect,
				new Rectangle?(frameRect),
				color.HasValue ? color.Value : Color.White,
				rotation,
				origin,
				spriteFx,
				1f
			);
		}
	}
}
