using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Fx {
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
			float offset = 10f * scale;
			var pos = new Vector2( position.X - offset, position.Y - offset );

			for( int i = 0; i < quantity; i++ ) {
				int goreType = Main.rand.Next( 61, 64 );
				int goreIdx = Gore.NewGore( pos, default(Vector2), goreType, scale );
				Gore gore = Main.gore[goreIdx];
				
				gore.velocity *= sprayAmount;
				gore.velocity.X += (float)Main.rand.Next( -10, 11 ) * 0.05f;
				gore.velocity.Y += (float)Main.rand.Next( -10, 11 ) * 0.05f;
			}
		}


		/// <summary>
		/// Creates fire embers.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="smallEmbers"></param>
		/// <param name="largeEmbers"></param>
		/// <param name="pouringEmbers"></param>
		/// <param name="floaters"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="scale"></param>
		public static void MakeFireEmbers(
					Vector2 position,
					int smallEmbers,
					int largeEmbers,
					int pouringEmbers,
					int floaters,
					int width=30,
					int height=30,
					float scale = 1f ) {
			int idx;
			Dust dust;
			void makeDust( int type, int num ) {
				for( int i = 0; i < num; i++ ) {
					idx = Dust.NewDust(
						Position: position,
						Width: width,
						Height: height,
						Type: type,
						SpeedX: 0f,
						SpeedY: 0f,
						Alpha: 0,
						newColor: new Color( 255, 255, 255 ),
						Scale: scale
					);
					dust = Main.dust[idx];
				}
			}

			makeDust( 6, smallEmbers );
			makeDust( 271, largeEmbers );
			makeDust( 170, pouringEmbers );
			makeDust( 259, floaters );
		}

		/// <summary>
		/// Creates teleportation particles.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="quantity">Dust particles.</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="scale"></param>
		public static void MakeTeleportFx( Vector2 position, int quantity, int width=30, int height=30, float scale = 1f ) {
			int idx;
			//Dust dust;
			void makeDust( int type, int num ) {
				for( int i = 0; i < num; i++ ) {
					idx = Dust.NewDust(
						Position: position,
						Width: width,
						Height: height,
						Type: type,
						SpeedX: 0f,
						SpeedY: 0f,
						Alpha: 0,
						newColor: new Color( 255, 255, 255 ),
						Scale: scale
					);
					//dust = Main.dust[idx];
				}
			}

			makeDust( 15, quantity );
		}


		/* TODO: Test particles
		public static void MakeSparks( Vector2 position, int quantity, int width=30, int height=30, float scale = 1f ) {
			int idx;
			Dust dust;
			void makeDust( int type, int num ) {
				for( int i = 0; i < num; i++ ) {
					idx = Dust.NewDust(
						Position: position,
						Width: width,
						Height: height,
						Type: type,
						SpeedX: 0f,
						SpeedY: 0f,
						Alpha: 0,
						newColor: new Color( 255, 255, 255 ),
						Scale: scale
					);
					dust = Main.dust[idx];
				}
			}
			
			// Generic, largish sprays
			makeDust( 246, quantity );

			// Welding torch (with streamers)
			makeDust( 222, quantity );

			// Generic, smallish sparks
			makeDust( 204, quantity );

			// Small, rapid sparks
			makeDust( 162, quantity );

			// Small, light falling sparks (emits)
			makeDust( 158, quantity );

			// Large, puffy, falling sparks
			makeDust( 87, quantity );

			// Very small, light falling sparks (emits)
			makeDust( 64, quantity );

			// Floating yellow sparks
			makeDust( 269, quantity );
		}*/

		// 44: Toxic spores; lingers a bit (emits)
		// 55: Flame-like sparking outward bigger puffs (emits)
		// 57: Golden spark outward puffs (emits)
		// 182: Red, lightless falling sparks
		// 252, 253: Plump water
		// 239: Dank water
	}
}
