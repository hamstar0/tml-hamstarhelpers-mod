using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Misc {
	/// <summary>
	/// Assorted static "helper" functions pertaining to sounds.
	/// </summary>
	public class SoundHelpers {
		/// <summary>
		/// Gets volume and pan data for a sound that would play at a given point.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <returns></returns>
		public static (float Volume, float Pan) GetSoundDataFromSource( int worldX, int worldY ) {
			Rectangle maxRange = new Rectangle(
				(int)( Main.screenPosition.X - (Main.screenWidth * 2) ),
				(int)( Main.screenPosition.Y - (Main.screenHeight * 2) ),
				Main.screenWidth * 5,
				Main.screenHeight * 5 );
			Rectangle source = new Rectangle( worldX, worldY, 1, 1 );

			Vector2 screenCenter = new Vector2(
				Main.screenPosition.X + (float)Main.screenWidth * 0.5f,
				Main.screenPosition.Y + (float)Main.screenHeight * 0.5f );

			if( !source.Intersects( maxRange ) ) {
				return (0, 0);
			}

			float pan = ((float)worldX - screenCenter.X) / ((float)Main.screenWidth * 0.5f);
			float distX = Math.Abs( (float)worldX - screenCenter.X );
			float distY = Math.Abs( (float)worldY - screenCenter.Y );
			float dist = (float)Math.Sqrt( (distX * distX) + (distY * distY) );
			float vol = 1f - (dist / ((float)Main.screenWidth * 1.5f));

			return (vol, pan);
		}
	}
}
