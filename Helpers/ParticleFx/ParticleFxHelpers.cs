using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace DestructibleTiles.Helpers.ParticleFx {
	/// <summary>
	/// Assorted static "helper" functions pertaining to visual particle and dust effects.
	/// </summary>
	public class ParticleFxHelpers {
		/// <summary>
		/// Creates a dust cloud.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="quantity">Dust particles.</param>
		/// <param name="sprayAmount">Outward spray velocity.</param>
		/// <param name="scale"></param>
		public static void MakeDustCloud( Vector2 position, int quantity, float sprayAmount=0.3f, float scale=1f ) {
			Vector2 pos = new Vector2( position.X - 10, position.Y - 10 );

			for( int i = 0; i < quantity; i++ ) {
				int goreType = Main.rand.Next( 61, 64 );
				int goreIdx = Gore.NewGore( pos, default( Vector2 ), goreType, scale );
				Gore gore = Main.gore[goreIdx];
				
				gore.velocity *= sprayAmount;
				gore.velocity.X += (float)Main.rand.Next( -10, 11 ) * 0.05f;
				gore.velocity.Y += (float)Main.rand.Next( -10, 11 ) * 0.05f;
			}
		}
	}
}
