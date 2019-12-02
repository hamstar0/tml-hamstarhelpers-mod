using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// @private
	[Obsolete( "use Attributes/TileWallHelpers", true )]
	public class TileWallHelpers {
		/// @private
		[Obsolete( "use Attributes/TileWallHelpers", true )]
		public static ISet<int> UnsafeDungeonWallTypes => Walls.TileWallHelpers.UnsafeDungeonWallTypes;

		/// @private
		[Obsolete( "use Attributes/TileWallHelpers", true )]
		public static bool IsDungeon( Tile tile, out bool isLihzahrd ) {
			return Walls.TileWallHelpers.IsDungeon( tile, out isLihzahrd );
		}
	}

	/// @private
	[Obsolete( "use Attributes/TileWallResourceHelpers", true )]
	public partial class TileWallResourceHelpers {
		/// @private
		[Obsolete( "use Attributes/TileWallHelpers", true )]
		public static Texture2D SafelyGetTexture( int type ) {
			return Walls.TileWallResourceHelpers.SafelyGetTexture( type );
		}
	}
}
