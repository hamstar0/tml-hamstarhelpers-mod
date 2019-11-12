﻿using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.ParticleFx {
	/// @private
	[Obsolete( "use Helpers.FX.ParticleFxHelpers", true )]
	public class ParticleFxHelpers {
		/// @private
		[Obsolete( "use Helpers.FX.ParticleFxHelpers", true )]
		public static void MakeDustCloud( Vector2 position, int quantity, float sprayAmount=0.3f, float scale=1f ) {
			FX.ParticleFxHelpers.MakeDustCloud( position, quantity, 0.3f, scale );
		}

		/// @private
		[Obsolete( "use Helpers.FX.ParticleFxHelpers", true )]
		public static void MakeFireEmbers(
				Vector2 position,
				int smallEmbers,
				int largeEmbers,
				int pouringEmbers,
				int floaters,
				int width=30,
				int height=30,
				float scale = 1f ) {
			FX.ParticleFxHelpers.MakeFireEmbers( position, smallEmbers, largeEmbers, pouringEmbers, floaters, width, height, scale );
		}

		/// @private
		[Obsolete( "use Helpers.FX.ParticleFxHelpers", true )]
		public static void MakeTeleportFx( Vector2 position, int quantity, int width=30, int height=30, float scale = 1f ) {
			FX.ParticleFxHelpers.MakeTeleportFx( position, quantity, width = 30, height = 30, scale );
		}
	}
}
