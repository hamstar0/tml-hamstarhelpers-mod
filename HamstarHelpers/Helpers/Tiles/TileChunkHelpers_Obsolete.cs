using System;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// @private
	[Obsolete("use Client service to subscribe to tile section packets", true)]
	public class TileChunkHelpers {
		/// @private
		[Obsolete("use Client service to subscribe to tile section packets", true)]
		public static bool IsTileSyncedForCurrentClient( int tileX, int tileY ) {
			int sectionX = Netplay.GetSectionX( tileX );
			int sectionY = Netplay.GetSectionY( tileY );

			if( !Main.sectionManager.SectionLoaded( sectionX, sectionY ) ) {
				Tile tile = Main.tile[tileX, tileY];
				return tile != null && !TileHelpers.IsEqual( tile, new Tile() );	// TODO
			}

			return true;
		}
	}
}
