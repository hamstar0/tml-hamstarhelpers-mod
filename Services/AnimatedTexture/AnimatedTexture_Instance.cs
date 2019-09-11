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
		public Func<AnimatedTexture, (int NextFrame, int TickDelay)> Animator { get; private set; }

		/// <summary>
		/// Elapsed ticks for the current frame.
		/// </summary>
		public int TicksElapsed = 0;
		/// <summary>
		/// Which frame in the cycle is being animated.
		/// </summary>
		public int CurrentFrame = 0;

		private int CurrentFrameMaxTicks = 0;



		////////////////

		/// <summary></summary>
		/// <param name="frames"></param>
		/// <param name="maxFrames"></param>
		/// <param name="animator"></param>
		public AnimatedTexture( Texture2D frames,
				int maxFrames,
				Func<AnimatedTexture, (int NextFrame, int TickDelay)> animator ) {
			this.FramesTexture = frames;
			this.MaxFrames = maxFrames;
			this.Animator = animator;

			(int NextFrame, int TickDelay) animData = animator( this );
			this.CurrentFrame = animData.NextFrame;
			this.CurrentFrameMaxTicks = animData.TickDelay;
		}

		internal void AdvanceFrame() {
			this.TicksElapsed += 1;
			if( this.TicksElapsed < this.CurrentFrameMaxTicks ) {
				return;
			}

			(int NextFrame, int TickDelay) animData = this.Animator( this );
			this.TicksElapsed = 0;
			this.CurrentFrameMaxTicks = animData.TickDelay;
			this.CurrentFrame = animData.NextFrame;
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
		public void Draw( SpriteBatch sb,
				Vector2 pos,
				Color? color = null ) {
				/*float rotation = 0f,
				float scale = 1f,
				Vector2 origin = default( Vector2 ),
				SpriteEffects spriteFx = SpriteEffects.None*/
			Rectangle rect = this.GetFrame();

			sb.Draw( this.FramesTexture,
				pos,
				new Rectangle?(rect),
				color.HasValue ? color.Value : Color.White//,
				//rotation,
				//origin,
				//scale,
				//spriteFx,
				//1f
			);
		}

		/// <summary>
		/// Draws the current animator at a given screen rectangle.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="destRect"></param>
		/// <param name="color"></param>
		public void Draw( SpriteBatch sb,
				Rectangle destRect,
				Color? color = null ) {
				/*float rotation = 0f,
				Vector2 origin = default( Vector2 ),
				SpriteEffects spriteFx = SpriteEffects.None*/
			Rectangle frameRect = this.GetFrame();

			sb.Draw( this.FramesTexture,
				destRect,
				new Rectangle?(frameRect),
				color.HasValue ? color.Value : Color.White//,
				//rotation,
				//origin,
				//spriteFx,
				//1f
			);
		}
	}
}
