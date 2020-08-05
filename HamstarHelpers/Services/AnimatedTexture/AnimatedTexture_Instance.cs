using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.Services.AnimatedTexture {
	/// <summary>
	/// Supplies a handy way to animate between frames within a given texture. Adjustable.
	/// </summary>
	public partial class AnimatedTexture {
		/// <summary>
		/// Frames texture.
		/// </summary>
		public Texture2D FramesTexture { get; private set; }
		/// <summary>
		/// Max number of frames to animate.
		/// </summary>
		public int MaxFrames { get; private set; }
		/// <summary>
		/// Function controlling how frames animate.
		/// </summary>
		public Func<AnimatedTexture, (int NextFrame, int TickDuration)> Animator { get; private set; }

		/// <summary>
		/// Elapsed ticks for the current frame.
		/// </summary>
		public int CurrentFrameTicksElapsed { get; private set; } = 0;
		/// <summary>
		/// Which frame in the cycle is being animated.
		/// </summary>
		public int CurrentFrame { get; private set; } = 0;

		private int CurrentFrameTickDuration = 0;



		////////////////

		/// <summary></summary>
		/// <param name="frameTex"></param>
		/// <param name="maxFrames"></param>
		/// <param name="animator"></param>
		public AnimatedTexture( Texture2D frameTex,
				int maxFrames,
				Func<AnimatedTexture, (int NextFrame, int TickDuration)> animator ) {
			this.FramesTexture = frameTex;
			this.MaxFrames = maxFrames;
			this.Animator = animator;

			(int NextFrame, int TickDuration) animData = animator( this );
			this.CurrentFrame = animData.NextFrame;
			this.CurrentFrameTickDuration = animData.TickDuration;
		}


		////////////////

		internal void AdvanceFrame() {
			if( this.CurrentFrameTicksElapsed++ < this.CurrentFrameTickDuration ) {
				return;
			}

			(int NextFrame, int TickDuration) animData = this.Animator( this );

			this.CurrentFrameTicksElapsed = 0;
			this.CurrentFrame = animData.NextFrame;
			this.CurrentFrameTickDuration = animData.TickDuration;
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
		/// <param name="origin"></param>
		/// <param name="scale"></param>
		/// <param name="spriteFx"></param>
		public void Draw( SpriteBatch sb,
				Vector2 pos,
				Color? color = null,
				float rotation = 0f,
				Vector2? origin = null,
				Vector2? scale = null,
				int spriteFx = 0 ) {
			Rectangle rect = this.GetFrame();

			sb.Draw( this.FramesTexture,
				pos,
				new Rectangle?(rect),
				color.HasValue ? color.Value : Color.White,
				rotation,
				origin.HasValue ? origin.Value : Vector2.One,
				scale.HasValue ? scale.Value : Vector2.One,
				(SpriteEffects)spriteFx,
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
				Vector2? origin = null,
				int spriteFx = 0 ) {
			Rectangle frameRect = this.GetFrame();

			sb.Draw( this.FramesTexture,
				destRect,
				new Rectangle?(frameRect),
				color.HasValue ? color.Value : Color.White,
				rotation,
				origin.HasValue ? origin.Value : Vector2.One,
				(SpriteEffects)spriteFx,
				1f
			);
		}
	}
}
