using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;


namespace HamstarHelpers.Libraries.Dusts {
	/// <summary>
	/// Assorted static "helper" functions pertaining to dusts.
	/// </summary>
	public class DustLibraries {
		/// <summary></summary>
		public const int TeleportSparkleTypeID = 15;
		
		/// <summary></summary>
		public const int GoldGlitterTypeID = 269;



		////////////////

		/// <summary>
		/// Indicates if the given dust (in `Main.dust`) is active.
		/// </summary>
		/// <param name="who"></param>
		/// <returns></returns>
		public static bool IsActive( int who ) {
			return who != 6000 && Main.dust[who].active && Main.dust[who].type != 0;
		}


		/// <summary>
		/// Gets all active dusts (from `Main.dust`).
		/// </summary>
		/// <returns></returns>
		public static IList<Dust> GetActive() {
			var list = new List<Dust>();

			for( int i = 0; i < Main.dust.Length; i++ ) {
				Dust dust = Main.dust[i];
				if( dust != null && dust.active && dust.type != 0 ) {
					list.Add( dust );
				}
			}
			return list;
		}


		////////////////

		/// <summary>
		/// Quickly creates a bunch of dusts of a given type at a given location with the given settings.
		/// </summary>
		/// <param name="dustType"></param>
		/// <param name="position"></param>
		/// <param name="quantity"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="noGravity"></param>
		/// <param name="noLight"></param>
		/// <param name="speedX"></param>
		/// <param name="speedY"></param>
		/// <param name="color"></param>
		/// <param name="alpha"></param>
		/// <param name="shader"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static int[] CreateMany(
					int dustType,
					Vector2 position,
					int quantity,
					int width = 16,
					int height = 16,
					bool noGravity = false,
					bool noLight = false,
					float speedX = 0f,
					float speedY = 0f,
					Color color = default(Color),
					byte alpha = 0,
					ArmorShaderData shader = null,
					float scale = 1f ) {
			int[] dustIndices = new int[ quantity ];

			for( int i=0; i<quantity; i++ ) {
				dustIndices[i] = Dust.NewDust(
					Position: position,
					Width: width,
					Height: height,
					Type: dustType,
					SpeedX: speedX,
					SpeedY: speedY,
					Alpha: alpha,
					newColor: color,
					Scale: scale
				);

				Dust dust = Main.dust[ dustIndices[i] ];
				dust.noGravity = noGravity;
				dust.noLight = noLight;
				dust.shader = shader;
			}

			return dustIndices;
		}
	}
}
