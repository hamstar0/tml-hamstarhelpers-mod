using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Helpers.Tiles.Walls;


namespace HamstarHelpers.Helpers.Tiles {
	/// @private
	[Obsolete( "use Walls/TileWallGroupIdentityHelpers", true )]
	public class TileWallHelpers {
		/// @private
		[Obsolete( "use Walls/TileWallGroupIdentityHelpers", true )]
		public static ISet<int> UnsafeDungeonWallTypes => TileWallGroupIdentityHelpers.UnsafeDungeonWallTypes;

		/// @private
		[Obsolete( "use Walls/TileWallGroupIdentityHelpers", true )]
		public static bool IsDungeon( Tile tile, out bool isLihzahrd ) {
			return TileWallGroupIdentityHelpers.IsDungeon( tile, out isLihzahrd );
		}
	}

	/// @private
	[Obsolete( "use Walls/TileWallGroupIdentityHelpers", true )]
	public partial class TileWallResourceHelpers {
		/// @private
		[Obsolete( "use Walls/TileWallGroupIdentityHelpers", true )]
		public static Texture2D SafelyGetTexture( int type ) {
			return Walls.TileWallResourceHelpers.SafelyGetTexture( type );
		}
	}
}
